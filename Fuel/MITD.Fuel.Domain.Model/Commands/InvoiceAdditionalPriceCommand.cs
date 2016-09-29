namespace MITD.Fuel.Domain.Model.Commands
{
    public class InvoiceAdditionalPriceCommand
    {
        public long Id { get; set; }
        public long EffectiveFactorId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool Divisionable { get; set; }
        public long InvoiceId { get; set; }

        public InvoiceAdditionalPriceCommand()
        {

        }

        public InvoiceAdditionalPriceCommand(long effectiveFactorId, decimal price, string description, bool divisionable)
        {
            EffectiveFactorId = effectiveFactorId;
            Price = price;
            Description = description;
            Divisionable = divisionable;
        }
    }
}