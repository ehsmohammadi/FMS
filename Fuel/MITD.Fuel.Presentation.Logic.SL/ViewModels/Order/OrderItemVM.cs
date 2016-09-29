using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ServiceWrapper;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using System.Linq;
namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{

    public class OrderItemVM : WorkspaceViewModel
    {
        #region Prop

        private IFuelController mainController;
        private IOrderServiceWrapper serviceWrapper;
        private IGoodServiceWrapper _goodServiceWrapper;

        private CommandViewModel submitCommand;
        public CommandViewModel SubmitCommand
        {
            get
            {
                if (submitCommand == null)
                {

                    submitCommand = new CommandViewModel("ذخیره", new DelegateCommand(save));
                }
                return submitCommand;
            }
        }

        private CommandViewModel cancelCommand;
        public CommandViewModel CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {

                    cancelCommand = new CommandViewModel("خروج", new DelegateCommand(() =>
                                                                                           {
                                                                                               this.mainController.Close(this);
                                                                                           }));
                }
                return cancelCommand;
            }
        }

        private CommandViewModel refreshCommand;
        public CommandViewModel RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {

                    refreshCommand = new CommandViewModel("بروزرسانی", new DelegateCommand(() =>
                                                                                           {
                                                                                               GetManiUnit();
                                                                                           }));
                }
                return refreshCommand;
            }
        }

        private long orderId;
        private long orderitemId;
        private long goodId;
        public long GoodId
        {
            get { return goodId; }
            set
            {
                if (value != 0)
                {

                    if (value != GoodId)
                    {
                        this.SetField(p => p.GoodId, ref goodId, value);
                        if (Entity.Good != null && Entity.Good.Unit != null)
                            UnitId = Entity.Good.Unit.Id;
                        else
                            UnitId = 0;
                        Entity.Good = GoodDtos.Where(c => c.Id == value).SingleOrDefault();
                    }


                }

            }
        }

        private long unitId;
        public long UnitId
        {
            get { return unitId; }
            set
            {

                this.SetField(p => p.UnitId, ref unitId, value);
            }
        }




        private OrderItemDto entity;
        public OrderItemDto Entity
        {
            get { return entity; }
            set
            {

                this.SetField(p => p.Entity, ref entity, value);


            }
        }

        private OrderDto parentEntity;
        public OrderDto ParentEntity
        {
            get { return parentEntity; }
            set
            {

                this.SetField(p => p.ParentEntity, ref parentEntity, value);

            }
        }

        private MainUnitValueDto mainUnitValueDto;
        public MainUnitValueDto MainUnitValueDto
        {
            get { return mainUnitValueDto; }
            set { this.SetField(p => p.MainUnitValueDto, ref mainUnitValueDto, value); }
        }



        private ObservableCollection<GoodDto> goodDtos;
        public ObservableCollection<GoodDto> GoodDtos
        {
            get
            {
                return goodDtos;
            }
            set
            {

                this.SetField(vm => vm.GoodDtos, ref goodDtos, value);




            }
        }

        private ObservableCollection<GoodUnitDto> unitDtos;

        public ObservableCollection<GoodUnitDto> UnitDtos
        {


            get
            {

                return unitDtos;
            }
            set
            {
                this.SetField(vm => vm.UnitDtos, ref unitDtos, value);
            }

        }



        #endregion

        #region ctor



        public OrderItemVM()
        {


        }
        public OrderItemVM(IFuelController appController, IOrderServiceWrapper orderServiceWrapper, IGoodServiceWrapper goodServiceWrapper)
        {
            this.mainController = appController;
            this.serviceWrapper = orderServiceWrapper;

            setEntity(new OrderItemDto());

            Entity.Good = new GoodDto();
            Entity.Good.Unit = new GoodUnitDto();

            UnitDtos = new ObservableCollection<GoodUnitDto>();
            GoodDtos = new ObservableCollection<GoodDto>();

            DisplayName = "افزودن/اصلاح سفارش ";
            _goodServiceWrapper = goodServiceWrapper;

            this.PropertyChanged += OrderItemVM_PropertyChanged;
        }

        void OrderItemVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.GetPropertyName(p => p.UnitId))
                GetManiUnit();
        }


        #endregion

        #region Method

        public void SetProp(OrderDto order)
        {

            parentEntity = order;
            GetGoods();


        }


        #endregion

        #region methods

        void GetGoods()
        {
            this._goodServiceWrapper.GetAll((res, exp) => this.mainController.BeginInvokeOnDispatcher(() =>
            {
                ShowBusyIndicator(" درحال دریافت اطلاعات نوع سوخت ............");

                if (exp == null)
                {


                    res.ToList().ForEach(p => GoodDtos.Add(p));
                    GetOrderItem();

                }
                else
                {
                    mainController.HandleException(exp);
                }

                HideBusyIndicator();
            }), "GetAll", mainController.GetCurrentUser().User.CompanyDto.Id);

        }

        void GetOrderItem()
        {

            ShowBusyIndicator("در حال دریافت اطلاعات جزئیات سفارش ...");
            if (orderId != 0 && orderitemId != 0)
                serviceWrapper.GetItem((res, exp) => mainController.BeginInvokeOnDispatcher(() =>
                                                                                             {

                                                                                                 if (exp == null)
                                                                                                 {
                                                                                                     setEntity(res);

                                                                                                     GoodId = res.Good.Id;
                                                                                                 }
                                                                                                 else
                                                                                                 {
                                                                                                     mainController.HandleException(exp);
                                                                                                 }

                                                                                                 HideBusyIndicator();
                                                                                             }), orderId, orderitemId);
        }

        private void setEntity(OrderItemDto dto)
        {
            Entity = dto;
            Entity.PropertyChanged += Entity_PropertyChanged;
        }

        void Entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Entity.GetPropertyName(p => p.Quantity))
                GetManiUnit();
        }

        private void save()
        {
            if (!Entity.Validate()) return;

            ShowBusyIndicator("درحال ذخیره سازی");
            Entity.Good.Id = GoodId;
            Entity.Good.Unit = new GoodUnitDto();
            Entity.Good.Unit.Id = UnitId;
            if (Entity.Id == 0)
            {
                Entity.OrderId = parentEntity.Id;
                serviceWrapper.AddItem((res, exp) => mainController.BeginInvokeOnDispatcher(() =>
                                                                                                {

                                                                                                    if (exp != null)
                                                                                                    {
                                                                                                        mainController.HandleException(exp);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        setEntity(res);

                                                                                                        mainController.Publish<OrderItemListChangeArg>(new OrderItemListChangeArg());

                                                                                                    }
                                                                                                    HideBusyIndicator();
                                                                                                    mainController.Close(this);
                                                                                                }), Entity);
            }
            else
            {
                serviceWrapper.UpdateItem((res, exp) => mainController.BeginInvokeOnDispatcher(() =>
                                                                                                   {

                                                                                                       if (exp != null)
                                                                                                           mainController.HandleException(exp);
                                                                                                       else
                                                                                                       {
                                                                                                           setEntity(res);

                                                                                                           mainController.Publish<OrderItemListChangeArg>(new OrderItemListChangeArg());

                                                                                                       }
                                                                                                       HideBusyIndicator();
                                                                                                       mainController.Close(this);
                                                                                                   }), Entity);
            }


        }

        public void Load(OrderItemDto orderItemDto)
        {
            orderId = orderItemDto.OrderId;
            orderitemId = orderItemDto.Id;
            GetGoods();
        }

        public void GetManiUnit()
        {
            MainUnitValueDto = new MainUnitValueDto();

            if (GoodId == 0 || UnitId == 0) return;

            ShowBusyIndicator("بروزرسانی واحد اصلی ...");
            serviceWrapper.GetMainUnit((res, exp) => mainController.BeginInvokeOnDispatcher(() =>
            {
                if (exp == null)
                {
                    MainUnitValueDto = res;
                }
                else
                {
                    mainController.HandleException(exp);
                }
                HideBusyIndicator();

            }), GoodId, UnitId, Entity.Quantity);
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            mainController.Close(this);
        }

        #endregion
    }
}
