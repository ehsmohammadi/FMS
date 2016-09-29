#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using MITD.Core;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class OrderItem
    {
        public const decimal ORDER_RECEIVE_TOLERANCE_DECIMAL = 10;

        #region Properties

        public long Id { get; private set; }

        public string Description { get; private set; }

        public decimal Quantity { get; private set; }

        public long MeasuringUnitId { get; private set; }

        public long OrderId { get; private set; }

        public long GoodId { get; private set; }

        public decimal InvoicedInMainUnit { get; private set; }

        public decimal OperatedQuantityInMainUnit { get; private set; }

        #endregion

        #region Navigation Properties

        //        public virtual GoodPartyAssignment GoodPartyAssignment { get; private set; }
        public virtual GoodUnit MeasuringUnit { get; private set; }

        public virtual Good Good { get; private set; }

        public virtual Order Order { get; private set; }

        public byte[] TimeStamp { get; private set; }

        public virtual IList<OrderItemBalance> OrderItemBalances { get; private set; }

        public virtual IList<OrderItemOperatedQuantity> OrderItemOperatedQuantities { get; private set; }
        #endregion

        #region ctor

        public OrderItem()
        {
            OrderItemBalances = new List<OrderItemBalance>();
            OrderItemOperatedQuantities = new List<OrderItemOperatedQuantity>();
        }

        public OrderItem(string description, decimal quantity, long goodId, long unitId, GoodFullInfo goodFullInfo)
            :this()
        {
            CommonBussinessRule(quantity, goodId, unitId, goodFullInfo);
            Description = description;
            Quantity = quantity;
            GoodId = goodId;
            MeasuringUnitId = unitId;
        }


        internal void Update(string description, decimal quantity, long goodId, long unitId, GoodFullInfo goodFullInfo)
        {
            CommonBussinessRule(quantity, goodId, unitId, goodFullInfo);

            ValidateToNotBeReferenced();

            Description = description;
            Quantity = quantity;
            GoodId = goodId;
            MeasuringUnitId = unitId;
        }

        #endregion

        #region BussinesRules

        private void CommonBussinessRule(decimal quantity, long goodId, long unitId, GoodFullInfo goodFullInfo)
        {
            //BR_PO5
            IsNotEmpty(goodId, unitId);
            //BR_PO8
            HasValidQuantity(quantity);
            //BR_PO19
            ValidateGoodUnit(goodFullInfo, unitId);
        }

        //BR_PO5
        private void IsNotEmpty(long goodId, long unitId)
        {
            if (goodId == 0 || unitId == 0)
                throw new BusinessRuleException("BR_PO5", "Unit OR Good Is Empty ");
        }

        //BR_PO8
        private void HasValidQuantity(decimal quantity)
        {
            if (quantity <= 0)
                throw new BusinessRuleException("BR_PO8", "Quantity Is Negative ");
        }

        //BR_PO19
        private void ValidateGoodUnit(GoodFullInfo goodFullInfo, long unitId)
        {
            if (goodFullInfo.CompanyGoodUnits == null || goodFullInfo.CompanyGoodUnits.All(c => c.Id != unitId))
                throw new BusinessRuleException("", " Unit is not defined For Compony.");
        }

        public bool IsReferenced()
        {
            return this.OperatedQuantityInMainUnit != 0 || this.InvoicedInMainUnit != 0 || this.OrderItemBalances.Count > 0;
        }

        public void ValidateToNotBeReferenced()
        {
            if (this.IsReferenced())
            {
                var fuelReportsDetailIds = this.OrderItemBalances.Where(oib => oib.FuelReportDetailId.HasValue).Select(oib => oib.FuelReportDetailId.Value.ToString());
                var fuelReportsDetailIdsText = fuelReportsDetailIds.Count() > 0 ? string.Format("\n\nFuel Report Detail Ids:\n{0}", string.Join(" , ", fuelReportsDetailIds)) : string.Empty;

                var invoiceIds = this.OrderItemBalances.Where(oib => oib.InvoiceItem != null).Select(oib => oib.InvoiceItem.InvoiceId.ToString());
                invoiceIds = invoiceIds.Concat(this.OrderItemBalances.Where(oib => oib.PairingInvoiceItemId.HasValue).Select(oib => oib.PairingInvoiceItem.InvoiceId.ToString()));

                var invoiceIdsText = invoiceIds.Count() > 0 ? string.Format("\n\nInvoice Ids:\n{0}", string.Join(" , ", invoiceIds)) : string.Empty;

                throw new BusinessRuleException(string.Format("The Order Item for good '{0}' is referenced.{1}{2}", this.Good.Code, fuelReportsDetailIdsText, invoiceIdsText));
            }
        }

        #endregion

        public void UpdateInvoiced(decimal invoicedQuantityInMainUnit)
        {
            if (InvoicedInMainUnit + invoicedQuantityInMainUnit > this.OperatedQuantityInMainUnit)
                throw new BusinessRuleException("", "Invoiced quantity coud not be greater than Received quantity.");

            InvoicedInMainUnit += invoicedQuantityInMainUnit;
        }

        public void AddFuelReportReceive(decimal receivedQuantityInMainUnit, long fuelReportDetailId, IGoodUnitConvertorDomainService goodUnitConvertorDomainService)
        {
            //this.RemoveOperatedQuantity(fuelReportDetailId);

            var orderItemQuantityInMainUnit = goodUnitConvertorDomainService.ConvertUnitValueToMainUnitValue(this.MeasuringUnitId, this.Quantity + ORDER_RECEIVE_TOLERANCE_DECIMAL);

            if(this.Order.OrderType != OrderTypes.Purchase 
                && this.Order.OrderType != OrderTypes.PurchaseForVessel 
                && this.Order.OrderType != OrderTypes.PurchaseWithTransferOperations
                && this.Order.OrderType != OrderTypes.InternalTransfer)
                throw new BusinessRuleException("", "Invalid Order Type to set received FuelReport quantity.");

            if (receivedQuantityInMainUnit <= 0)
                throw new BusinessRuleException("", "Received quantity could not be less than or equal to zero.");

            if (this.OperatedQuantityInMainUnit + receivedQuantityInMainUnit > orderItemQuantityInMainUnit)
                throw new BusinessRuleException("", "Received quantity could not become greater then available ordered quantity.");

            if (this.OperatedQuantityInMainUnit + receivedQuantityInMainUnit < InvoicedInMainUnit)
                throw new BusinessRuleException("", "Received quantity could not become less then current invoiced quantity.");

            this.OperatedQuantityInMainUnit += receivedQuantityInMainUnit;

            this.OrderItemOperatedQuantities.Add(new OrderItemOperatedQuantity(this.Id, fuelReportDetailId, receivedQuantityInMainUnit, this.MeasuringUnit.MainGoodUnit.Abbreviation));
        }

        public void AddSupplyForDeliveredVesselReceive(decimal receivedQuantityInMainUnit, IGoodUnitConvertorDomainService goodUnitConvertorDomainService)
        {
            var orderItemQuantityInMainUnit = goodUnitConvertorDomainService.ConvertUnitValueToMainUnitValue(this.MeasuringUnitId, this.Quantity + ORDER_RECEIVE_TOLERANCE_DECIMAL);

            if (this.Order.OrderType != OrderTypes.SupplyForDeliveredVessel)
                throw new BusinessRuleException("", "Invalid Order Type to set supplied FuelReport quantity.");

            if (receivedQuantityInMainUnit <= 0)
                throw new BusinessRuleException("", "Received quantity could not be less than or equal to zero.");

            if (this.OperatedQuantityInMainUnit + receivedQuantityInMainUnit > orderItemQuantityInMainUnit)
                throw new BusinessRuleException("", "Received quantity could not become greater then available ordered quantity.");

            if (this.OperatedQuantityInMainUnit + receivedQuantityInMainUnit < InvoicedInMainUnit)
                throw new BusinessRuleException("", "Received quantity could not become less then current invoiced quantity.");

            this.OperatedQuantityInMainUnit += receivedQuantityInMainUnit;
        }

        public void RemoveOperatedQuantity(long fuelReportDetailId)
        {
            var orderItemOperatedQuantityToRemove = this.OrderItemOperatedQuantities.SingleOrDefault(q => q.FuelReportDetailId == fuelReportDetailId);

            if (orderItemOperatedQuantityToRemove == null) return;

            var operatedQuantityInMainUnit = orderItemOperatedQuantityToRemove.QuantityAmountInMainUnit;

            if (operatedQuantityInMainUnit <= 0)
                throw new BusinessRuleException("", "Received quantity could not be less than or equal to zero.");

            if (this.OperatedQuantityInMainUnit - operatedQuantityInMainUnit < 0)
                throw new BusinessRuleException("", "Received quantity could not be less than current received quantity");

            if (this.OperatedQuantityInMainUnit - operatedQuantityInMainUnit < InvoicedInMainUnit)
                throw new BusinessRuleException("", "Received quantity could not become less than current invoiced quantity.");

            this.OperatedQuantityInMainUnit -= operatedQuantityInMainUnit;

            var orderItemOperatedQuantityRepository = ServiceLocator.Current.GetInstance<IRepository<OrderItemOperatedQuantity>>();

            orderItemOperatedQuantityRepository.Delete(orderItemOperatedQuantityToRemove);
        }

        public void AddFuelReportTransfer(decimal transferedQuantityInMainUnit, long fuelReportDetailId, IGoodUnitConvertorDomainService goodUnitConvertorDomainService)
        {
            var orderItemQuantityInMainUnit = goodUnitConvertorDomainService.ConvertUnitValueToMainUnitValue(this.MeasuringUnitId, this.Quantity + ORDER_RECEIVE_TOLERANCE_DECIMAL);

            if (this.Order.OrderType != OrderTypes.Transfer && this.Order.OrderType != OrderTypes.InternalTransfer)
                throw new BusinessRuleException("", "Invalid Order Type to set transfered FuelReport quantity.");

            if (transferedQuantityInMainUnit <= 0)
                throw new BusinessRuleException("", "Transfered quantity could not be less than or equal to zero.");

            if (this.OperatedQuantityInMainUnit + transferedQuantityInMainUnit > orderItemQuantityInMainUnit)
                throw new BusinessRuleException("", "Transfered quantity could not become greater then available ordered quantity.");

            if (this.OperatedQuantityInMainUnit + transferedQuantityInMainUnit < InvoicedInMainUnit)
                throw new BusinessRuleException("", "Transfered quantity could not become less then current invoiced quantity.");

            this.OperatedQuantityInMainUnit += transferedQuantityInMainUnit;

            this.OrderItemOperatedQuantities.Add(new OrderItemOperatedQuantity(this.Id, fuelReportDetailId, transferedQuantityInMainUnit, this.MeasuringUnit.MainGoodUnit.Abbreviation));
        }


        public decimal GetAvailableForInvoiceInMainUnit()
        {
            return this.OperatedQuantityInMainUnit - InvoicedInMainUnit;
        }

        public bool IsValidToReceive(decimal receiveQuantity, long goodUnitId, IGoodUnitConvertorDomainService goodUnitConvertorDomainService)
        {
            var orderItemQuantityInMainUnit = goodUnitConvertorDomainService.ConvertUnitValueToMainUnitValue(this.MeasuringUnitId, this.Quantity + ORDER_RECEIVE_TOLERANCE_DECIMAL);

            var receiveQuantityInMainUit = goodUnitConvertorDomainService.ConvertUnitValueToMainUnitValue(goodUnitId, receiveQuantity);

            return this.OperatedQuantityInMainUnit + receiveQuantityInMainUit <= orderItemQuantityInMainUnit;
        }

        public bool IsValidToTransfer(decimal transferQuantity, long goodUnitId, IGoodUnitConvertorDomainService goodUnitConvertorDomainService)
        {
            var orderItemQuantityInMainUnit = goodUnitConvertorDomainService.ConvertUnitValueToMainUnitValue(this.MeasuringUnitId, this.Quantity + ORDER_RECEIVE_TOLERANCE_DECIMAL);

            var transferQuantityInMainUit = goodUnitConvertorDomainService.ConvertUnitValueToMainUnitValue(goodUnitId, transferQuantity);

            return (transferQuantityInMainUit <= this.OperatedQuantityInMainUnit) ||
                   (transferQuantityInMainUit <= orderItemQuantityInMainUnit);
                //this.OperatedQuantityInMainUnit + receiveQuantityInMainUit <= orderItemQuantityInMainUnit;
        }

        internal void CheckDeleteRules()
        {
            ValidateToNotBeReferenced();
        }


    }
}