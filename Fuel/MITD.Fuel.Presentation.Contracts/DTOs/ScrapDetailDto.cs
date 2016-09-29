using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class ScrapDetailDto
    {
        public ScrapDetailDto()
        {
        }

        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }


        private double rob;
        public double ROB
        {
            get { return rob; }
            set { this.SetField(p => p.ROB, ref rob, value); }
        }

        private double price;
        public double Price
        {
            get { return price; }
            set { this.SetField(p => p.Price, ref price, value); }
        }

        private CurrencyDto currency;
        public CurrencyDto Currency
        {
            get { return currency; }
            set { this.SetField(p => p.Currency, ref currency, value); }
        }

        private GoodDto good;
        public GoodDto Good
        {
            get { return good; }
            set { this.SetField(p => p.Good, ref good, value); }
        }

        private GoodUnitDto unit;
        public GoodUnitDto Unit
        {
            get { return unit; }
            set { this.SetField(p => p.Unit, ref unit, value); }
        }

        private TankDto tank;
        public TankDto Tank
        {
            get { return tank; }
            set { this.SetField(p => p.Tank, ref tank, value); }
        }

        private ScrapDto scrap;
        public ScrapDto Scrap
        {
            get { return scrap; }
            set { this.SetField(p => p.Scrap, ref scrap, value); }
        }
    }
}
