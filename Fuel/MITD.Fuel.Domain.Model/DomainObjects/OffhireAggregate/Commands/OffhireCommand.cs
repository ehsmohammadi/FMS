using System;
using System.Collections.Generic;
namespace MITD.Fuel.Domain.Model.DomainObjects.Commands
{
    public class OffhireCommand
    {
        public long Id { get; set; }
        public long ReferenceNumber { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public long IntroducerId { get; set; }
        public long VesselInCompanyId { get; set; }
        public long? VoyageId { get; set; }
        public long OffhireLocationId { get; set; }
        public DateTime VoucherDate { get; set; }
        public long VoucherCurrencyId { get; set; }

        public List<OffhireCommandDetail> OffhireDetails { get; set; }
    }

    public class OffhireCommandDetail
    {
        public long Id { get; set; }
        public decimal Quantity { get; set; }
        public virtual long GoodId { get; set; }
        public virtual long UnitId { get; set; }
        public virtual long? TankId { get; set; }
        public decimal? FeeInMainCurrency { get; set; }
        public decimal? FeeInVoucherCurrency { get; set; }
    }
}
