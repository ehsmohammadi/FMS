namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate
{
    public class InvoiceAdditionalPrice
    {
        public long Id { get; private set; }
        public long EffectiveFactorId { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }

        public bool Divisionable { get; private set; }
        public long InvoiceId { get; private set; }


        public byte[] TimeStamp { get; private set; }
        public virtual Invoice Invoice { get; private set; }
        public virtual EffectiveFactor EffectiveFactor { get; private set; }

        public InvoiceAdditionalPrice()
        {

        }
        public InvoiceAdditionalPrice(EffectiveFactor effectiveFactor, decimal price, bool divisionable, string description)
        {
            EffectiveFactor = effectiveFactor;
            EffectiveFactorId = effectiveFactor.Id;
            Price = price;
            Divisionable = divisionable;
            Description = description;
        }


        //        public InvoiceAdditionalPrice(decimal quantity, decimal fee, long goodId, long goodUnitId, string description)
        //        {
        //            Quantity = quantity;
        //            Fee = fee;
        //            GoodId = goodId;
        //            GoodUnitId = goodUnitId;
        //            Description = description;
        //        }
        //
        //        public InvoiceAdditionalPrice(decimal quantity, decimal fee, Good good, GoodUnit goodUnit, string description)
        //        {
        //            Quantity = quantity;
        //            Fee = fee;
        //            Good = good;
        //            GoodId = good.Id;
        //
        //            GoodUnit = goodUnit;
        //            GoodUnitId = goodUnit.Id;
        //            Description = description;
        //
        //            IsNotEmpty();
        //            IsHaveValidQuantity();
        //
        //        }

        //        public void UpdateQuantity(decimal quantity)
        //        {
        //            Quantity += quantity;
        //        }
        //
        //
        //        //BR_IN14
        //        private void IsNotEmpty()
        //        {
        //            if (GoodId == 0 || GoodUnitId == 0)
        //                throw new BusinessRuleException("BR_PO5", "Unit OR Good Is Empty ");
        //        }
        //
        //        //BR_R6
        //        private void IsHaveValidQuantity()
        //        {
        //            if (Quantity <= 0)
        //                throw new BusinessRuleException("BR_PO8", "Quantity Is Negative ");
        //        }



    }
}