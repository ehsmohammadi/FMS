using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Application.Service.Security;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class ScrapFacadeService : IScrapFacadeService
    {
        private readonly IScrapApplicationService scrapApplicationService;
        private readonly IScrapDomainService scrapDomainService;
        private readonly IScrapToScrapDtoMapper scrapDtoMapper;
        private readonly IScrapDetailToScrapDetailDtoMapper scrapDetailDtoMapper;
        private readonly IInventoryOperationDomainService inventoryOperationDomainService;
        private readonly IInventoryOperationToInventoryOperationDtoMapper inventoryOperationDtoMapper;
        private readonly IFuelUserDomainService fuelUserDomainService;

        public ScrapFacadeService(
            IScrapApplicationService scrapApplicationService,
            IScrapDomainService scrapDomainService,
            IScrapToScrapDtoMapper scrapDtoMapper,
            IScrapDetailToScrapDetailDtoMapper scrapDetailDtoMapper, 
            IInventoryOperationDomainService inventoryOperationDomainService, 
            IInventoryOperationToInventoryOperationDtoMapper inventoryOperationDtoMapper, 
            IFuelUserDomainService fuelUserDomainService)
        {
            this.scrapApplicationService = scrapApplicationService;
            this.scrapDomainService = scrapDomainService;
            this.scrapDtoMapper = scrapDtoMapper;
            this.scrapDetailDtoMapper = scrapDetailDtoMapper;
            this.inventoryOperationDomainService = inventoryOperationDomainService;
            this.inventoryOperationDtoMapper = inventoryOperationDtoMapper;
            this.fuelUserDomainService = fuelUserDomainService;
        }

        //================================================================================

        public ScrapDto GetById(long id)
        {
            var scrap = scrapDomainService.Get(id);

            var scrapDto = scrapDtoMapper.MapToModel(scrap, this.setEditProperties);

            return scrapDto;
        }

        //================================================================================

        private void setEditProperties(Scrap scrap, ScrapDto scrapDto)
        {
            scrapDto.IsScrapEditPermitted = scrapDomainService.IsScrapEditPermitted(scrap);
            scrapDto.IsScrapDeletePermitted = scrapDomainService.IsScrapDeletePermitted(scrap);

            scrapDto.IsScrapAddDetailPermitted = scrapDomainService.IsScrapDetailAddPermitted(scrap);
            scrapDto.IsScrapEditDetailPermitted = scrapDomainService.IsScrapDetailEditPermitted(scrap);
            scrapDto.IsScrapDeleteDetailPermitted = scrapDomainService.IsScrapDetailDeletePermitted(scrap);


            scrapDto.IsGoodEditable = scrapDomainService.IsGoodEditable(scrap);
            scrapDto.IsTankEditable = scrapDomainService.IsTankEditable(scrap);
        }

        //================================================================================

        public PageResultDto<ScrapDto> GetPagedData(int pageSize, int pageIndex)
        {
            var pageResult = scrapDomainService.GetPagedData(pageSize, pageIndex);

            return mapPageResult(pageResult);
        }

        //================================================================================

        public PageResultDto<ScrapDto> GetPagedDataByFilter(long? companyId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex)
        {
            var pageResult = scrapDomainService.GetPagedDataByFilter(companyId, fromDate, toDate, pageSize, pageIndex);

            return mapPageResult(pageResult);
        }

        //================================================================================

        private PageResultDto<ScrapDto> mapPageResult(PageResult<Scrap> pageResult)
        {
            return new PageResultDto<ScrapDto>()
            {
                CurrentPage = pageResult.CurrentPage,
                PageSize = pageResult.PageSize,
                TotalCount = pageResult.TotalCount,
                TotalPages = pageResult.TotalPages,
                Result = scrapDtoMapper.MapToModel(pageResult.Result, this.setEditProperties).ToList()
            };
        }

        //================================================================================

        private PageResultDto<ScrapDetailDto> mapPageResult(PageResult<ScrapDetail> pageResult)
        {
            return new PageResultDto<ScrapDetailDto>()
            {
                CurrentPage = pageResult.CurrentPage,
                PageSize = pageResult.PageSize,
                TotalCount = pageResult.TotalCount,
                TotalPages = pageResult.TotalPages,
                Result = scrapDetailDtoMapper.MapToModel(pageResult.Result).ToList()
            };
        }

        //================================================================================

        public ScrapDto ScrapVessel(ScrapDto dto)
        {
            var result = scrapApplicationService.ScrapVessel(dto.VesselInCompany.Id, dto.SecondParty.Id, dto.ScrapDate,
                fuelUserDomainService.GetCurrentFuelUserId());

            return scrapDtoMapper.MapToModel(result, this.setEditProperties);
        }

        //================================================================================

        public ScrapDto Update(ScrapDto dto)
        {
            var result = scrapApplicationService.UpdateScrap(dto.Id, dto.VesselInCompany.Id, dto.SecondParty.Id, dto.ScrapDate);

            return scrapDtoMapper.MapToModel(result, this.setEditProperties);
        }

        //================================================================================

        public void Delete(long id)
        {
            scrapApplicationService.DeleteScrap(id);
        }

        //================================================================================

        public ScrapDetailDto GetScrapDetail(long scrapId, long scrapDetailId)
        {
            var detail = scrapDomainService.GetScrapDetail(scrapId, scrapDetailId);

            var detailDto = scrapDetailDtoMapper.MapToModel(detail);

            detailDto.Scrap = GetById(scrapId);

            return detailDto;
        }

        //================================================================================

        public ScrapDetailDto AddScrapDetail(ScrapDetailDto detailDto)
        {
            var result = scrapApplicationService.AddScrapDetail(
                detailDto.Scrap.Id, detailDto.ROB, detailDto.Price, detailDto.Currency.Id, detailDto.Good.Id, detailDto.Unit.Id, detailDto.Tank.Id);

            return scrapDetailDtoMapper.MapToModel(result);
        }

        //================================================================================

        public ScrapDetailDto UpdateScrapDetail(ScrapDetailDto detailDto)
        {
            var result = scrapApplicationService.UpdateScrapDetail(
                detailDto.Scrap.Id, detailDto.Id, detailDto.ROB, detailDto.Price, detailDto.Currency.Id, detailDto.Good.Id, detailDto.Unit.Id, detailDto.Tank.Id);

            return scrapDetailDtoMapper.MapToModel(result);
        }

        //================================================================================

        public void DeleteScrapDetail(long scrapId, long scrapDetailId)
        {
            scrapApplicationService.DeleteScrapDetail(scrapId, scrapDetailId);
        }

        //================================================================================

        public PageResultDto<FuelReportInventoryOperationDto> GetInventoryOperations(long id, int? pageSize, int? pageIndex)
        {
            var pageResult = inventoryOperationDomainService.GetScrapInventoryOperations(id, pageSize, pageIndex);

            //var detailDto = GetScrapDetail(id, detailId);

            var scrapDto = GetById(id);

            return mapPageResult(pageResult, scrapDto);
        }

        //================================================================================

        public PageResultDto<ScrapDetailDto> GetPagedScrapDetailData(long scrapId, int pageSize, int pageIndex)
        {
            var pageResult = scrapDomainService.GetPagedScrapDetailData(scrapId, pageSize, pageIndex);

            return mapPageResult(pageResult);
        }

        //================================================================================

        //private PageResultDto<FuelReportInventoryOperationDto> mapPageResult(PageResult<InventoryOperation> pageResult, GoodDto good)
        //{
        //    var result = new PageResultDto<FuelReportInventoryOperationDto>()
        //    {
        //        CurrentPage = pageResult.CurrentPage,
        //        PageSize = pageResult.PageSize,
        //        TotalCount = pageResult.TotalCount,
        //        TotalPages = pageResult.TotalPages,
        //        Result = inventoryOperationDtoMapper.MapToModel(pageResult.Result).ToList()
        //    };

        //    result.Result.ToList().ForEach(invDto => invDto.Good = good);

        //    return result;
        //}

        //================================================================================

        private PageResultDto<FuelReportInventoryOperationDto> mapPageResult(PageResult<InventoryOperation> pageResult, ScrapDto scrap)
        {
            var invOperationDtos = new List<FuelReportInventoryOperationDto>();

            foreach (var invOpr in pageResult.Result)
            {
                foreach (var detailDto in scrap.ScrapDetails)
                {
                    var invOprDto = inventoryOperationDtoMapper.MapToModel(invOpr);

                    invOprDto.Good = detailDto.Good;

                    invOperationDtos.Add(invOprDto);
                }
            }

            var result = new PageResultDto<FuelReportInventoryOperationDto>()
            {
                CurrentPage = pageResult.CurrentPage,
                PageSize = pageResult.PageSize,
                TotalCount = pageResult.TotalCount * scrap.ScrapDetails.Count,
                Result = invOperationDtos
            };

            return result;
        }

        //================================================================================

    }
}