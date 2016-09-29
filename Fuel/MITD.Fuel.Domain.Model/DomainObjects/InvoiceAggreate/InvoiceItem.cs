
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Exceptions;

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate
{
    public class InvoiceItem
    {
        public long Id { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal Fee { get; private set; }

        public long InvoiceId { get; private set; }
        public long GoodId { get; private set; }
        public long MeasuringUnitId { get; private set; }

        public string Description { get; private set; }


        public virtual Good Good { get; private set; }
        public virtual GoodUnit MeasuringUnit { get; private set; }

        public byte[] TimeStamp { get; private set; }

        public virtual Invoice Invoice { get; private set; }

        public decimal Price
        {
            get { return Quantity * Fee + DivisionPrice; }
        }


        public decimal DivisionPrice { get; private set; }

        public virtual List<OrderItemBalance> OrderItemBalances { get; private set; }

        public InvoiceItem()
        {

        }
        public InvoiceItem(decimal quantity, decimal fee, long goodId, long goodUnitId, string description)
        {
            Quantity = quantity;
            Fee = fee;
            GoodId = goodId;
            MeasuringUnitId = goodUnitId;
            Description = description;
        }
        public InvoiceItem(decimal quantity, decimal fee, Good good, GoodUnit goodUnit, decimal divisionPrice, string description)
        {
            Quantity = quantity;
            Fee = fee;
            Good = good;
            GoodId = good.Id;

            MeasuringUnit = goodUnit;
            MeasuringUnitId = goodUnit.Id;
            Description = description;
            DivisionPrice = divisionPrice;

            IsNotEmpty();
            IsHaveValidQuantity();

        }

        public void UpdateQuantity(decimal quantity)
        {
            Quantity += quantity;
        }

        public void SetDivisionPrice(decimal divisionPrice)
        {
            DivisionPrice = divisionPrice;
            //Price = Quantity*Fee + DivisionPrice;
        }

        //BR_IN14
        private void IsNotEmpty()
        {
            if (GoodId == 0 || MeasuringUnitId == 0)
                throw new BusinessRuleException("BR_PO5", "Unit OR Good Is Empty ");
        }

        //BR_R6
        private void IsHaveValidQuantity()
        {
            if (Quantity <= 0)
                throw new BusinessRuleException("BR_PO8", "Quantity Is Negative ");
        }


        public void UpdateUnitAndQuantity(long goodUnitId, decimal value)
        {

            MeasuringUnitId = goodUnitId;
            Quantity = value;

        }
    }
}