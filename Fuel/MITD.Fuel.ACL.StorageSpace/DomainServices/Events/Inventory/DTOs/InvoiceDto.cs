using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;
using AccountingTypesEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.AccountingTypesEnum;
using DivisionMethodEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.DivisionMethodEnum;
using InvoiceTypeEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.InvoiceTypeEnum;
using WorkflowStageEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.WorkflowStageEnum;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public class InvoiceDto : DTOBase
    {
        #region props

        private AccountingTypesEnum accountType;
        private ObservableCollection<InvoiceAdditionalPriceDto> additionalPrices;
        private WorkflowStageEnum approveStatus;
        private long companyId;
        private long currencyId;
        private string description;
        private DivisionMethodEnum divisionMethod;
        private long id;
        private DateTime invoiceDate;
        private ObservableCollection<InvoiceItemDto> invoiceItems;
        private string invoiceNumber;
        private InvoiceDto invoiceRefrence;
        private InvoiceTypeEnum invoiceType;
        private bool isCreditor;
        private ObservableCollection<OrderDto> orderRefrences;
        private long ownerId;
        private long? supplierId;
        private string supplierName;
        private decimal totalOfadditionalPrice;
        private long? transporterId;
        private string transporterName;


        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((ReferenceEquals(this.Id, value) != true)) { this.id = value; } }
        }

        [DataMember]
        public bool IsCreditor
        {
            get { return this.isCreditor; }
            set { if ((ReferenceEquals(this.IsCreditor, value) != true)) { this.isCreditor = value; } }
        }


        [DataMember]
        public long CompanyId
        {
            get { return this.companyId; }
            set { if ((ReferenceEquals(this.CompanyId, value) != true)) { this.companyId = value; } }
        }

        [DataMember]
        public long CurrencyId
        {
            get { return this.currencyId; }
            set { if ((ReferenceEquals(this.CurrencyId, value) != true)) { this.currencyId = value; } }
        }

        [DataMember]
        public DateTime InvoiceDate
        {
            get { return this.invoiceDate; }
            set { if ((ReferenceEquals(this.InvoiceDate, value) != true)) { this.invoiceDate = value; } }
        }


        [DataMember]
        public WorkflowStageEnum ApproveStatus
        {
            get { return this.approveStatus; }
            set { if ((ReferenceEquals(this.ApproveStatus, value) != true)) { this.approveStatus = value; } }
        }

        [DataMember]
        public string ApproveStatusString
        {
            //get { return approveStatus.GetDescription(); }
            get { return this.approveStatus.ToString(); }
        }

        [DataMember]
        public string Description
        {
            get { return this.description; }
            set { if ((ReferenceEquals(this.Description, value) != true)) { this.description = value; } }
        }


        [DataMember]
        public string InvoiceNumber
        {
            get { return this.invoiceNumber; }
            set { if ((ReferenceEquals(this.InvoiceNumber, value) != true)) { this.invoiceNumber = value; } }
        }


        [DataMember]
        public long? TransporterId
        {
            get { return this.transporterId; }
            set { if ((ReferenceEquals(this.TransporterId, value) != true)) { this.transporterId = value; } }
        }

        [DataMember]
        public string TransporterName
        {
            get { return this.transporterName; }
            set { if ((ReferenceEquals(this.TransporterName, value) != true)) { this.transporterName = value; } }
        }

        [DataMember]
        public long? SupplierId
        {
            get { return this.supplierId; }
            set { if ((ReferenceEquals(this.SupplierId, value) != true)) { this.supplierId = value; } }
        }

        [DataMember]
        public string SupplierName
        {
            get { return this.supplierName; }
            set { if ((ReferenceEquals(this.SupplierName, value) != true)) { this.supplierName = value; } }
        }


        [DataMember]
        public long AccountTypeId
        {
            get { return (long) this.accountType; }
            set { if ((ReferenceEquals(this.AccountType, value) != true)) { this.accountType = (AccountingTypesEnum) value; } }
        }


        [DataMember]
        public AccountingTypesEnum AccountType
        {
            get { return this.accountType; }
            set { if ((ReferenceEquals(this.AccountType, value) != true)) { this.accountType = value; } }
        }


        [DataMember]
        public DivisionMethodEnum DivisionMethod
        {
            get { return this.divisionMethod; }
            set { if ((ReferenceEquals(this.DivisionMethod, value) != true)) { this.divisionMethod = value; } }
        }

        [DataMember]
        public InvoiceTypeEnum InvoiceType
        {
            get { return this.invoiceType; }
            set { if ((ReferenceEquals(this.InvoiceType, value) != true)) { this.invoiceType = value; } }
        }

        //        public long InvoiceTypeId
        //        {
        //            get { return (long) invoiceType; }
        //            set { if ((object.ReferenceEquals(this.InvoiceType, value) != true)) {this.invoiceType, (InvoiceTypeEnum) value); }
        //        }

        [DataMember]
        public InvoiceDto InvoiceRefrence
        {
            get { return this.invoiceRefrence; }
            set { if ((ReferenceEquals(this.InvoiceRefrence, value) != true)) { this.invoiceRefrence = value; } }
        }

        [DataMember]
        public ObservableCollection<OrderDto> OrderRefrences
        {
            get { return this.orderRefrences; }
            set { if ((ReferenceEquals(this.OrderRefrences, value) != true)) { this.orderRefrences = value; } }
        }

        [DataMember]
        public ObservableCollection<InvoiceItemDto> InvoiceItems
        {
            get { return this.invoiceItems; }
            set { if ((ReferenceEquals(this.InvoiceItems, value) != true)) { this.invoiceItems = value; } }
        }

        [DataMember]
        public long OwnerId
        {
            get { return this.ownerId; }
            set { if ((ReferenceEquals(this.OwnerId, value) != true)) { this.ownerId = value; } }
        }

        [DataMember]
        public ObservableCollection<InvoiceAdditionalPriceDto> AdditionalPrices
        {
            get { return this.additionalPrices; }
            set { if ((ReferenceEquals(this.AdditionalPrices, value) != true)) { this.additionalPrices = value; } }
        }

        [DataMember]
        public decimal TotalOfDivisionPrice
        {
            get { return this.totalOfadditionalPrice; }
            set { if ((ReferenceEquals(this.TotalOfDivisionPrice, value) != true)) { this.totalOfadditionalPrice = value; } }
        }

        #endregion
    }
}
