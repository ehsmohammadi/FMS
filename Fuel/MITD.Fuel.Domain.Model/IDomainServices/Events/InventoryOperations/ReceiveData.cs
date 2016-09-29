using System;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations
{
    public class ReceiveData
    {
        public long CompanyId;

        public long VesselInCompanyId;

        public long TankId;

        public DateTime ReportDate;

        public long CompanyGoodId;

        public long UnitId;

        public double Receive;

        public ReceiveTypes ReceiveType;

        public ReceiveReferenceType ReferenceType;

        public long ReferenceId;
    }

    public enum ReceiveReferenceType : int
    {
        Order = 1,
    }
}