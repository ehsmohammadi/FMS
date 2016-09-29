
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{

    public partial class Inventory_TransactionDto
    {
        public Inventory_TransactionDto()
        {
            //Warehouse = new Inventory_WarehouseDto();
            //PricingReference = new Inventory_TransactionDto();
            //StoreTypes = new Inventory_StoreTypeDto();
            //Inventory_TransactionDetail = new List<Inventory_TransactionDetailDto>();
            //UserCreator = new Inventory_UserDto();
        }

        int id;
        public int Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        byte action;
        public byte Action
        {
            get { return action; }
            set { this.SetField(p => p.Action, ref action, value); }
        }

        decimal code;
        public decimal Code
        {
            get { return code; }
            set { this.SetField(p => p.Code, ref code, value); }
        }

        string description;
        public string Description
        {
            get { return description; }
            set { this.SetField(p => p.Description, ref description, value); }
        }

        int? pricingReferenceId;
        public int? PricingReferenceId
        {
            get { return pricingReferenceId; }
            set { this.SetField(p => p.PricingReferenceId, ref pricingReferenceId, value); }
        }

        long warehouseId;
        public long WarehouseId
        {
            get { return warehouseId; }
            set { this.SetField(p => p.WarehouseId, ref warehouseId, value); }
        }

        int storeTypesId;
        public int StoreTypesId
        {
            get { return storeTypesId; }
            set { this.SetField(p => p.StoreTypesId, ref storeTypesId, value); }
        }

        int timeBucketId;
        public int TimeBucketId
        {
            get { return timeBucketId; }
            set { this.SetField(p => p.TimeBucketId, ref timeBucketId, value); }
        }

        byte? status;
        public byte? Status
        {
            get { return status; }
            set { this.SetField(p => p.Status, ref status, value); }
        }

        private DateTime? registrationDate;
        public DateTime? RegistrationDate
        {
            get { return this.registrationDate; }
            set { this.SetField(p => p.RegistrationDate, ref this.registrationDate, value); }
        }

        int? senderReciver;
        public int? SenderReciver
        {
            get { return senderReciver; }
            set { this.SetField(p => p.SenderReciver, ref senderReciver, value); }
        }

        string hardCopyNo;
        public string HardCopyNo
        {
            get { return hardCopyNo; }
            set { this.SetField(p => p.HardCopyNo, ref hardCopyNo, value); }
        }

        string referenceType;
        public string ReferenceType
        {
            get { return referenceType; }
            set { this.SetField(p => p.ReferenceType, ref referenceType, value); }
        }

        string referenceNo;
        public string ReferenceNo
        {
            get { return referenceNo; }
            set { this.SetField(p => p.ReferenceNo, ref referenceNo, value); }
        }

        private DateTime? referenceDate;
        public DateTime? ReferenceDate
        {
            get { return this.referenceDate; }
            set { this.SetField(p => p.ReferenceDate, ref this.referenceDate, value); }
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

        private Inventory_WarehouseDto warehouse;
        public Inventory_WarehouseDto Warehouse
        {
            get { return warehouse; }
            set { this.SetField(p => p.Warehouse, ref warehouse, value); }
        }

        private Inventory_TransactionDto pricingReference;
        public Inventory_TransactionDto PricingReference
        {
            get { return pricingReference; }
            set { this.SetField(p => p.PricingReference, ref pricingReference, value); }
        }

        private Inventory_StoreTypeDto storeTypes;
        public Inventory_StoreTypeDto StoreTypes
        {
            get { return storeTypes; }
            set { this.SetField(p => p.StoreTypes, ref storeTypes, value); }
        }

        private List<Inventory_TransactionDetailDto> inventory_TransactionDetail;
        public List<Inventory_TransactionDetailDto> Inventory_TransactionDetail
        {
            get { return inventory_TransactionDetail; }
            set { this.SetField(p => p.Inventory_TransactionDetail, ref inventory_TransactionDetail, value); }
        }

        private Inventory_UserDto userCreator;
        public Inventory_UserDto UserCreator
        {
            get { return userCreator; }
            set { this.SetField(p => p.UserCreator, ref userCreator, value); }
        }
    }
}
