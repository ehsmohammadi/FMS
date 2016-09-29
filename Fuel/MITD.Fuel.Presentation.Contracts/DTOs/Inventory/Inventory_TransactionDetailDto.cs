using System;
using System.Collections.Generic;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class Inventory_TransactionDetailDto
    {
        public Inventory_TransactionDetailDto()
        {
            //Good=new Inventory_GoodDto();
            //Transaction=new Inventory_TransactionDto();
            //Inventory_TransactionDetailPrice=new List<Inventory_TransactionDetailPriceDto>();
            //QuantityUnit=new Inventory_UnitDto();
            //UserCreator=new Inventory_UserDto();
        }

        int id;
        public int Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        short rowVersion;
        public short RowVersion
        {
            get { return rowVersion; }
            set { this.SetField(p => p.RowVersion, ref rowVersion, value); }
        }

        int transactionId;
        public int TransactionId
        {
            get { return transactionId; }
            set { this.SetField(p => p.TransactionId, ref transactionId, value); }
        }

        long goodId;
        public long GoodId
        {
            get { return goodId; }
            set { this.SetField(p => p.GoodId, ref goodId, value); }
        }

        long quantityUnitId;
        public long QuantityUnitId
        {
            get { return quantityUnitId; }
            set { this.SetField(p => p.QuantityUnitId, ref quantityUnitId, value); }
        }

        decimal? quantityAmount;
        public decimal? QuantityAmount
        {
            get { return quantityAmount; }
            set { this.SetField(p => p.QuantityAmount, ref quantityAmount, value); }
        }

        string description;
        public string Description
        {
            get { return description; }
            set { this.SetField(p => p.Description, ref description, value); }
        }
        int? userCreatorId;
        public int? UserCreatorId
        {
            get { return userCreatorId; }
            set { this.SetField(p => p.UserCreatorId, ref userCreatorId, value); }
        }

        private DateTime? createDate;
        public DateTime? CreateDate
        {
            get { return this.createDate; }
            set { this.SetField(p => p.CreateDate, ref this.createDate, value); }
        }

        private Inventory_GoodDto good;
        public Inventory_GoodDto Good
        {
            get { return good; }
            set { this.SetField(p => p.Good, ref good, value); }
        }

        private Inventory_TransactionDto transaction;
        public Inventory_TransactionDto Transaction
        {
            get { return transaction; }
            set { this.SetField(p => p.Transaction, ref transaction, value); }
        }

        private List<Inventory_TransactionDetailPriceDto> inventory_TransactionDetailPrice;
        public List<Inventory_TransactionDetailPriceDto> Inventory_TransactionDetailPrice
        {
            get { return inventory_TransactionDetailPrice; }
            set { this.SetField(p => p.Inventory_TransactionDetailPrice, ref inventory_TransactionDetailPrice, value); }
        }

        private Inventory_UnitDto quantityUnit;
        public Inventory_UnitDto QuantityUnit
        {
            get { return quantityUnit; }
            set { this.SetField(p => p.QuantityUnit, ref quantityUnit, value); }
        }

        private Inventory_UserDto userCreator;
        public Inventory_UserDto UserCreator
        {
            get { return userCreator; }
            set { this.SetField(p => p.UserCreator, ref userCreator, value); }
        }

        public decimal? TotalMainCurrencyPrice
        {
            get { return this.totalMainCurrencyPrice; }
            set { this.SetField(p => p.TotalMainCurrencyPrice, ref this.totalMainCurrencyPrice, value); }
        }

        private decimal? totalMainCurrencyPrice;

    }
}
