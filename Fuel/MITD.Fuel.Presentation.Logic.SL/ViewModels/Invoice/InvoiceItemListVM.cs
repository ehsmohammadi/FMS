#region

using System;
using System.Collections.ObjectModel;
using Castle.Windsor;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    
    public class InvoiceItemListVM : WorkspaceViewModel, IEventHandler<InvoiceListSelectedIndexChangeEvent>, IEventHandler<InvoiceItemListChangeArg>
                                                
    {
        #region props

        private readonly IInvoiceItemController invoiceItemController;
        private readonly IFuelController mainController;
        private readonly IInvoiceServiceWrapper serviceWrapper;
        private PagedSortableCollectionView<InvoiceItemDto> data;
        private CommandViewModel deleteCommand;
        private CommandViewModel editCommand;
        //private CommandViewModel nextPageCommand;


        private ObservableCollection<GoodDto> goodDtos;
        private IGoodServiceWrapper goodServiceWrapper;

        //private InvoiceItemVM selectedInvoiceItemVm;
        private InvoiceItemDto selectedInvoiceItem;

        public PagedSortableCollectionView<InvoiceItemDto> Data
        {
            get { return data; }
            set { this.SetField(p => p.Data, ref data, value); }
        }

        public InvoiceItemDto SelectedInvoiceItem
        {
            get { return selectedInvoiceItem; }
            set { this.SetField(p => p.SelectedInvoiceItem, ref selectedInvoiceItem, value); }
        }

        public Guid UniqId
        {
            get;
            set;
        }

        #endregion

        #region Commands


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

                                       if (mainController.ShowConfirmationBox("آیا برای حذف مطمئن هستید ", "اخطار"))
                                           if (SelectedInvoiceItem.Id == 0)
                                           {
                                               Data.Remove(SelectedInvoiceItem);
                                           }
                                           else
                                           {
                                               ShowBusyIndicator("درحال حذف ");
                                               serviceWrapper.DeleteItem
                                                   (
                                                       (res, exp) => this.mainController.BeginInvokeOnDispatcher
                                                           (
                                                            () =>
                                                            {
                                                                this.HideBusyIndicator();
                                                                if (exp != null)
                                                                {
                                                                    this.mainController.HandleException(exp);
                                                                }
                                                                else
                                                                {
                                                                    this.Data.Remove(this.SelectedInvoiceItem);
                                                                    this.mainController.Publish(new InvoiceItemListChangeArg());
                                                                }
                                                                this.HideBusyIndicator();
                                                            }), this.SelectedInvoiceItem);
                                           }
                                   })));
            }
        }

        #endregion

        #region ctor

        public InvoiceItemListVM()
        {
        }

        public InvoiceItemListVM(IInvoiceItemController controller,
                                 IFuelController mainController,
                                 IInvoiceServiceWrapper invoiceServiceWrapper,
                                 IGoodServiceWrapper goodServiceWrapper)
        {
            invoiceItemController = controller;
            serviceWrapper = invoiceServiceWrapper;
            this.mainController = mainController;
            this.goodServiceWrapper = goodServiceWrapper;
            DisplayName = "InvoiceItem";


            data = new PagedSortableCollectionView<InvoiceItemDto>();
            data.PageChanged += data_PageChanged;
        }

        void data_PageChanged(object sender, EventArgs e)
        {
            GetInvoiceItems();
        }

        #endregion

        #region methods

        private InvoiceDto SelectedInvoice
        {
            get;
            set;
        }

        #region IEventHandler<InvoiceItemListChangeArg> Members

        public void Handle(InvoiceItemListChangeArg eventData)
        {
            GetInvoiceItems();
        }

        #endregion

        #region IEventHandler<InvoiceListSelectedIndexChangeEvent> Members

        public void Handle(InvoiceListSelectedIndexChangeEvent eventData)
        {
            if (UniqId != eventData.UniqId)
                return;
            if (eventData.Entity == null)
                Data.SourceCollection = new Collection<InvoiceItemDto>();
            SelectedInvoice = eventData.Entity;
            Data.SourceCollection = eventData.Entity.InvoiceItems;
            //GetInvoiceItems();
        }

        #endregion

        private bool CheckIsSelected()
        {
            if (SelectedInvoiceItem == null)
            {
                mainController.ShowMessage("لطفا قلم صورتحساب را انتخاب نمائید");
                return false;
            }
            return true;
        }

       


        private void GetInvoiceItems()
        {
            if (SelectedInvoice == null)
                return;
            ShowBusyIndicator("در حال دریافت اطلاعات قلم ها");
            serviceWrapper.GetById
                (
                    (res, exp) => mainController.BeginInvokeOnDispatcher
                        (
                            () =>
                                {
                                    if (exp == null)
                                    {
                                        Data.SourceCollection = res.InvoiceItems;
                                    }
                                    else
                                        mainController.HandleException(exp);

                                    HideBusyIndicator();
                                }), SelectedInvoice.Id);
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            mainController.Close(this);
        }

        #endregion
    }
}