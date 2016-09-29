#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Claims;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.DomainService;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Factories;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.FuelSecurity.Domain.Model.Repository;
using MITD.Fuel.Application.Service.Security;

#endregion

namespace MITD.Fuel.Application.Service
{
    public class InvoiceApplicationService : IInvoiceApplicationService
    {
        private readonly ICompanyDomainService companyDomainService;
        private readonly IInvoiceDomainService invoiceDomainService;
        private readonly IGoodDomainService goodDomainService;
        private readonly IInvoiceFactory invoiceFactory;
        private readonly ICompanyRepository companyRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IGoodRepository goodRepository;
        private readonly ICurrencyDomainService currencyDomainService;
        private readonly IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService;
        private readonly IInvoiceItemDomainService invoiceItemDomainService;
        private readonly IEffectiveFactorDomainService effectiveFactorDomainService;
        private readonly IInvoiceRepository invoiceRepository;
        private readonly IUnitOfWorkScope unitOfWorkScope;
        private readonly IVesselInCompanyDomainService vesselDomainService;
        //    private readonly IApprovableDomainService _approvableDomainService;
        //    private readonly IGoodPartyAssignmentDomainService _goodPartyAssignmentDomainService;

        private readonly IGoodUnitConvertorDomainService goodUnitConvertorDomainService;
        private IBalanceDomainService balanceDomainService;
        private IEntityConfigurator<Invoice> invoiceConfigurator;

        public InvoiceApplicationService(IInvoiceRepository invoiceRepository,
                                         IUnitOfWorkScope unitOfWorkScope,
            //  IGoodPartyAssignmentDomainService goodPartyAssignmentDomainService,
            //IUserRepository userRepository,
                                         IVesselInCompanyDomainService vesselDomainService,
                                         IGoodDomainService goodDomainService,
                                         IInvoiceFactory invoiceFactory,
                                         ICompanyDomainService companyDomainService,
                                         IInvoiceDomainService invoiceDomainService,
                                         IInvoiceItemDomainService invoiceItemDomainService,
                                         IEffectiveFactorDomainService effectiveFactorDomainService,
                                         ICompanyRepository companyRepository
            //,IApprovableDomainService approvableDomainService
                                         ,
                                         IOrderRepository orderRepository,
                                         IGoodRepository goodRepository,
                                         ICurrencyDomainService currencyDomainService,
                                         IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService,
                                         IGoodUnitConvertorDomainService goodUnitConvertorDomainService, IBalanceDomainService balanceDomainService, IEntityConfigurator<Invoice> invoiceConfigurator)
        {
            this.invoiceRepository = invoiceRepository;
            this.vesselDomainService = vesselDomainService;
            this.goodDomainService = goodDomainService;
            this.invoiceFactory = invoiceFactory;
            this.unitOfWorkScope = unitOfWorkScope;
            this.companyDomainService = companyDomainService;
            this.invoiceDomainService = invoiceDomainService;

            this.invoiceItemDomainService = invoiceItemDomainService;
            this.effectiveFactorDomainService = effectiveFactorDomainService;
            this.companyRepository = companyRepository;
            this.orderRepository = orderRepository;
            this.goodRepository = goodRepository;
            this.currencyDomainService = currencyDomainService;
            this.invoiceAdditionalPriceDomainService = invoiceAdditionalPriceDomainService;
            this.goodUnitConvertorDomainService = goodUnitConvertorDomainService;
            this.balanceDomainService = balanceDomainService;
            this.invoiceConfigurator = invoiceConfigurator;


            // _approvableDomainService = approvableDomainService;
            //  _goodPartyAssignmentDomainService = goodPartyAssignmentDomainService;
        }

        #region Operations

