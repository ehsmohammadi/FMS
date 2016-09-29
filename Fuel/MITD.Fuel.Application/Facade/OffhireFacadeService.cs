using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Application.Service.Security;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.Commands;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class OffhireFacadeService : IOffhireFacadeService
    {
        private readonly IOffhireApplicationService offhireApplicationService;
        private readonly IOffhireDomainService offhireDomainService;
        private readonly IOffhireToOffhireDtoMapper offhireDtoMapper;
        private readonly IOffhireDetailToOffhireDetailDtoMapper offhireDetailDtoMapper;
        private readonly IInventoryOperationToInventoryOperationDtoMapper inventoryOperationDtoMapper;
        private readonly IOffhireManagementSystemDomainService offhireManagementSystemDomainService;
        private readonly IOffhireManagementSystemEntityToOffhireManagementSystemDtoMapper offhireManagementSystemDtoMapper;
        private readonly IOffhirePreparedDataToOffhireDtoMapper offhirePreparedDataToOffhireDtoMapper;

        private readonly IPricingValueToPricingValueDtoMapper pricingValueDtoMapper;
        private readonly IFuelUserRepository fuelUserRepository;


        public OffhireFacadeService(
            IOffhireApplicationService offhireApplicationService,
            IOffhireDomainService offhireDomainService,
            IEntityConfigurator<Offhire> offhireConfigurator,
            IOffhireToOffhireDtoMapper offhireDtoMapper,
            IOffhireDetailToOffhireDetailDtoMapper offhireDetailDtoMapper,
            IInventoryOperationToInventoryOperationDtoMapper inventoryOperationDtoMapper,
            IOffhireManagementSystemDomainService offhireManagementSystemDomainService,
            IOffhireManagementSystemEntityToOffhireManagementSystemDtoMapper offhireManagementSystemDtoMapper,
            IOffhirePreparedDataToOffhireDtoMapper offhirePreparedDataToOffhireDtoMapper,
            IPricingValueToPricingValueDtoMapper pricingValueDtoMapper, IFuelUserRepository fuelUserRepository)
        {
            this.offhireApplicationService = offhireApplicationService;
            this.offhireDomainService = offhireDomainService;
            this.offhireDtoMapper = offhireDtoMapper;
            this.offhireDetailDtoMapper = offhireDetailDtoMapper;
            this.inventoryOperationDtoMapper = inventoryOperationDtoMapper;
            this.offhireManagementSystemDomainService = offhireManagementSystemDomainService;
            this.offhireManagementSystemDtoMapper = offhireManagementSystemDtoMapper;
            this.offhirePreparedDataToOffhireDtoMapper = offhirePreparedDataToOffhireDtoMapper;
            this.pricingValueDtoMapper = pricingValueDtoMapper;
            this.fuelUserRepository = fuelUserRepository;

            this.offhireDomainService.SetConfigurator(offhireConfigurator);
        }

        //================================================================================

        public OffhireDto GetById(long id)
        {
            var offhire = offhireDomainService.Get(id);

            var result = offhireDtoMapper.MapToModel(offhire, this.setEditProperties);

            setModelPricingValues(result);

            return result;
        }

        //================================================================================

        private void setEditProperties(Offhire offhire, OffhireDto offhireDto)
        {
            offhireDto.IsOffhireEditPermitted = offhireDomainService.IsOffhireEditPermitted(offhire);
            offhireDto.IsOffhireDeletePermitted = offhireDomainService.IsOffhireDeletePermitted(offhire);
        }

        //================================================================================

        public PageResultDto<OffhireDto> GetPagedData(int pageSize, int pageIndex)
        {
            var pageResult = offhireDomainService.GetPagedData(pageSize, pageIndex);

            return mapPageResult(pageResult);
        }

        //================================================================================

        public PageResultDto<OffhireDto> GetPagedDataByFilter(long? companyId, long? vesselInCompanyId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex)
        {
            var pageResult = offhireDomainService.GetPagedDataByFilter(companyId, vesselInCompanyId, fromDate, toDate, pageSize, pageIndex);

            return mapPageResult(pageResult);
        }

        //================================================================================

        private PageResultDto<OffhireDto> mapPageResult(PageResult<Offhire> pageResult)
        {
            return new PageResultDto<OffhireDto>()
            {
                CurrentPage = pageResult.CurrentPage,
                PageSize = pageResult.PageSize,
                TotalCount = pageResult.TotalCount,
                TotalPages = pageResult.TotalPages,
                Result = offhireDtoMapper.MapToModel(pageResult.Result, this.setEditProperties).ToList()
            };
        }

        //================================================================================

        private PageResultDto<OffhireDetailDto> mapPageResult(PageResult<OffhireDetail> pageResult)
        {
            return new PageResultDto<OffhireDetailDto>()
            {
                CurrentPage = pageResult.CurrentPage,
                PageSize = pageResult.PageSize,
                TotalCount = pageResult.TotalCount,
                TotalPages = pageResult.TotalPages,
                Result = offhireDetailDtoMapper.MapToModel(pageResult.Result).ToList()
            };
        }

        //================================================================================

        public OffhireDto Add(OffhireDto dto)
        {
            var command = convertDtoToCommand(dto);

            var updatedEntity = offhireApplicationService.AddOffhire(command, getFuelUserId());

            var result = offhireDtoMapper.MapToModel(updatedEntity, this.setEditProperties);

            setModelPricingValues(result);

            return result;
        }

        //================================================================================

        public OffhireDto Update(OffhireDto dto)
        {
            var command = convertDtoToCommand(dto);

            var updatedEntity = offhireApplicationService.UpdateOffhire(command);

            var result = offhireDtoMapper.MapToModel(updatedEntity, this.setEditProperties);

            setModelPricingValues(result);

            return result;
        }

        private OffhireCommand convertDtoToCommand(OffhireDto dto)
        {
            var command = new OffhireCommand()
            {
                Id = dto.Id,
                ReferenceNumber = dto.ReferenceNumber,
                OffhireLocationId = dto.OffhireLocation.Id,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                IntroducerId = dto.Introducer.Id,
                VesselInCompanyId = dto.VesselInCompany.Id,
                VoyageId = dto.Voyage != null ? (long?)dto.Voyage.Id : null,
                VoucherCurrencyId = dto.VoucherCurrency == null ? -1 : dto.VoucherCurrency.Id,
                VoucherDate = dto.VoucherDate == null ? DateTime.MinValue : dto.VoucherDate.Value,
                OffhireDetails = dto.OffhireDetails.Select(od => new OffhireCommandDetail()
                {
                    Id = od.Id,
                    GoodId = od.Good.Id,
                    Quantity = od.Quantity,
                    UnitId = od.Unit.Id,
                    TankId = od.Tank != null ? (long?)od.Tank.Id : null,
                    FeeInMainCurrency = od.FeeInMainCurrency,
                    FeeInVoucherCurrency = od.FeeInVoucherCurrency
                }).ToList()
            };

            return command;
        }

        //================================================================================

        public void Delete(long id)
        {
            offhireApplicationService.DeleteOffhire(id);
        }

        //================================================================================

        public OffhireDetailDto GetOffhireDetail(long offhireId, long offhireDetailId)
        {
            var detail = offhireDomainService.GetOffhireDetail(offhireId, offhireDetailId);

            var detailDto = offhireDetailDtoMapper.MapToModel(detail);

            detailDto.Offhire = GetById(offhireId);

            return detailDto;
        }

        //================================================================================

        //public OffhireDetailDto AddOffhireDetail(OffhireDetailDto detailDto)
        //{
        //    var result = offhireApplicationService.AddOffhireDetail(
        //        detailDto.Offhire.Id, detailDto.ROB, detailDto.Price, detailDto.Currency.Id, detailDto.Good.Id, detailDto.Unit.Id, detailDto.Tank.Id);

        //    return offhireDetailDtoMapper.MapToModel(result);
        //}

        //================================================================================

        //public OffhireDetailDto UpdateOffhireDetail(OffhireDetailDto detailDto)
        //{
        //    var result = offhireApplicationService.UpdateOffhireDetail(
        //        detailDto.Offhire.Id, detailDto.Id, detailDto.ROB, detailDto.Price, detailDto.Currency.Id, detailDto.Good.Id, detailDto.Unit.Id, detailDto.Tank.Id);

        //    return offhireDetailDtoMapper.MapToModel(result);
        //}

        //================================================================================

        //public void DeleteOffhireDetail(long offhireId, long offhireDetailId)
        //{
        //    offhireApplicationService.DeleteOffhireDetail(offhireId, offhireDetailId);
        //}

        //================================================================================

        public PageResultDto<OffhireManagementSystemDto> GetOffhireManagementSystemPagedDataByFilter(long companyId, long vesselInCompanyId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex)
        {
            var result = offhireManagementSystemDomainService.GetFinalizedOffhires(companyId, vesselInCompanyId, fromDate, toDate);

            pageIndex = Math.Min(pageIndex, result.Count / pageSize);

            var pageResult = new PageResult<OffhireManagementSystemEntity>()
                                    {
                                        CurrentPage = pageIndex,
                                        PageSize = pageSize,
                                        TotalCount = result.Count,
                                        TotalPages = pageSize == 0 ? 0 : (int)Math.Ceiling((double)result.Count / pageSize),
                                        Result = pageSize == 0 ? result : result.Skip(pageSize * pageIndex).Take(pageSize).ToList()
                                    };

            return mapPageResult(pageResult);
        }

        //================================================================================

        public OffhireDto PrepareOffhireData(long referenceNumber, long introducerId)
        {
            var preparedData = offhireApplicationService.GetPreparedData(referenceNumber, introducerId);

            var result = offhirePreparedDataToOffhireDtoMapper.MapToModel(preparedData);

            setModelPricingValues(result);

            return result;
        }

        //================================================================================

        private void setModelPricingValues(OffhireDto result)
        {
            result.PricingValuesInMainCurrency = GetOffhirePricingValuesInMainCurrency(result.Introducer.Id, result.VesselInCompany.Id, result.StartDateTime);

            result.OffhireDetails.ToList().ForEach(
                                    od =>
                                    {
                                        var relevantGoodPricing = result.PricingValuesInMainCurrency.FirstOrDefault(pv => pv.Good.Id == od.Good.Id);

                                        if (relevantGoodPricing != null)
                                        {
                                            var feeInMainCurrency = relevantGoodPricing.Fee;

                                            if (feeInMainCurrency.HasValue)
                                                od.FeeInMainCurrency = feeInMainCurrency;

                                            od.IsFeeInVoucherCurrencyReadOnly = feeInMainCurrency.HasValue;
                                        }
                                    });
        }

        //================================================================================

        public PricingValueDto GetOffhirePricingValueInVoucherCurrency(long introducerId, long vesselInCompanyId, DateTime startDateTime, long goodId, long voucherCurrencyId, DateTime voucherDate)
        {
            var pricingValue = this.offhireApplicationService.GetOffhirePricingValueInVoucherCurrency(introducerId, vesselInCompanyId, startDateTime, goodId, voucherCurrencyId, voucherDate);

            return pricingValueDtoMapper.MapToModel(pricingValue);
        }

        //================================================================================

        public PricingValueDto GetOffhirePricingValueInMainCurrency(long introducerId, long vesselInCompanyId, DateTime startDateTime, long goodId)
        {
            var pricingValue = this.offhireApplicationService.GetOffhirePricingValueInMainCurrency(introducerId, vesselInCompanyId, startDateTime, goodId);

            return pricingValueDtoMapper.MapToModel(pricingValue);
        }

        //================================================================================

        public List<PricingValueDto> GetOffhirePricingValuesInVoucherCurrency(long introducerId, long vesselInCompanyId, DateTime startDateTime, long voucherCurrencyId, DateTime voucherDate)
        {
            var pricingValue = this.offhireApplicationService.GetOffhirePricingValuesInVoucherCurrency(introducerId, vesselInCompanyId, startDateTime, voucherCurrencyId, voucherDate);

            return pricingValueDtoMapper.MapToModel(pricingValue).ToList();
        }

        //================================================================================

        public List<PricingValueDto> GetOffhirePricingValuesInMainCurrency(long introducerId, long vesselInCompanyId, DateTime startDateTime)
        {
            var pricingValue = this.offhireApplicationService.GetOffhirePricingValuesInMainCurrency(introducerId, vesselInCompanyId, startDateTime);

            return pricingValueDtoMapper.MapToModel(pricingValue).ToList();
        }

        //================================================================================

        private PageResultDto<OffhireManagementSystemDto> mapPageResult(PageResult<OffhireManagementSystemEntity> pageResult)
        {
            return new PageResultDto<OffhireManagementSystemDto>()
                {
                    CurrentPage = pageResult.CurrentPage,
                    PageSize = pageResult.PageSize,
                    TotalCount = pageResult.TotalCount,
                    TotalPages = pageResult.TotalPages,
                    Result = offhireManagementSystemDtoMapper.MapToModel(pageResult.Result).ToList()
                };
        }

        //================================================================================

        public PageResultDto<OffhireDetailDto> GetPagedOffhireDetailData(long offhireId, int pageSize, int pageIndex)
        {
            var pageResult = offhireDomainService.GetPagedOffhireDetailData(offhireId, pageSize, pageIndex);

            return mapPageResult(pageResult);
        }

        //================================================================================

        private long getFuelUserId()
        {
            var currentUserId = SecurityApplicationService.GetCurrentUserId();
            var currentCompanyId = SecurityApplicationService.GetCurrentUserCompanyId();

            return fuelUserRepository.Single(fu => fu.IdentityId == currentUserId && fu.CompanyId == currentCompanyId).Id;
        }

        
        //================================================================================
    }
}