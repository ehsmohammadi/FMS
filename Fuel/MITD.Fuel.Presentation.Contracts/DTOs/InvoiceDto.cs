#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

#endregion

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class InvoiceDto
    {
        #region props

        private AccountingTypesEnum accountType;
        private ObservableCollection<InvoiceAdditionalPriceDto> additionalPrices;
        private WorkflowStageEnum approveStatus;
        private long companyId;
        private long currencyId;
        private string description;
        private long id;
        private DateTime invoiceDate;
        private ObservableCollection<InvoiceItemDto> invoiceItems;
        private string invoiceNumber;
        private InvoiceDto invoiceRefrence;
        private InvoiceTypeEnum invoiceType;
        private ObservableCollection<OrderDto> orderRefrences;
        private long ownerId;
        private long? supplierId;
        private string supplierName;
        private long? transporterId;
        private string transporterName;
        private bool isCreditor;
        private decimal totalOfadditionalPrice;
      

        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        public bool IsCreditor
        {
            get { return isCreditor; }
            set { this.SetField(p => p.IsCreditor, ref isCreditor, value); }
        }


        public long CompanyId
        {
            get { return companyId; }
            set { this.SetField(p => p.CompanyId, ref companyId, value); }
        }

        public long CurrencyId
        {
            get { return currencyId; }
            set { this.SetField(p => p.CurrencyId, ref currencyId, value); }
        }

        public DateTime InvoiceDate
        {
            get { return invoiceDate; }
            set { this.SetField(p => p.InvoiceDate, ref invoiceDate, value); }
        }


        public WorkflowStageEnum ApproveStatus
        {
            get { return approveStatus; }
            set { this.SetField(p => p.ApproveStatus, ref approveStatus, value); }
        }

        public string ApproveStatusString
        {
            get { return approveStatus.GetDescription(); }
        }

        public string Description
        {
            get { return description; }
            set { this.SetField(p => p.Description, ref description, value); }
        }


        public string InvoiceNumber
        {
            get { return invoiceNumber; }
            set { this.SetField(p => p.InvoiceNumber, ref invoiceNumber, value); }
        }


        public long? TransporterId
        {
            get { return transporterId; }
            set { this.SetField(p => p.TransporterId, ref transporterId, value); }
        }

        public string TransporterName
        {
            get { return transporterName; }
            set { this.SetField(p => p.TransporterName, ref transporterName, value); }
        }

        public long? SupplierId
        {
            get { return supplierId; }
            set { this.SetField(p => p.SupplierId, ref supplierId, value); }
        }

        public string SupplierName
        {
            get { return supplierName; }
            set { this.SetField(p => p.SupplierName, ref supplierName, value); }
        }


        public long AccountTypeId
        {
            get { return (long)accountType; }
            set { this.SetField(p => p.AccountType, ref accountType, (AccountingTypesEnum)value); }
        }


        public AccountingTypesEnum AccountType
        {
            get { return accountType; }
            set { this.SetField(p => p.AccountType, ref accountType, value); }
        }

        private DivisionMethodEnum divisionMethod;


        public DivisionMethodEnum DivisionMethod
        {
            get { return divisionMethod; }
            set { this.SetField(p => p.DivisionMethod, ref divisionMethod, value); }
        }

        public InvoiceTypeEnum InvoiceType
        {
            get { return invoiceType; }
            set { this.SetField(p => p.InvoiceType, ref invoiceType, value); }
        }

        //        public long InvoiceTypeId
        //        {
        //            get { return (long) invoiceType; }
        //            set { this.SetField(p => p.InvoiceType, ref invoiceType, (InvoiceTypeEnum) value); }
        //        }

        public InvoiceDto InvoiceRefrence
        {
            get { return invoiceRefrence; }
            set { this.SetField(p => p.InvoiceRefrence, ref invoiceRefrence, value); }
        }

        public ObservableCollection<OrderDto> OrderRefrences
        {
            get { return orderRefrences; }
            set { this.SetField(p => p.OrderRefrences, ref orderRefrences, value); }
        }

        public ObservableCollection<InvoiceItemDto> InvoiceItems
        {
            get { return invoiceItems; }
            set { this.SetField(p => p.InvoiceItems, ref invoiceItems, value); }
        }

        public long OwnerId
        {
            get { return ownerId; }
            set { this.SetField(p => p.OwnerId, ref ownerId, value); }
        }

        public ObservableCollection<InvoiceAdditionalPriceDto> AdditionalPrices
        {
            get { return additionalPrices; }
            set { this.SetField(p => p.AdditionalPrices, ref additionalPrices, value); }
        }

        string currentStateName;
        public string CurrentStateName
        {
            get { return this.currentStateName; }
            set { this.SetField(p => p.CurrentStateName, ref this.currentStateName, value); }
        }

        string userInChargName;
        public string UserInChargName
        {
            get { return this.userInChargName; }
            set { this.SetField(p => p.UserInChargName, ref this.userInChargName, value); }
        }

        private string vesselsNames;
        public string VesselsNames
        {
            get { return this.vesselsNames; }
            set { this.SetField(p => p.VesselsNames, ref this.vesselsNames ,value); }
        }

        private string voyagesNumbers;
        public string VoyagesNumbers
        {
            get { return this.voyagesNumbers; }
            set { this.SetField(p => p.VoyagesNumbers, ref this.voyagesNumbers,value); }
        }

        private string orderNumbers;
        public string OrderNumbers
        {
            get { return this.orderNumbers; }
            set { this.SetField(p => p.OrderNumbers, ref this.orderNumbers, value); }
        }

        private string fuelReportDetailIds;
        public string FuelReportDetailIds
        {
            get { return this.fuelReportDetailIds; }
            set { this.SetField(p => p.FuelReportDetailIds, ref this.fuelReportDetailIds, value); }
        }

        private ObservableCollection<DateTime> fuelReportsDateTimes;
        public ObservableCollection<DateTime> FuelReportsDateTimes
        {
            get { return this.fuelReportsDateTimes; }
            set { this.SetField(p=>p.FuelReportsDateTimes, ref this.fuelReportsDateTimes,  value); }
        }

        private string currencyName;
        public string CurrencyName
        {
            get { return this.currencyName; }
            set { this.SetField(p => p.CurrencyName, ref this.currencyName, value); }
        }

        //public string FuelReportDateTimesText
        //{
        //    get { return this.fuelReportDateTimesText; }
        //    set { this.SetField(p=>p.FuelReportDateTimesText, ref this.fuelReportDateTimesText , value); }
        //}
        //private string fuelReportDateTimesText;

        #endregion
    }
}