        public Invoice Add(InvoiceCommand invoiceCommand)
        {
            var userId = SecurityApplicationService.GetCurrentUserId();
            var companyId = SecurityApplicationService.GetCurrentUserCompanyId();

            //ClaimsPrincipal.Current.Claims.Select(c=>c.Type) ["CurrentUserId"];
            //ClaimsPrincipal.Current.Claims["CurrentUserCompanyId"];

            List<Order> orderRefrences = GetOrderRefrences(invoiceCommand.OrdersRefrenceId);
            Invoice invoiceRefrence = GetInvoiceRefrence(invoiceCommand.InvoiceRefrenceId);

            var transporter = invoiceCommand.TransporterId == null ? null : companyRepository.Single(c => c.Id == invoiceCommand.TransporterId);
            var supplier = invoiceCommand.SupplierId == null ? null : companyRepository.Single(c => c.Id == invoiceCommand.SupplierId);
            var owner = companyRepository.Single(c => c.Id == invoiceCommand.OwnerId);
            if (owner == null)
                throw new ObjectNotFound("Company", invoiceCommand.OwnerId);

            var currency = currencyDomainService.Get(invoiceCommand.CurrencyId);
            if (currency == null)
                throw new ObjectNotFound("Currency", invoiceCommand.CurrencyId);

            var goods = goodDomainService.GetCompanyGoodsWithUnits(invoiceCommand.OwnerId);


            var invoiceItems = GetInvoiceItems(invoiceCommand, goods);

            var invoiceAdditionalPriceList = GetAdditionalPrice(invoiceCommand);

            var invoice = invoiceFactory.CreateInvoiceObject
                (
                    invoiceCommand, goods, owner, transporter, supplier, invoiceRefrence, orderRefrences, currency, invoiceItems,
                    invoiceAdditionalPriceList, false,
                    userId);

            invoice.CheckInvoiceNumberToBeUnique(invoiceDomainService);

            invoiceRepository.Add(invoice);

            unitOfWorkScope.Commit();

            return invoice;
        }

        public Invoice Update(InvoiceCommand invoiceCommand)
        {
            var invoice = invoiceRepository.GetConfiguredInvoice(invoiceCommand.Id, 
                new SingleResultFetchStrategy<Invoice>()
                   .Include(c => c.InvoiceItems)
                   .Include(c => c.OrderRefrences),
                invoiceConfigurator);

            if (invoice == null)
                throw new ObjectNotFound("Invoice", invoiceCommand.Id);

            List<Order> orderRefrences = GetOrderRefrences(invoiceCommand.OrdersRefrenceId);
            Invoice invoiceRefrence = GetInvoiceRefrence(invoiceCommand.InvoiceRefrenceId);

            var transporter = invoiceCommand.TransporterId == null ? null : companyRepository.Single(c => c.Id == invoiceCommand.TransporterId);
            var supplier = invoiceCommand.SupplierId == null ? null : companyRepository.Single(c => c.Id == invoiceCommand.SupplierId);
            var owner = companyRepository.Single(c => c.Id == invoiceCommand.OwnerId);
            if (owner == null)
                throw new ObjectNotFound("Company", invoiceCommand.OwnerId);

            var currency = currencyDomainService.Get(invoiceCommand.CurrencyId);
            if (currency == null)
                throw new ObjectNotFound("Currency", invoiceCommand.CurrencyId);

            var goods = goodDomainService.GetCompanyGoodsWithUnits(invoiceCommand.OwnerId);

            if (invoiceCommand.InvoiceType != invoice.InvoiceType)
                throw new InvalidOperation("Update", "Can not Update Invoice Type");

            var invoiceItems = GetInvoiceItems(invoiceCommand, goods);

            var invoiceAdditionalPriceList = GetAdditionalPrice(invoiceCommand);


            invoice.Update
                (
                    invoiceCommand.InvoiceNumber, invoiceCommand.InvoiceDate, invoiceCommand.DivisionMethod, invoiceRefrence, orderRefrences, currency,
                    invoiceCommand.IsCreditor, transporter, supplier, invoiceCommand.Description, invoiceItems, invoiceAdditionalPriceList,
                    invoiceDomainService, invoiceItemDomainService, goodUnitConvertorDomainService, invoiceAdditionalPriceDomainService, balanceDomainService);


            invoiceRepository.Update(invoice);

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("Invoice");
            }
            return invoice;
        }

