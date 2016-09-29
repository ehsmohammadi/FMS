namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class PricingValueDto
    {
        public GoodDto Good { get; set; }
        public CurrencyDto Currency { get; set; }
        public decimal? Fee { get; set; }
    }
}