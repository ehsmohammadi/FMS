using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.Infrastructure;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{

    public partial class OrderItemDto
    {
        #region private
        private long id;
        private string description;
        private decimal quantity;
        private long orderdId;
        private GoodDto good;
        private decimal invoicedInMainUnit;
        private decimal operatedQuantityInMainUnit;

        #endregion

        #region Public


        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }
        public string Description
        {
            get { return description; }
            set { this.SetField(p => p.Description, ref description, value); }
        }
        [CustomValidation(typeof(ValidationDto), "IsGreaterZero")]
        public decimal Quantity
        {
            get { return quantity; }
            set { this.SetField(p => p.Quantity, ref quantity, value); }
        }
        public long OrderId
        {
            get { return orderdId; }
            set { this.SetField(p => p.OrderId, ref orderdId, value); }
        }

        public virtual GoodDto Good
        {
            get { return good; }
            set
            {
                this.SetField(p => p.Good, ref good, value);
            }
        }

        public long? AssigneBuessinessPartyForGoodId { get; set; }

        public string AssigneBuessinessPartyForGoodName { get; set; }

        public decimal InvoicedInMainUnit
        {
            get { return this.invoicedInMainUnit; }
            set { this.SetField(p => p.InvoicedInMainUnit, ref this.invoicedInMainUnit, value); }
        }

        public decimal OperatedQuantityInMainUnit
        {
            get { return this.operatedQuantityInMainUnit; }
            set { this.SetField(p => p.OperatedQuantityInMainUnit, ref this.operatedQuantityInMainUnit, value); }
        }

        #endregion
    }
}
