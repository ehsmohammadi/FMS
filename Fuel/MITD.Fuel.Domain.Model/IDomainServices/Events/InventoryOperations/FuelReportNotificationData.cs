using System;
using System.Collections.Generic;

namespace MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations
{
    public class FuelReportNotificationData
    {
        public long FuelReportId;

        public List<FuelReportNotificationDataItem> Items;
    }

    public class FuelReportNotificationDataItem
    {
        public long FuelReportDetailId;

        public PositiveCorrectionData PositiveCorrection;

        public NegativeCorrectionData NegativeCorrection;

        public TransferData Transfer;

        public ReceiveData Receive;

        public EndOfMonthData EndOfMonth;

        public EndOfYearData EndOfYear;

        public EndOfVoyageData EndOfVoyage;
    }
}