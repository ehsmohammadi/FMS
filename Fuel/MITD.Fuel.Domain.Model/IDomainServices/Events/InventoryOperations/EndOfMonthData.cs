using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations
{
    public class EndOfMonthData
    {
        public long CompanyId;

        public long VesselInCompanyId;

        public long TankId;

        public DateTime ReportDate;

        public long CompanyGoodId;

        public long UnitId;

        public double ConsumptionQuantity;
    }
}
