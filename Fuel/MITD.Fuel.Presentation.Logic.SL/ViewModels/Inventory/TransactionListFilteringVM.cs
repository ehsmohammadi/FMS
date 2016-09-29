using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class TransactionListFilteringVM : WorkspaceViewModel
    {
        private ObservableCollection<Inventory_CompanyDto> companies;
        public ObservableCollection<Inventory_CompanyDto> Companies
        {
            get { return companies; }
            set { this.SetField(p => p.Companies, ref companies, value); }
        }
        
        private Inventory_CompanyDto selectedCompany;
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Company should be selected")]

        public Inventory_CompanyDto SelectedCompany
        {
            get { return selectedCompany; }
            set { this.SetField(p => p.SelectedCompany, ref selectedCompany, value); }
        }

        public long? SelectedCompanyId
        {
            get { return (SelectedCompany == null || SelectedCompany.Id == long.MinValue) ? null : (long?)SelectedCompany.Id; }
        }

        private ObservableCollection<Inventory_WarehouseDto> warehouse;
        public ObservableCollection<Inventory_WarehouseDto> Warehouse
        {
            get { return this.warehouse; }
            set { this.SetField(p => p.Warehouse, ref this.warehouse, value); }
        }

        private Inventory_WarehouseDto selectedWarehouse;
        public Inventory_WarehouseDto SelectedWarehouse
        {
            get { return selectedWarehouse; }
            set { this.SetField(p => p.SelectedWarehouse, ref selectedWarehouse, value); }
        }

        public long? SelectedWarehouseId
        {
            get { return (SelectedWarehouse == null || SelectedWarehouse.Id == long.MinValue) ? null : (long?)SelectedWarehouse.Id; }
        }

        private List<ComboBoxItm>transactionTypes;
        public List<ComboBoxItm> TransactionTypes
        {
            get { return transactionTypes; }
            set { this.SetField(p => p.TransactionTypes, ref transactionTypes, value); }
        }

        private long selectedTransactionType;
        public long SelectedTransactionType
        {
            get { return selectedTransactionType; }
            set { this.SetField(p => p.SelectedTransactionType, ref selectedTransactionType, value); }
        }

        private List<ComboBoxItm> transactionStatus;
        public List<ComboBoxItm> TransactionStatus
        {
            get { return transactionStatus; }
            set { this.SetField(p => p.TransactionStatus, ref transactionStatus, value); }
        }
        //TransactionStatusEnum
        private long selectedtransactionStatus;
        public long SelectedTransactionStatus
        {
            get { return selectedtransactionStatus; }
            set { this.SetField(p => p.SelectedTransactionStatus, ref selectedtransactionStatus, value); }
        }

        private DateTime? fromDate;
        public DateTime? FromDate
        {
            get { return fromDate; }
            set { this.SetField(p => p.FromDate, ref fromDate, value); }
        }

        private DateTime? toDate;
        public DateTime? ToDate
        {
            get { return toDate; }
            set { this.SetField(p => p.ToDate, ref toDate, value); }
        }

        public TransactionListFilteringVM()
        {
            this.Companies = new ObservableCollection<Inventory_CompanyDto>();
            this.Warehouse = new ObservableCollection<Inventory_WarehouseDto>();
        }

        public void Initialize(IEnumerable<Inventory_CompanyDto> companyDtos)
        {
            this.Companies.Clear();
            this.Companies.Add(new Inventory_CompanyDto()
                                {
                                  Code = decimal.MinValue,
                                  CreateDate = null,
                                  Name = "انتخاب نمایید...",
                                  Id = 0,
                                  IsActive = false,
                                  UserCreator = new Inventory_UserDto(),
                                  UserCreatorId = int.MaxValue
                                });
            foreach (var company in companyDtos)
            {
                this.Companies.Add(company);
            }
            TransactionTypes = (typeof(TransactionTypeEnum)).ToComboItemList();
            TransactionStatus = (typeof(TransactionStatusEnum)).ToComboItemList();


            ResetToDefaults();
        }

        public void ResetToDefaults()
        {
            this.SelectedCompany = this.Companies.Count == 2 ? this.Companies[1] : null;

            this.SelectedWarehouse = null;

            this.FromDate = null;

            this.ToDate = null;
        }
    }
}
