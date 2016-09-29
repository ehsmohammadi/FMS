using System;
using System.Collections.Generic;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Application;

namespace MITD.Fuel.Application.Service.Contracts
{
    public interface IFuelReportApplicationService : IApplicationService
    {
        FuelReport ManageCommand(FuelReportCommandDto data);

        FuelReport GetById(long id);

        FuelReportDetail UpdateFuelReportDetail(long fuelReportId, long fuelReportDetailId, decimal rob, decimal consumption, decimal? receive, ReceiveTypes? receiveTypeId, decimal? transfer, TransferTypes? transferTypeId, decimal? correction, CorrectionTypes? correctionType, CorrectionPricingTypes? correctionPricingType, decimal? correctionPrice, long? currencyId, Reference transferReference, Reference receiveReference, Reference correctionReference,
            long? trustIssueInventoryTransactionItemId);

        FuelReport UpdateVoyageId(long fuelReportId, long voyageId);

        void IsSetFuelReportInventoryResultPossible(long fuelReportId);

        void SetFuelReportInventoryResults(InventoryResultCommand resultBag);

        FuelReport UpdateVoyageEndOfVoyageFuelReport(long fuelReportId, DateTime newDateTime);

        void RefreshFuelReportsVoyage(long companyId, long? vesselInCompanyId);
        void Delete(long id);

        void RevertFuelReportConsumptionInventoryOperations(long fuelReportId);
        void RevertFuelReportDetailCorrectionInventoryOperations(long fuelReportId, long fuelReportDetailId);
        void RevertFuelReportDetailReceiveInventoryOperations(long fuelReportId, long fuelReportDetailId);
        void RevertFuelReportDetailTransferInventoryOperations(long fuelReportId, long fuelReportDetailId);
    }
}
