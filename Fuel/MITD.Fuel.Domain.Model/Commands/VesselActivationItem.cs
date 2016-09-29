namespace MITD.Fuel.Domain.Model.Commands
{
    public partial class VesselActivationItem
    {
        public long Id { get; set; }

        public long CurrencyId { get; set; }
        public string CurrencyCode { get; set; }

        public long GoodId { get; set; }
        public string GoodCode { get; set; }
        public string GoodName { get; set; }

        public long TankId { get; set; }

        public decimal Rob { get; set; }

        public long UnitId { get; set; }
        public string UnitCode { get; set; }

        public decimal Fee { get; set; }

        public string TankCode { get; set; }
    }
}
