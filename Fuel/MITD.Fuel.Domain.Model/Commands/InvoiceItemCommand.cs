using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Domain.Model.Commands
{
    public class InvoiceItemCommand
    {
        public long Id { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal Fee { get; private set; }

        public long InvoiceId { get; private set; }
        public long GoodId { get; private set; }
        public long GoodUnitId { get; private set; }

        public string Description { get; private set; }


        public Good Good { get; private set; }
        public GoodUnit GoodUnit { get; private set; }

        public decimal Price { get; private set; }


        public decimal DivisionPrice { get; private set; }

        public InvoiceItemCommand()
        {
            
        }
        public InvoiceItemCommand(decimal quantity, decimal fee, long goodId, long goodUnitId, decimal divisionPrice, string description)
        {
            Quantity = quantity;
            Fee = fee;
            GoodId = goodId;
            GoodUnitId = goodUnitId;
            DivisionPrice = divisionPrice;
            Description = description;
        }

        public InvoiceItemCommand(decimal quantity, decimal fee, Good good, GoodUnit goodUnit, string description)
        {
            Quantity = quantity;
            Fee = fee;
            Good = good;
            GoodId = good.Id;
            GoodUnit = goodUnit;
            GoodUnitId = goodUnit.Id;
            Description = description;

        }

    }
}