using System;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations
{
    public class PositiveCorrectionData
    {
        public long CompanyId;

        public long VesselInCompanyId;

        public long TankId;

        public DateTime ReportDate;

        public long CompanyGoodId;

        public long UnitId;

        public double Correction;

        public CorrectionTypes CorrectionType;

        public CorrectionReferenceType ReferenceType;

        public long ReferenceId;
    }

    public enum CorrectionReferenceType : int
    {
        NotSpecified = 0,
        Voyage = 1,
        FuelReportDetailNumber = 2
    }
}