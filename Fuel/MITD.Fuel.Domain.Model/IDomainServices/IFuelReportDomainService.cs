using System;
using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.ReportObjects;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IFuelReportDomainService : IDomainService
    {
        FuelReport Get(long id);

        List<FuelReport> GetAll();

        PageResult<FuelReport> GetByFilter(long? companyId, long? vesselInCompanyId, string vesselReportCode, DateTime? fromDate, DateTime? toDate, string fuelReportIds, string fuelReportDetailIds, int pageSize, int pageIndex);

        FuelReportDetail GetFuelReportDetailById(long id, long detailId);

        List<FuelReport> GetPreviousNotFinalApprovedReports(FuelReport fuelReport);

        List<FuelReport> GetFuelReportsFromYesterday(FuelReport fuelReport);

        FuelReport GetPreviousFuelReport(long currentFuelReportId);

        FuelReportDetail GetPreviousFuelReportDetail(long currentFuelReportId, long currentFuelReportDetailId);
        
        FuelReportDetail GetPreviousFuelReportDetailByGood(long currentFuelReportId, long goodId);

        List<FuelReport> GetVoyagesValidEndOfVoyageFuelReports(List<Voyage> voyages);

        FuelReport GetVoyageValidEndOfVoyageFuelReport(long voyageId);
        FuelReport GetVoyageValidEndOfVoyageFuelReport(Voyage voyage);

        List<FuelReport> GetCompanyVesselFuelReports(long vesselInCompanyId, DateTime? startDateTime, DateTime? endDateTime, bool ignoreTime = false);


        //List<Reference> GetFuelReportDetailTransferReferences(FuelReportDetail fuelReportDetail);
        List<Reference> GetFuelReportDetailInternalTransferTransferReferences(FuelReportDetail fuelReportDetail);
        List<Reference> GetFuelReportDetailSaleTransferTransferReferences(FuelReportDetail fuelReportDetail);
       
        //List<Reference> GetFuelReportDetailReceiveReferences(FuelReportDetail fuelReportDetail);
       
        List<Reference> GetFuelReportDetailInternalTransferReceiveReferences(FuelReportDetail fuelReportDetail, bool isTrustIssueQuantityAssignmentPossible, decimal? trustQuantity);
        List<Reference> GetFuelReportDetailTransferPurchaseReceiveReferences(FuelReportDetail fuelReportDetail, bool isTrustIssueQuantityAssignmentPossible, decimal? trustQuantity);
        List<Reference> GetFuelReportDetailPurchaseReceiveReferences(FuelReportDetail fuelReportDetail, bool isTrustIssueQuantityAssignmentPossible, decimal? trustQuantity);

        List<Reference> GetFuelReportDetailCorrectionReferences(FuelReportDetail fuelReportDetail);

        FuelReport GetLastIssuedEOVFuelReportOfPreviousVoyages(FuelReport fuelReport);
        FuelReport GetLastIssuedEOVFuelReportOfPreviousVoyages(long vesselInCompanyId, DateTime dateTimeToCompare);

        List<FuelReport> GetNotIssuedEOVFuelReportsOfPreviousVoyages(FuelReport fuelReport);

        void SetFuelReportInventoryResults(InventoryResultCommand resultCommand, FuelReport fuelReport);

        //Commented due to be unusable
        //void InvalidateAllPreviousEndOfVoyageFuelReports(FuelReport fuelReport);

        List<FuelReport> GetVoyageAllEndOfVoyageFuelReport(Voyage voyage);

        InventoryOperation GetVoyageConsumptionIssueOperation(long voyageId);

        ChangingFuelReportDateData GetChangingFuelReportDateData(long fuelReportId, DateTime newDateTime);
        List<Reference> GetFuelReportDetailRejectedTransferReferences(FuelReportDetail fuelReportDetail);

        decimal CalculateReportingConsumption(FuelReportDetail fuelReportDetail);

        FuelReportDetail GetLastPurchasingReceiveFuelReportDetailBefore(FuelReportDetail fuelReportDetail);

        CharterPreparedData PrepareCharterData(long fuelReportId);

        Charter GetVesselActivationCharterContract(FuelReport processingFuelReport);

        List<FuelReport> GetVesselFuelReports(string vesselCode, DateTime? startDateTime, DateTime? endDateTime, bool ignoreTime = false);

        void MoveFuelReportsToCompany(string vesselCode, long destinationCompanyId, DateTime fromDateTime, DateTime? toDateTime, long userId);

        void ChangeFuelReportsStateForCharteredOutVessel(VesselInCompany vesselInCompany, DateTime fromDateTime, DateTime? toDateTime, long userId);

        void ManageFuelReportCharterContract(FuelReport fuelReport, long approverId);

        #region VesselEventReport

        VesselEventReportsView GetVesselEventReportsView(string eventReportCode);

        IList<VesselEventReportsView> GetVesselEventReportsViewData(DateTime fromDate, DateTime toDate, string vesselCode);

        #endregion

        List<GoodTrustReceiveData> GetGoodsTotalTrustReceiveData(CharterOut comparingCharterOutStart);
        List<GoodTrustReceiveData> GetGoodsTotalTrustReceiveData(CharterIn comparingCharterInEnd);

        bool IsFuelReportTheFirstOneInVesselActivePeriod(FuelReport fuelReport);
        bool IsFuelReportTheFirstOneInVesselActivePeriod(FuelReport fuelReport, Charter vesselActivationCharterContract);
        void SetNextFuelReportsFirstPositionStatus(FuelReport fuelReport);
        bool IsThereAnySubmittedFuelReportAfter(FuelReport fuelReport);
        bool IsThereAnySubmittedFuelReportAtTheTimeOfCurrentReport(FuelReport fuelReport);
        List<long> FindFuelReportDetailsWithReceiveByOrder(long orderId);

        void Delete(FuelReport fuelReport);
        List<long> GetFuelReportsWithCorrectionButNotRevisedByFinance(long? companyId, string warehouseCode, DateTime toDateTime);
    }

    public class InventoryResultCommand
    {
        public long FuelReportId;
        public List<InventoryResultCommandItem> Items;
    }

    public class InventoryResultCommandItem
    {
        public long FuelReportDetailId;
        public long OperationId;
        public string ActionNumber;
        public InventoryActionType ActionType;
        public DateTime ActionDate;
    }

    public class ChangingFuelReportDateData
    {
        public FuelReport ChangingFuelReport;

        public FuelReport NextFuelReportBeforeChangeDate;

        public FuelReport NextFuelReportAfterChangeDate;
    }
}
