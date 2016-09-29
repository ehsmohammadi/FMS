
using System;
using System.Collections.Generic;
using MITD.Presentation;
using MITD.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.DTOs.Report;


namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IFuelReportServiceWrapper : IServiceWrapper
    {
        void GetAll(Action<PageResultDto<FuelReportDto>, Exception> action, string methodName, int pageSize, int pageIndex);

        void GetById(Action<FuelReportDto, Exception> action, long id, bool includeReferencesLookup = false);

        void GetFuelReportDetailById(Action<FuelReportDetailDto, Exception> action, long id, long detailId, bool includeReferencesLookup = false);

        void Add(Action<FuelReportDto, Exception> action, FuelReportDto ent);

        void Update(Action<FuelReportDto, Exception> action, FuelReportDto ent);

        void Delete(Action<string, Exception> action, long id);
        void UpdateFuelReportDetail(Action<FuelReportDetailDto, Exception> action, FuelReportDetailDto ent);

        void GetAllCurrency(Action<List<CurrencyDto>, Exception> action);


        void GetByFilter(Action<PageResultDto<FuelReportDto>, Exception> action, long? companyId, long? vesselInCompanyId, string vesselReportCode, DateTime? fromDate, DateTime? toDate, string fuelReportIds, string fuelReportDetailIds, int pageSize, int pageIndex);

        void RefreshFuelReportsVoyage(Action<object, Exception> action, long companyId, long? vesselInCompanyId);

        void GetVesselEventData(Action<VesselEventReportViewDto, Exception> action, string eventReportCode);

        void RevertFuelReportInventoryOperations(Action<object, Exception> action, long fuelReportId);
    }
}