        public Invoice CalculateAdditionalPrice(InvoiceCommand invoiceCommand, long userId)
        {
            //var invoice = new Invoice();
            //if (invoiceCommand.Id != 0)
            //{
            //    invoice = invoiceRepository.GetConfiguredInvoice(invoiceCommand.Id, new SingleResultFetchStrategy<Invoice>()
            //                                                                            .Include(c => c.InvoiceItems)
            //                                                                            .Include(c => c.OrderRefrences)
            //                                                                            .Include(c => c.AdditionalPrices)
            //                                                                            .Include(c => c.OrderRefrences.Select(d => d.OrderItems)), invoiceConfigurator);

            //    if (invoice == null)
            //        throw new ObjectNotFound("Invoice", invoiceCommand.Id);

            //    invoiceRepository.Detach(invoice);
            //}


            var orderRefrences = GetOrderRefrences(invoiceCommand.OrdersRefrenceId);
            var invoiceRefrence = GetInvoiceRefrence(invoiceCommand.InvoiceRefrenceId);

            var transporter = invoiceCommand.TransporterId == null ? null : companyRepository.Single(c => c.Id == invoiceCommand.TransporterId);
            var supplier = invoiceCommand.SupplierId == null ? null : companyRepository.Single(c => c.Id == invoiceCommand.SupplierId);
            var owner = companyRepository.Single(c => c.Id == invoiceCommand.OwnerId);
            if (owner == null)
                throw new ObjectNotFound("Company", invoiceCommand.OwnerId);

            var currency = currencyDomainService.Get(invoiceCommand.CurrencyId);
            if (currency == null)
                throw new ObjectNotFound("Currency", invoiceCommand.CurrencyId);

            var goods = goodDomainService.GetCompanyGoodsWithUnits(invoiceCommand.OwnerId).ToList();


            //if (invoiceCommand.Id != 0 && invoiceCommand.InvoiceType != invoice.InvoiceType)
            //    throw new InvalidOperation("Update", "Can not Update Invoice Type");


            var invoiceItems = GetInvoiceItems(invoiceCommand, goods).ToList();

            var invoiceAdditionalPriceList = GetAdditionalPrice(invoiceCommand);

            var invoice = invoiceFactory.CreateInvoiceObject
                 (
                  invoiceCommand, goods, owner, transporter, supplier, invoiceRefrence, orderRefrences, currency, invoiceItems,
                  invoiceAdditionalPriceList, true, userId);

            if (invoiceCommand.Id != 0)
            {
                invoice.Id = invoiceCommand.Id;
            }

            return invoice;

            //if (invoice.Id == 0)
            //{
            //    invoice = invoiceFactory.CreateInvoiceObject
            //        (
            //         invoiceCommand, goods, owner, transporter, supplier, invoiceRefrence, orderRefrences, currency, invoiceItems,
            //         invoiceAdditionalPriceList, true, userId);

            //    return invoice;
            //}
            //else
            //{
            //    invoice.Update
            //        (
            //         invoiceCommand.InvoiceNumber, invoiceCommand.InvoiceDate, invoiceCommand.DivisionMethod, invoiceRefrence, orderRefrences,
            //         currency, invoiceCommand.IsCreditor, transporter, supplier, invoiceCommand.Description, invoiceItems,
            //         invoiceAdditionalPriceList, invoiceDomainService, invoiceItemDomainService, goodUnitConvertorDomainService,
            //         invoiceAdditionalPriceDomainService, balanceDomainService);

            //    return invoice;
            //}

        }

        private List<InvoiceAdditionalPrice> GetAdditionalPrice(InvoiceCommand invoiceCommand)
        {
            var effectiveFactors = effectiveFactorDomainService.GetEffectiveFactors();
            var invoiceAdditionalPriceList =
                invoiceCommand.AdditionalPrices.Select
                    (
                        additionalPrice =>
                            new InvoiceAdditionalPrice
                            (
                            effectiveFactors.Single(c => c.Id == additionalPrice.EffectiveFactorId), additionalPrice.Price,
                            additionalPrice.Divisionable, additionalPrice.Description)).ToList();
            return invoiceAdditionalPriceList;
        }

        private static List<InvoiceItem> GetInvoiceItems(InvoiceCommand invoiceCommand, IEnumerable<Good> goods)
        {
            var invoiceItems = new List<InvoiceItem>();
            foreach (var invoiceItem in invoiceCommand.InvoiceItems)
            {
                var good = goods.Single(c => c.Id == invoiceItem.GoodId);
                if (good == null)
                    throw new ObjectNotFound("Good");
                var goodUnit = good.GoodUnits.SingleOrDefault(d => d.Id == invoiceItem.GoodUnitId);
                if (goodUnit == null)
                    throw new ObjectNotFound("GoodUnit");
                invoiceItems.Add
                    (new InvoiceItem(invoiceItem.Quantity, invoiceItem.Fee, good, goodUnit, invoiceItem.DivisionPrice, invoiceItem.Description));
            }
            return invoiceItems;
        }

