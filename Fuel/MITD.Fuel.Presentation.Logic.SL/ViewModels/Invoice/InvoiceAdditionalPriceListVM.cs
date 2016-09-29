#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Castle.Core.Internal;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;

#endregion

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels.Invoice
{
    public class InvoiceAdditionalPriceListVM : WorkspaceViewModel, IEventHandler<InvoiceAdditionalPriceEditedArg>
    {
        #region props

        #region injected fields

        private readonly IInvoiceController controller;
        private readonly IFuelController mainController;
        private readonly IInvoiceServiceWrapper serviceWrapper;

        #endregion

        #region filter

        private InvoiceDto invoice;

        public InvoiceDto Invoice
        {
            get { return invoice; }
            set { this.SetField(v => v.Invoice, ref invoice, value); }
        }

        #endregion

        #region selected & main data

        private ObservableCollection<InvoiceAdditionalPriceDto> data;
        private ObservableCollection<EffectiveFactorDto> effectiveFactors;
        private InvoiceAdditionalPriceDto selectedAdditionalPrice;

        public InvoiceAdditionalPriceDto SelectedAdditionalPrice
        {
            get { return selectedAdditionalPrice; }
            set { this.SetField(p => p.SelectedAdditionalPrice, ref selectedAdditionalPrice, value); }
        }

        public ObservableCollection<InvoiceAdditionalPriceDto> Data
        {
            get { return data; }
            set { this.SetField(p => p.Data, ref data, value); }
        }

        public ObservableCollection<EffectiveFactorDto> EffectiveFactors
        {
            get { return effectiveFactors; }
            set { this.SetField(p => p.EffectiveFactors, ref effectiveFactors, value); }
        }

        #endregion

        #region commands

        private CommandViewModel addCommand;
        private CommandViewModel submitCommand;
        private CommandViewModel deleteCommand;
        private CommandViewModel editCommand;
        private decimal currencyToMainCurrencyRate;

        //command props


        public CommandViewModel EditCommand
        {
            get
            {
                return editCommand ?? (editCommand = new CommandViewModel
                    (
                    "ویرایش", new DelegateCommand
                                  (
                                  () =>
                                  {
                                      if (!CheckIsSelected())
                                          return;
                                      controller.EditAdditionalPrice(SelectedAdditionalPrice, effectiveFactors, currencyToMainCurrencyRate);
                                  })));
            }
        }

        public CommandViewModel AddCommand
        {
            get
            {
                SelectedAdditionalPrice = new InvoiceAdditionalPriceDto();
                return addCommand ?? (addCommand = new CommandViewModel("افزودن", new DelegateCommand(() =>
                    controller.AddAdditionalPrice(effectiveFactors, currencyToMainCurrencyRate, UniqId))));
            }
        }

        public CommandViewModel DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new CommandViewModel
                    (
                    "حذف", new DelegateCommand
                               (
                               () =>
                               {
                                   if (!CheckIsSelected())
                                       return;


                                   if (!mainController.ShowConfirmationBox("آیا برای حذف مطمئن هستید ", "اخطار"))
                                       return;

                                   ShowBusyIndicator("درحال انجام حذف");
                                   Data.Remove(SelectedAdditionalPrice);
                               })));
            }
        }

        public CommandViewModel SubmitCommand
        {
            get
            {
                return submitCommand ?? (submitCommand = new CommandViewModel
                    (
                    "خروج", new DelegateCommand
                                (
                                () =>
                                {
                                    //Invoice.AdditionalPrices = Data;
                                    mainController.Publish(new InvoiceAdditionalPriceListChangedArg(Data));
                                    this.mainController.Close(this);
                                })));
            }
        }

        #endregion

        #region column visibility

        #endregion

        #endregion

        #region ctor

        public InvoiceAdditionalPriceListVM()
        {
            //FromDateFilter = DateTime.Now;
        }

        // vesselServiceWrapper must be added ***********************
        public InvoiceAdditionalPriceListVM(IInvoiceController controller, IFuelController mainController, IInvoiceServiceWrapper serviceWrapper)
        {
            this.controller = controller;
            this.serviceWrapper = serviceWrapper;
            this.mainController = mainController;

            // InvoiceTypesVM = new EnumVM<InvoiceTypeEnum>();

            DisplayName = "صورتحساب";
            Data = new PagedSortableCollectionView<InvoiceAdditionalPriceDto>();
            // Data.OnRefresh += (s, e) => LoadInvoicesByFilters(Data.PageIndex);

            //filters
            //            CompaniesFilter = new List<CompanyDto>();
            //            InvoiceCreatorsFilter = new ObservableCollection<UserDto>();
            // InvoiceTypesVM.SelectedItem = InvoiceTypesVM.Items.FirstOrDefault();
            //            FromDateFilter = DateTime.Now.AddMonths(-1);
            //            ToDateFilter = DateTime.Now;

            //inline editing 
            // approcalServiceWrapper = _approcalServiceWrapper;

            // Data.OnRefresh += (s, args) => Load(TODO);
        }

        #endregion

        #region methods

        private bool CheckIsSelected()
        {
            if (SelectedAdditionalPrice == null)
            {
                mainController.ShowMessage("لطفا سفارش مورد نظر را انتخاب فرمائید");
                return false;
            }
            else
                return true;
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            mainController.Close(this);
        }

        #region loading data

        public Guid UniqId { get; set; }

        public void Load(InvoiceDto currentInvoice, decimal currencyToMainCurrencyRate, Guid uniqId)
        {
            Invoice = currentInvoice;
            //Data = Invoice.AdditionalPrices??new PagedSortableCollectionView<InvoiceAdditionalPriceDto>();
            Data = new ObservableCollection<InvoiceAdditionalPriceDto>(currentInvoice.AdditionalPrices);
            Data.ForEach(p=>p.CurrencyToMainCurrencyRate = currencyToMainCurrencyRate); //Set Exchange to Main Currency to each item.

            UniqId = Guid.NewGuid();
            ShowBusyIndicator("درحال دريافت اطلاعات ............");
            this.currencyToMainCurrencyRate = currencyToMainCurrencyRate;

            serviceWrapper.GetEffectiveFactors
                (
                    (res, exp) => mainController.BeginInvokeOnDispatcher
                        (
                            () =>
                            {
                                HideBusyIndicator();
                                if (exp == null)
                                {
                                    EffectiveFactors = res;
                                }
                                else
                                {
                                    mainController.HandleException(exp);
                                }
                            }));
        }

        #endregion

        #endregion
        public void Handle(InvoiceAdditionalPriceEditedArg eventData)
        {
            if (eventData.UniqId != UniqId)
                return;
            if (Data.Count(c => c.EffectiveFactorId == eventData.InvoiceAdditionalPrice.EffectiveFactorId) > 0)
            {
                MessageBox.Show("فاکتور های تاثیر گذار نباید تکراری باشد");
                return;
            }
            Data.Add(eventData.InvoiceAdditionalPrice);
        }
    }
}