using System;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations
{
    public class NegativeCorrectionData
    {
        public long CompanyId;

        public long VesselInCompanyId;

        public long TankId;

        public DateTime ReportDate;

        public long CompanyGoodId;

        public long UnitId;

        public double Correction;

        public bool IsReferenceSpecified;
    }
}