        private Invoice GetInvoiceRefrence(long? invoiceId)
        {

            if (invoiceId != null)
                return invoiceRepository.Single(c => c.Id == invoiceId, new SingleResultFetchStrategy<Invoice>().Include(d => d.InvoiceItems));
            return null;
        }


        private List<Order> GetOrderRefrences(IList<long> orderRefrencesId)
        {
            List<Order> orderRefrences = null;
            if (orderRefrencesId.Count > 0)
                orderRefrences = orderRepository.Find(c => orderRefrencesId.Contains(c.Id), new ListFetchStrategy<Order>().Include(d => d.OrderItems)).ToList();
            return orderRefrences;
        }


        public void DeleteById(long id)
        {
            Invoice invoice = invoiceRepository.FindByKey(id);
            if (invoice == null)
                throw new ObjectNotFound("Invoice", id);

            invoice.Delete(invoiceItemDomainService, invoiceAdditionalPriceDomainService, invoiceRepository);

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("Invoice");
            }
        }

        private List<VesselInCompany> GetVessels(long? fromVesselInCompanyId, long? toVesselInCompanyId)
        {
            var ids = new List<long>();
            if (fromVesselInCompanyId.HasValue)
                ids.Add(fromVesselInCompanyId.Value);

            if (toVesselInCompanyId.HasValue)
                ids.Add(toVesselInCompanyId.Value);

            var idList = ids.Distinct().ToList();

            var list = vesselDomainService.Get(idList);

            if (idList.Count != list.Count)
                throw new ObjectNotFound("Vessel");

            return list;
        }

        private List<Company> GetCompanies(long ownerId, long? transporter, long? supplier, long? receiver)
        {
            var ids = new List<long> { ownerId };

            if (transporter.HasValue)
                ids.Add(transporter.Value);

            if (supplier.HasValue)
                ids.Add(supplier.Value);

            if (receiver.HasValue)
                ids.Add(receiver.Value);

            var idList = ids.Distinct().ToList();

            var list = companyDomainService.Get(idList);

            if (idList.Count != list.Count)
                throw new ObjectNotFound("Company");

            return list;
        }

        #endregion

        #region InvoiceItem

        //        public void DeleteItem(long invoiceId, long invoiceItemId)
        //        {
        //            var invoice = invoiceRepository.FindByKey(invoiceId);
        //            if (invoice == null)
        //                throw new ObjectNotFound("invoice", invoiceId);
        //
        //            var invoiceItem = invoice.InvoiceItems.SingleOrDefault(c => c.Id == invoiceItemId);
        //            if (invoiceItem == null)
        //                throw new ObjectNotFound("invoiceItem", invoiceItemId);
        //            invoice.DeleteItem(invoiceItem, invoiceItemDomainService);
        //            try
        //            {
        //                unitOfWorkScope.Commit();
        //            }
        //            catch (OptimisticConcurrencyException ex)
        //            {
        //                throw new ConcurencyException("Invoice");
        //            }
        //            //            catch (Exception ex)
        //            //            {
        //            //                throw new UnHandleException(ex);
        //            //            }
        //        }
        //
        //        public InvoiceItem UpdateItem(long id, long invoiceId, decimal fee, decimal quantity, string description)
        //        {
        //            var invoice = invoiceRepository.FindByKey(invoiceId);
        //
        //            if (invoice == null)
        //                throw new ObjectNotFound("invoice", invoiceId);
        //
        //
        //            //            invoice.UpdateItem(id, description, quantity,
        //            //                             goodId,
        //            //                             unitId,
        //            //                             assigneBuessinessPartyForGoodId,
        //            //                             goodDetails
        //            // );
        //
        //            try
        //            {
        //                unitOfWorkScope.Commit();
        //            }
        //            catch (OptimisticConcurrencyException ex)
        //            {
        //                throw new ConcurencyException("Invoice");
        //            }
        //            return invoice.InvoiceItems.Single(c => c.Id == id);
        //        }




        private GoodFullInfo GetGoodFromDomain(long ownerId, long goodId)
        {
            var goodDetails = goodDomainService.GetGoodInfoes(ownerId, goodId);
            if (goodDetails == null)
                throw new ObjectNotFound("Good", goodId);
            return goodDetails;
        }

        #endregion
    }
}