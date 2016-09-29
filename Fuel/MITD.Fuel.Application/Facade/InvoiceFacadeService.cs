using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Application.Service.Security;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public partial class InvoiceFacadeService : IInvoiceFacadeService
    {
        private readonly IInvoiceDomainService invoiceDomainService;
        private readonly IInvoiceRepository invoiceRepository;
        private readonly IGoodUnitConvertorDomainService goodUnitConvertorDomainService;
        private readonly IMainUnitVlaueTomainUnitVlaueDtoMapper mainUnitVlaueTomainUnitVlaueDtoMapper;
        private readonly IInvoiceItemDomainService invoiceItemDomainService;
        private readonly IEffectiveFactorMapper effectiveFactorMapper;
        private readonly IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService;
        private readonly IBalanceDomainService balanceDomainService;

        #region props

        private readonly IInvoiceApplicationService invoiceAppService;
        private readonly IInvoiceToDtoMapper invoiceDtoMapper;
        private readonly IInvoiceItemToDtoMapper itemToDtoMapper;
        private readonly IFuelUserRepository fuelUserRepository;

        #endregion

        #region ctor

        //public InvoiceFacadeService()
        //{
        //    try
        //    {
        //        ServiceLocator.Current.GetInstance<IInvoiceDomainService>();
        //        ServiceLocator.Current.GetInstance<IInvoiceApplicationService>();
        //        ServiceLocator.Current.GetInstance<IInvoiceToDtoMapper>();
        //        ServiceLocator.Current.GetInstance<IInvoiceItemToDtoMapper>();
        //        ServiceLocator.Current.GetInstance<IInvoiceRepository>();
        //        ServiceLocator.Current.GetInstance<IGoodUnitConvertorDomainService>();
        //        ServiceLocator.Current.GetInstance<IMainUnitVlaueTomainUnitVlaueDtoMapper>();
        //        ServiceLocator.Current.GetInstance<IUnitOfWorkScope>();
        //        ServiceLocator.Current.GetInstance<IInvoiceItemDomainService>();
        //        ServiceLocator.Current.GetInstance<IEffectiveFactorMapper>();
        //        ServiceLocator.Current.GetInstance<IInvoiceAdditionalPriceDomainService>();

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}
        public InvoiceFacadeService(IInvoiceDomainService invoiceDomainService,
            IInvoiceApplicationService invoiceAppService,
            IInvoiceToDtoMapper invoiceDtoMapper,
            IInvoiceItemToDtoMapper itemToDtoMapper,
            IInvoiceRepository invoiceRepository,
            IGoodUnitConvertorDomainService goodUnitConvertorDomainService,
            IMainUnitVlaueTomainUnitVlaueDtoMapper mainUnitVlaueTomainUnitVlaueDtoMapper,
            IInvoiceItemDomainService invoiceItemDomainService,
            IEffectiveFactorMapper effectiveFactorMapper,
            IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService,
            IBalanceDomainService balanceDomainService, IFuelUserRepository fuelUserRepository)
        {
            this.invoiceDomainService = invoiceDomainService;
            this.invoiceRepository = invoiceRepository;
            this.goodUnitConvertorDomainService = goodUnitConvertorDomainService;
            this.mainUnitVlaueTomainUnitVlaueDtoMapper = mainUnitVlaueTomainUnitVlaueDtoMapper;
            this.invoiceItemDomainService = invoiceItemDomainService;

            this.effectiveFactorMapper = effectiveFactorMapper;
            this.invoiceAdditionalPriceDomainService = invoiceAdditionalPriceDomainService;
            this.balanceDomainService = balanceDomainService;
            this.fuelUserRepository = fuelUserRepository;
            this.invoiceAppService = invoiceAppService;
            this.invoiceDtoMapper = invoiceDtoMapper;
            this.itemToDtoMapper = itemToDtoMapper;
        }

        #endregion

        #region methods

        public InvoiceDto Add(InvoiceDto data)
        {
            var invoice = invoiceDtoMapper.MapModelToCommandWithAllIncludes(data);
            var added = invoiceAppService.Add(invoice);
            //                (
            //                    data.InvoiceNumber, data.OwnerId, data.InvoiceDate, (DivisionMethods) data.DivisionMethodId, (AccountingTypes) data.AccountTypeId,
            //                    data.InvoiceRefrence==null?(long?) null:data.InvoiceRefrence.Id, data.OrderRefrences.Select(c => c.Id).ToList(), data.CurrencyId,
            //                    data.TransporterId, data.SupplierId, (InvoiceTypes) (int) data.InvoiceType, data.Description,data.InvoiceItems.ToList());

            var result = invoiceDtoMapper.MapToModel(added);

            return result;
        }



        //TODO Sholde Check Type
        public InvoiceDto Update(InvoiceDto data)
        {
            var invoice = invoiceDtoMapper.MapModelToCommandWithAllIncludes(data);
            var updatedEnt = invoiceAppService.Update(invoice);
            var result = invoiceDtoMapper.MapToModel(updatedEnt);
            return result;
        }
        public InvoiceDto CalculateAdditionalPrice(InvoiceDto data)
        {
            var invoice = invoiceDtoMapper.MapModelToCommandWithAllIncludes(data);
            var updatedEnt = invoiceAppService.CalculateAdditionalPrice(invoice, getFuelUserId());
            var result = invoiceDtoMapper.MapToModelWithAllIncludes(updatedEnt);
            result.Id = data.Id;  //This calculation might be done for both new and saved invoice objects, so the id from presentation must be reverted back to it for saved invoices.
            return result;
        }

        public void Delete(InvoiceDto data)
        {
            invoiceAppService.DeleteById(data.Id);
        }

        public InvoiceDto GetById(long id)
        {
            var fetch = new SingleResultFetchStrategy<Invoice>()
                .Include(o => o.InvoiceItems)
                .Include(o => o.Supplier)
                .Include(o => o.Transporter)
                .Include(o => o.Owner)
                .Include(o => o.InvoiceRefrence)
                .Include(o => o.OrderRefrences)
                .Include(o => o.ApproveWorkFlows)
                .Include(c => c.AdditionalPrices);


            var invoicesPageResult = invoiceRepository.Single(o => o.Id == id, fetch);

            var invoiceDtos = invoiceDtoMapper.MapToModelWithAllIncludes(invoicesPageResult);

            return invoiceDtos;

            //var ent = Data.FirstOrDefault(e => e.Id == id);
            //return ent;
        }

        // todo: input parameters must be converted to IPageCritera
        public PageResultDto<InvoiceDto> GetAll(int pageSize, int pageIndex)
        {
            var fetch = new ListFetchStrategy<Invoice>().WithPaging(pageSize, pageIndex);
            invoiceRepository.GetAll(fetch);

            var finalResult = new PageResultDto<InvoiceDto>
                                  {
                                      CurrentPage = pageIndex,
                                      PageSize = pageSize,
                                      Result = invoiceDtoMapper.MapToModel(fetch.PageCriteria.PageResult.Result.ToList()).ToList(),
                                      TotalCount = fetch.PageCriteria.PageResult.TotalCount,
                                      TotalPages = fetch.PageCriteria.PageResult.TotalPages
                                  };

            return finalResult;
        }

        public void DeleteById(int id)
        {
            invoiceAppService.DeleteById(id);
        }


        public PageResultDto<InvoiceDto> GetByFilter(long? companyId, DateTime? fromDate, DateTime? toDate, string invoiceIds, string invoiceItemIds, string invoiceNumber, long? vesselInCompanyId, string orderNumbers, InvoiceTypeEnum? invoiceType, int pageSize, int pageIndex, bool submitedState)
        {
            var toDateParam = toDate == null ? DateTime.MaxValue : toDate.Value.Date.AddDays(1);
            var fromDateParam = fromDate == null ? DateTime.MinValue : fromDate.Value.Date;

            var fetch = new ListFetchStrategy<Invoice>()
                .Include(o => o.InvoiceItems)
                .Include(o => o.Supplier)
                .Include(o => o.Transporter)
                .Include(o => o.Owner)
                .Include(o => o.ApproveWorkFlows)
                .OrderByDescending(p => p.InvoiceDate)
                .WithPaging(pageSize, pageIndex + 1);

            var invoiceIdsList = string.IsNullOrWhiteSpace(invoiceIds) ? new List<long>() : invoiceIds.Split(',').Select(long.Parse).ToList();
            var invoiceItemIdsList = string.IsNullOrWhiteSpace(invoiceItemIds) ? new List<long>() : invoiceItemIds.Split(',').Select(long.Parse).ToList();
            var orderNumbersList = string.IsNullOrWhiteSpace(orderNumbers) ? new List<string>() : new List<string>(orderNumbers.Split(',').Select(s=>s.Trim()));

            // var invoiceType = _invoiceDtoMapper.MapInvoiceTypeDtoToInvoiceTypeEntity(invoiceTypeDto);
            invoiceRepository.Find(i =>
                        (string.IsNullOrEmpty(invoiceNumber) || i.InvoiceNumber.Contains(invoiceNumber)) &&
                        (!submitedState || i.State == States.Submitted) &&
                        (companyId == null || companyId == -1 || i.OwnerId == companyId) &&
                        (i.InvoiceDate >= fromDateParam) && (i.InvoiceDate <= toDateParam) &&
                        (!invoiceIdsList.Any() || invoiceIdsList.Contains(i.Id)) &&
                        (!invoiceItemIdsList.Any() || i.InvoiceItems.Any(item => invoiceItemIdsList.Contains(item.Id))) &&
                        (!orderNumbersList.Any() || i.OrderRefrences.Any(or => orderNumbersList.Contains(or.Code)) || i.InvoiceRefrence.OrderRefrences.Any(or => orderNumbersList.Contains(or.Code))) &&
                        (!invoiceType.HasValue || i.InvoiceType == (InvoiceTypes)(int)invoiceType.Value) &&
                        (!vesselInCompanyId.HasValue || i.OrderRefrences.Any(or=>or.ToVesselInCompanyId.HasValue ? or.ToVesselInCompanyId == vesselInCompanyId : or.OrderItems.Any(ori=>ori.OrderItemBalances.Any(orib=>orib.FuelReportDetail.FuelReport.VesselInCompanyId == vesselInCompanyId))))
                        ,
                    fetch);

            var invoicesPageResult = fetch.PageCriteria.PageResult;
            
            var result = new PageResultDto<InvoiceDto>
                             {
                                 CurrentPage = invoicesPageResult.CurrentPage,
                                 PageSize = invoicesPageResult.PageSize,
                                 Result = invoiceDtoMapper.MapToModelWithAllIncludes(invoicesPageResult.Result).ToList(),
                                 TotalCount = invoicesPageResult.TotalCount,
                                 TotalPages = invoicesPageResult.TotalPages,
                             };
            return result;
        }

        //================================================================================


        private long getFuelUserId()
        {
            var currentUserId = SecurityApplicationService.GetCurrentUserId();
            var currentCompanyId = SecurityApplicationService.GetCurrentUserCompanyId();

            return fuelUserRepository.Single(fu => fu.IdentityId == currentUserId && fu.CompanyId == currentCompanyId).Id;
        }


        //================================================================================

        #endregion

        #region InvoiceItem

        //        public InvoiceItemDto UpdateItem(InvoiceItemDto data)
        //        {
        //
        //            return _itemToDtoMapper.MapEntityToDto(
        //                _invoiceAppService.UpdateItem(
        //                data.Id, data.InvoiceId, data.Fee, data.Quantity, data.Description));
        //
        //
        //
        //        }
        //
        //        public void DeleteItem(InvoiceItemDto data)
        //        {
        //            this._invoiceAppService.DeleteItem(data.InvoiceId, data.Id);
        //        }

        public IEnumerable<InvoiceItemDto> GenerateInvoiceItemForOrders(string strOrderList)
        {
            var orderList = strOrderList.Split(',').ToList().Select(long.Parse).ToList();
            var invoiceItemList = balanceDomainService.GenerateInvoiceItemFromOrders(orderList);
            return itemToDtoMapper.MapEntityToDto(invoiceItemList);
        }

        public IEnumerable<EffectiveFactorDto> GetAllEffectiveFactors()
        {
            var factors = invoiceRepository.GetAllEffectiveFactors();
            return this.effectiveFactorMapper.MapToModel(factors);
        }


        public InvoiceItemDto GetInvoiceItemById(long invoiceId, long invoiceItemId)
        {
            var invoice = this.invoiceRepository.FindByKey(invoiceId);
            var invoiceItem = invoice.InvoiceItems.SingleOrDefault(c => c.Id == invoiceItemId);
            return itemToDtoMapper.MapEntityToDto(invoiceItem);
        }

        public MainUnitValueDto GetGoodMainUnit(long goodId, long goodUnitId, decimal value)
        {
            return mainUnitVlaueTomainUnitVlaueDtoMapper.MapToModel(goodUnitConvertorDomainService.GetUnitValueInMainUnit(goodId, goodUnitId, value));
        }

        #endregion
    }

}