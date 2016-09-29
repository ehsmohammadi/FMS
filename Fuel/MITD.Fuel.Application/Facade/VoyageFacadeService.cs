
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Castle.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;
using MITD.Fuel.Presentation.Contracts.FacadeServices.Fuel;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class VoyageFacadeService : IVoyageFacadeService
    {
        private readonly IVoyageApplicationService voyageApplicationService;
        private readonly IVoyageDomainService voyageDomainService;
        private readonly IVoyageToVoyageDtoMapper voyageDtoMapper;
        private readonly IFuelReportFuelReportDtoMapper fuelReportDtoMapper;
        private readonly IInventoryOperationDomainService inventoryOperationDomainService;
        private readonly IInventoryOperationToInventoryOperationDtoMapper inventoryOperationDtoMapper;
        private readonly IVoyageLogToVoyageLogDtoMapper voyageLogDtoMapper;
        private readonly IGoodToGoodDtoMapper goodDtoMapper;
        private readonly IVoyageLogDomainService voyageLogDomainService;
        private readonly IInventoryManagementDomainService inventoryManagementDomainService;

        public VoyageFacadeService(
            IVoyageDomainService voyageDomainService,
            IVoyageToVoyageDtoMapper voyageDtoMapper,
            IFuelReportDomainService fuelReportDomainService,
            IFuelReportFuelReportDtoMapper fuelReportDtoMapper,
            IInventoryOperationDomainService inventoryOperationDomainService,
            IInventoryOperationToInventoryOperationDtoMapper inventoryOperationDtoMapper,
            IVoyageLogDomainService voyageLogDomainService,
            IVoyageLogToVoyageLogDtoMapper voyageLogDtoMapper,
            IGoodToGoodDtoMapper goodDtoMapper, IInventoryManagementDomainService inventoryManagementDomainService, IVoyageApplicationService voyageApplicationService)
        {
            this.voyageDomainService = voyageDomainService;
            this.voyageDtoMapper = voyageDtoMapper;
            //this.fuelReportDomainService = fuelReportDomainService;
            this.fuelReportDtoMapper = fuelReportDtoMapper;
            this.inventoryOperationDomainService = inventoryOperationDomainService;
            this.inventoryOperationDtoMapper = inventoryOperationDtoMapper;/**/
            this.voyageLogDtoMapper = voyageLogDtoMapper;
            this.goodDtoMapper = goodDtoMapper;
            this.inventoryManagementDomainService = inventoryManagementDomainService;
            this.voyageApplicationService = voyageApplicationService;
            this.voyageLogDomainService = voyageLogDomainService;
        }

        //================================================================================

        private void populateData(VoyageDto voyageDto, bool includeFuelReports, bool includeInventoryOperations)
        {
            if (includeFuelReports)
                populateVoyageFuelReports(voyageDto);

            if (includeInventoryOperations)
                populateVoyageInventoryOperations(voyageDto);
        }

        //================================================================================

        private void populateVoyageFuelReports(VoyageDto voyageDto)
        {
            var endOfVoyageFuelReport = voyageDomainService.GetVoyageValidEndOfVoyageFuelReport(voyageDto.Id);

            if (endOfVoyageFuelReport != null)
            {
                voyageDto.EndOfVoyageFuelReport = this.fuelReportDtoMapper.MapToModel(endOfVoyageFuelReport);
            }
        }

        //================================================================================

        private void populateVoyageInventoryOperations(VoyageDto voyageDto)
        {
            var endOfVoyageFuelReport = voyageDomainService.GetVoyageValidEndOfVoyageFuelReport(voyageDto.Id);

            if (endOfVoyageFuelReport != null)
            {
            //    var inventoryOperationsDtos = endOfVoyageFuelReport.FuelReportDetails.SelectMany(frd =>
            //    {
            //        var fuelReportDetailInventoryOperations = inventoryOperationDomainService.GetFuelReportDetailInventoryOperations(frd.FuelReportId, frd.Id);
            //        var invDtos = this.inventoryOperationDtoMapper.MapToModel(fuelReportDetailInventoryOperations).ToList();
            //        invDtos.ForEach(invDto => invDto.Good = goodDtoMapper.MapToModel(frd.Good));
            //        return invDtos;
            //    });

            //    voyageDto.EndOfVoyageInventoryOperations = new ObservableCollection<FuelReportInventoryOperationDto>(inventoryOperationsDtos);

            //    //var inventoryOperations = endOfVoyageFuelReport.FuelReportDetails.SelectMany(frd => fuelReportFacadeService.GetInventoryOperations(frd.FuelReportId, frd.Id));
            //    //voyageDto.EndOfVoyageInventoryOperations = new ObservableCollection<FuelReportInventoryOperationDto>(inventoryOperations);

                var operationDtos = endOfVoyageFuelReport.ConsumptionInventoryOperations.SelectMany(invo =>{
                    var inventoryResult = inventoryManagementDomainService.GetInventoryResult(endOfVoyageFuelReport.VesselInCompany.CompanyId, invo.InventoryOperationId, invo.ActionType, false);

                    var inventoryOperationDtos =
                        inventoryResult.InventoryResultItems.Select(invr =>
                                                                        new FuelReportInventoryOperationDto()
                                                                        {
                                                                            Id = invo.InventoryOperationId,
                                                                            ActionDate = invo.ActionDate,
                                                                            ActionType = inventoryResult.ActionType.ToString(),
                                                                            Code = inventoryResult.Number,
                                                                            Good = goodDtoMapper.MapToModel(invr.Good)
                                                                        });
                    return inventoryOperationDtos;
                });

                voyageDto.EndOfVoyageInventoryOperations = new ObservableCollection<FuelReportInventoryOperationDto>(operationDtos);
            }
        }

        //================================================================================

        public List<VoyageDto> GetAll(bool includeFuelReports = false, bool includeInventoryOperations = false)
        {
            var voyages = voyageDomainService.GetAll();

            var result = this.voyageDtoMapper.MapToModel(voyages).ToList();

            result.ForEach(vdto => populateData(vdto, includeFuelReports, includeInventoryOperations));

            return result;
        }

        //================================================================================

        public List<VoyageDto> GetByFilter(long companyId, long vesselInCompanyId, bool includeFuelReports = false, bool includeInventoryOperations = false)
        {
            var entities = this.voyageDomainService.GetByFilter(companyId, vesselInCompanyId);
            var result = this.voyageDtoMapper.MapToModel(entities).ToList();

            result.ForEach(vdto => populateData(vdto, includeFuelReports, includeInventoryOperations));

            return result;
        }

        //================================================================================

        public VoyageDto GetById(long id, bool includeFuelReports = false, bool includeInventoryOperations = false)
        {
            var voyage = voyageDomainService.Get(id);
            var result = this.voyageDtoMapper.MapToModel(voyage);

            populateData(result, includeFuelReports, includeInventoryOperations);

            return result;
        }

        //================================================================================

        public PageResultDto<VoyageDto> GetPagedData(int pageSize, int pageIndex, bool includeFuelReports = false, bool includeInventoryOperations = false)
        {
            var pageResult = voyageDomainService.GetPagedData(pageSize, pageIndex);

            var result = this.voyageDtoMapper.MapToModel(pageResult.Result).ToList();

            result.ForEach(vdto => populateData(vdto, includeFuelReports, includeInventoryOperations));

            return new PageResultDto<VoyageDto>()
                {
                    CurrentPage = pageResult.CurrentPage,
                    Result = result,
                    PageSize = pageResult.PageSize,
                    TotalCount = pageResult.TotalCount,
                    TotalPages = pageResult.TotalPages
                };
        }

        //================================================================================

        public PageResultDto<VoyageDto> GetPagedDataByFilter(long companyId, long vesselInCompanyId, int pageSize, int pageIndex, bool includeFuelReports = false, bool includeInventoryOperations = false)
        {
            var pageResult = voyageDomainService.GetPagedDataByFilter(companyId, vesselInCompanyId, pageSize, pageIndex);

            var result = this.voyageDtoMapper.MapToModel(pageResult.Result).ToList();

            result.ForEach(vdto => populateData(vdto, includeFuelReports, includeInventoryOperations));

            return new PageResultDto<VoyageDto>
                {
                    CurrentPage = pageResult.CurrentPage,
                    Result = result,
                    PageSize = pageResult.PageSize,
                    TotalCount = pageResult.TotalCount,
                    TotalPages = pageResult.TotalPages
                };
        }

        //================================================================================

        public PageResultDto<VoyageLogDto> GetChenageHistory(long voyageId, int pageSize, int pageIndex)
        {
            var pageResult = voyageLogDomainService.GetPagedDataByFilter(voyageId, pageSize, pageIndex);

            var result = this.voyageLogDtoMapper.MapToModel(pageResult.Result).ToList();

            return new PageResultDto<VoyageLogDto>
                {
                    CurrentPage = pageResult.CurrentPage,
                    Result = result,
                    PageSize = pageResult.PageSize,
                    TotalCount = pageResult.TotalCount,
                    TotalPages = pageResult.TotalPages
                };
        }

        public List<VoyageDto> FindVoyages(long companyId, long vesselInCompanyId, DateTime lookingDateTime)
        {
            var entity = this.voyageDomainService.FindVoyages(companyId, vesselInCompanyId, lookingDateTime);

            var result = this.voyageDtoMapper.MapToModel(entity);
            
            return result.ToList();
        }

        public void UpdateVoyagesFromRotationData()
        {
            voyageApplicationService.UpdateVoyagesFromRotationData();
        }

        //================================================================================
    }
}
