using System.Runtime.CompilerServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate
{
    public class OrderItemOperatedQuantity
    {
        public OrderItemOperatedQuantity()
        {
        }

        public long Id { get; private set; }
        public long OrderItemId { get; private set; }
        public virtual OrderItem OrderItem { get; private set; }
        public decimal QuantityAmountInMainUnit { get; private set; }

        public string UnitCode { get; private set; }

        public long FuelReportDetailId { get; private set; }
        public virtual FuelReportDetail FuelReportDetail { get; private set; }

        public OrderItemOperatedQuantity(OrderItem orderItem, FuelReportDetail fuelReportDetail, decimal amount, string unitCode)
            :this(orderItem.Id, fuelReportDetail.Id, amount, unitCode)
        {
            this.OrderItem = orderItem;
            this.FuelReportDetail = fuelReportDetail;
        }

        public OrderItemOperatedQuantity(long orderItemId, long fuelReportDetailId, decimal amount, string unitCode)
        {
           this.OrderItemId = orderItemId;
           this.FuelReportDetailId = fuelReportDetailId;

            this.UnitCode = unitCode;
            this.QuantityAmountInMainUnit = amount;
        }

    }
}