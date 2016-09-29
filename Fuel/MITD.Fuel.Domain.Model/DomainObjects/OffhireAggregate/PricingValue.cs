namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class PricingValue
    {
        public Good Good { get; set; }
        public Currency Currency { get; set; }
        public decimal? Fee { get; set; }
    }
}