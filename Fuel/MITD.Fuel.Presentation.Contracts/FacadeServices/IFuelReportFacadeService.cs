using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;
using System.Collections.Generic;
using System;
using MITD.Fuel.Presentation.Contracts.DTOs.Report;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
    public interface IFuelReportFacadeService : IFacadeService
    {
        ResultFuelReportDto Add(FuelReportCommandDto data);

        FuelReportDto GetById(long id, bool includeReferencesLookup);

        //PageResultDto<FuelReportDto> GetAll(int pageSize, int pageIndex, bool includeReferencesLookup);

        PageResultDto<FuelReportDto> GetByFilter(long? companyId, long? vesselInCompanyId, string vesselReportCode, DateTime? fromDate, DateTime? toDate, string fuelReportId, string fuelReportDetailId, int pageSize, int pageIndex, bool includeReferencesLookup);

        FuelReportDto Update(FuelReportDto fuelReportDto);

        FuelReportDetailDto UpdateFuelReportDetail(long fuelReportId, FuelReportDetailDto fuelReportDetailDto);
        FuelReportDetailDto UpdateFuelReportDetailByFinance(long fuelReportId, FuelReportDetailDto fuelReportDetailDto);

        //List<CurrencyDto> GetAllCurrency();

        List<FuelReportInventoryOperationDto> GetInventoryOperations(long id);
        List<FuelReportInventoryOperationDto> GetDetailInventoryOperations(long id, long detailId);

        CharterDto PrepareCharterData(long fuelReportId);

        FuelReportDetailDto GetFuelReportDetailById(long id, long detailId, bool includeReferencesLookup);

        void RefreshFuelReportsVoyage(long companyId, long? vesselInCompanyId);

        VesselEventReportViewDto GetVesselEventData(string eventReportCode);
        IList<VesselEventReportViewDto> GetVesselEventsData(DateTime fromDate, DateTime toDate, string vesselCode);
        void Delete(long id);

        void RevertFuelReportConsumptionInventoryOperations(long fuelReportId);
        void RevertFuelReportDetailCorrectionInventoryOperations(long fuelReportId, long fuelReportDetailId);
        void RevertFuelReportDetailReceiveInventoryOperations(long fuelReportId, long fuelReportDetailId);
        void RevertFuelReportDetailTransferInventoryOperations(long fuelReportId, long fuelReportDetailId);

    }
}
