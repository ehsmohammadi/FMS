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
namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class OrderItemListVM : WorkspaceViewModel, IEventHandler<OrderItemListChangeArg>
    {
        #region props

       
        private IOrderItemController orderItemController;
        private IFuelController appController;
        private IOrderServiceWrapper serviceWrapper;
        private IGoodServiceWrapper goodServiceWrapper;



        private CommandViewModel addCommand;
        private CommandViewModel editCommand;
        private CommandViewModel deleteCommand;
        //private CommandViewModel nextPageCommand;

        private PagedSortableCollectionView<OrderItemVM> orderItemVms;
        public PagedSortableCollectionView<OrderItemVM> OrderItemVms
        {
            get { return orderItemVms; }
            set
            {
                this.SetField(p => p.OrderItemVms, ref orderItemVms, value);

            }
        }



        private ObservableCollection<GoodDto> goodDtos;

        private OrderItemVM selectedOrderItemVm;
        public OrderItemVM SelectedOrderItemVm
        {
            get { return selectedOrderItemVm; }
            set
            {
                this.SetField(p => p.SelectedOrderItemVm, ref selectedOrderItemVm, value);
            }
        }

        private OrderVM orderVMSelected;
        public OrderVM OrderVMSelected
        {
            get { return orderVMSelected; }
            set
            {
                this.SetField(p => p.OrderVMSelected, ref orderVMSelected, value);

                if (OrderItemVms != null)
                    OrderItemVms.Clear();

                if (orderVMSelected != null)
                    if (orderVMSelected.Entity.Id != -1)
                    {
                        OrderItemVms.Clear();
                        OrderItemVms.SourceCollection = createOrderItem(orderVMSelected.Entity);
                    }
            }
        }


        public CommandViewModel AddCommand
        {
            get
            {

                if (addCommand == null)
                {
                    addCommand = new CommandViewModel("افزودن",
                        new DelegateCommand(() =>
                        {
                            if (OrderVMSelected.Entity.Id>0)
                            {
                                orderItemController.Add(orderVMSelected.Entity);
                            }
                            else
                            {
                                appController.ShowMessage("لطفا ابتدا سفارش مورد نظر را انتخاب نمایید");
                            }
                          

                        }));
                }
                return addCommand;

            }
        }

        public CommandViewModel EditCommand
        {
            get
            {
                if (editCommand == null)
                {
                    editCommand = new CommandViewModel("ویرایش",
                        new DelegateCommand(() =>
                        {
                            if (checkIsSelected())
                            {
                                orderItemController.Edit(SelectedOrderItemVm.Entity);  
                            }
                        }));
                }
                return editCommand;

            }
        }

        public CommandViewModel DeleteCommand
        {
            get
            {
                
                if (deleteCommand == null)
                {
                    deleteCommand = new CommandViewModel("حذف",
                        new DelegateCommand(() =>
                                                {
                                                    
                            if (!checkIsSelected()) return;

                            if (appController.ShowConfirmationBox("آیا برای حذف مطمئن هستید ",
                                                                                    "اخطار"))
                            if (SelectedOrderItemVm.Entity.Id == 0)
                            {
                                this.OrderItemVms.Remove(SelectedOrderItemVm);

                            }
                            else
                            {
                                ShowBusyIndicator("درحال حذف ");
                                serviceWrapper.DeleteItem((res, exp) => this.appController.BeginInvokeOnDispatcher(() =>
                                {
                                    this.HideBusyIndicator();
                                    if (exp != null)
                                    {
                                        this.appController.HandleException(exp);
                                    }
                                    else
                                    {
                                        
                                        this.OrderItemVms.Remove(this.SelectedOrderItemVm);
                                        this.orderVMSelected.OrderItemVms.Remove(this.SelectedOrderItemVm);
                                        this.appController.Publish<OrderItemListChangeArg>(new OrderItemListChangeArg());
                                    }
                                    this.HideBusyIndicator();
                                }), this.SelectedOrderItemVm.Entity);
                            }




                        }));
                }
                return deleteCommand;
            }
        }
        //public CommandViewModel NextPageCommand
        //{
        //    get
        //    {
        //        if (nextPageCommand == null)
        //        {
        //            nextPageCommand = new CommandViewModel("صفحه بعد", new DelegateCommand(
        //                                                                    () =>
        //                                                                    {
        //                                                                        this.OrderItemVms.PageIndex++;
        //                                                                        this.OrderItemVms.Refresh();
        //                                                                    }));


        //        }
        //        return nextPageCommand;

        //    }
        //}
        #endregion

        #region ctor

        public OrderItemListVM()
        {

        }

        public OrderItemListVM(IOrderItemController controller, IFuelController mainController,
                                      IOrderServiceWrapper serviceWrapper, IGoodServiceWrapper goodServiceWrapper)
        {
            Init(controller, mainController, serviceWrapper, goodServiceWrapper);


        }

        #endregion

        #region methods
        void Init(IOrderItemController controller, IFuelController mainController, IOrderServiceWrapper orderServiceWrapper, IGoodServiceWrapper goodServiceWrapper)
        {
            this.orderItemController = controller;
            this.serviceWrapper = orderServiceWrapper;
            this.appController = mainController;
            this.goodServiceWrapper = goodServiceWrapper;
            DisplayName = "OrderItem";

            OrderVMSelected = ServiceLocator.Current.GetInstance<OrderVM>();

            orderItemVms = new PagedSortableCollectionView<OrderItemVM>();
            orderItemVms.OnRefresh += (s, args) => GetOrderItems();

        }



        private bool checkIsSelected()
        {
            if (SelectedOrderItemVm == null)
            {
                appController.ShowMessage("لطفا قلم سفارش را انتخاب نمائید");
                return false;
            }
            else return true;
        }

        public void GetOrderItems()
        {
            if (orderVMSelected != null)
            {
               ShowBusyIndicator("در حال دریافت اطلاعات قلم ها");
                serviceWrapper.GetById((res,exp)=>this.appController.BeginInvokeOnDispatcher(()=>
                                                                                                {

                                                                                                    if (exp==null)
                                                                                                    {
                                                                                                        orderItemVms.Clear();
                                                                                                        orderItemVms.
                                                                                                              SourceCollection
                                                                                                              = createOrderItem(res);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                       appController.HandleException(exp);
                                                                                                    }
                                                                                                  
                                                                                                    HideBusyIndicator();
                                                                                                    
                                                                                                }),OrderVMSelected.Entity.Id); 
            }
        }

        private ObservableCollection<OrderItemVM> createOrderItem(OrderDto dto)
        {
            var result = new ObservableCollection<OrderItemVM>();
            if (dto.OrderItems != null && dto.OrderItems.Count > 0)
            {
                dto.OrderItems.ToList().ForEach(c =>
                {
                    var x = ServiceLocator.Current.GetInstance<OrderItemVM>();
                    x.Entity = c;
                    result.Add(x);
                });

            }
            return result;
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            appController.Close(this);
        }

        public void Handle(OrderItemListChangeArg eventData)
        {
            GetOrderItems();
        }
        #endregion


    
    }
}
