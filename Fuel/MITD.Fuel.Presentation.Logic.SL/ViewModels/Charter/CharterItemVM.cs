using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
using MITD.Fuel.Presentation.Contracts.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class CharterItemVM : WorkspaceViewModel
    {
        #region Prop
        CharterType CurrentCharterType { get; set; }
        CharterStateTypeEnum CurrentStateTypeEnum { get; set; }
        private IFuelController _fuelController;
        private ICharterInServiceWrapper _charterInServiceWrapper;
        private ICharterOutServiceWrapper _charterOutServiceWrapper;


        private CharterItemDto entity;
        public CharterItemDto Entity
        {
            get { return entity; }
            set { this.SetField(p => p.Entity, ref entity, value); }
        }


        private CommandViewModel cancelCommand;
        public CommandViewModel CancelCommand
        {
            get
            {
                cancelCommand = new CommandViewModel("خروج", new DelegateCommand(() =>
                {

                    this._fuelController.Close(this);

                }));
                return cancelCommand;
            }

        }


        private CommandViewModel submitCommand;
        public CommandViewModel SubmitCommand
        {
            get
            {
                submitCommand = new CommandViewModel("ذخیره", new DelegateCommand(() =>
                {


                    if (CurrentCharterType == CharterType.In)
                    {
                        SubmitCharterIn();
                    }
                    else
                    {
                        SubmitCharterOut();
                    }

                }));
                return submitCommand;
            }

        }

        private long goodId;
        [CustomValidation(typeof(ValidationDto), "IsComboSelected")]
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
        [CustomValidation(typeof(ValidationDto), "IsComboSelected")]
        public long UnitId
        {
            get { return unitId; }
            set
            {

                this.SetField(p => p.UnitId, ref unitId, value);
            }
        }

        private long selectedVesselId;
        public long SelectedVesselId
        {
            get { return selectedVesselId; }
            set
            {

                this.SetField(p => p.SelectedVesselId, ref selectedVesselId, value);
            }
        }

        private long charterId;
        public long CharterId
        {
            get { return charterId; }
            set
            {

                this.SetField(p => p.CharterId, ref charterId, value);
            }
        }

        private long charterItemId;
        public long CharterItemId
        {
            get { return charterItemId; }
            set
            {

                this.SetField(p => p.CharterItemId, ref charterItemId, value);
            }
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


        private ObservableCollection<TankDto> _tankDtos;
        public ObservableCollection<TankDto> TankDtos
        {
            get
            {
                return _tankDtos;
            }
            set
            {

                this.SetField(vm => vm.TankDtos, ref _tankDtos, value);

            }
        }

        private ObservableCollection<CurrencyDto> _currencyDtos;
        public ObservableCollection<CurrencyDto> CurrencyDtos
        {
            get { return _currencyDtos; }
            set
            {
                this.SetField(p => p.CurrencyDtos, ref _currencyDtos, value);

            }
        }

        #endregion

        #region Ctor

        public CharterItemVM()
        {


        }

        public CharterItemVM(IFuelController fuelController, ICharterInServiceWrapper charterInServiceWrapper, ICharterOutServiceWrapper charterOutServiceWrapper)
        {
            this._fuelController = fuelController;
            this._charterInServiceWrapper = charterInServiceWrapper;
            this._charterOutServiceWrapper = charterOutServiceWrapper;
            Entity = new CharterItemDto();
            Entity.Good = new GoodDto();
            Entity.TankDto = new TankDto();
            GoodDtos = new ObservableCollection<GoodDto>();
            CurrencyDtos = new ObservableCollection<CurrencyDto>();
            TankDtos = new ObservableCollection<TankDto>();
           // SelectedVesselId = 1;


        }

        #endregion


        #region Method
        public void SetCharterType(CharterType charterType, CharterStateTypeEnum charterStateTypeEnum, long charterId, long charterItemId, long selectedVesselId)
        {
            CurrentCharterType = charterType;
            CurrentStateTypeEnum = charterStateTypeEnum;
            CharterId = charterId;
            CharterItemId = charterItemId;
            this.SelectedVesselId = selectedVesselId;
            if (charterType == CharterType.In)
            {
                this.DisplayName = "چارتر In";

            }
            else
            {
                this.DisplayName = "چارتر Out";
            }

        }

        public void Load()
        {
            LoadGeneralItems(() =>
                                 {
                                     if (CurrentCharterType == CharterType.In)
                                     {
                                         _charterInServiceWrapper.GetItemById(
                                             (res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                                                                                                       {
                                                                                                           ShowBusyIndicator
                                                                                                               ("درحال دریافت اطلاعات ...");
                                                                                                           if (exp ==
                                                                                                               null)
                                                                                                           {

                                                                                                               Entity =
                                                                                                                   res;
                                                                                                               GoodId
                                                                                                                   =
                                                                                                                   res
                                                                                                                       .
                                                                                                                       Good
                                                                                                                       .
                                                                                                                       Id;
                                                                                                           }
                                                                                                           else
                                                                                                           {
                                                                                                               _fuelController
                                                                                                                   .
                                                                                                                   HandleException
                                                                                                                   (exp);
                                                                                                           }
                                                                                                           HideBusyIndicator
                                                                                                               ();

                                                                                                       }),
                                             CurrentStateTypeEnum, charterId, charterItemId);
                                     }
                                     else
                                     {
                                         _charterOutServiceWrapper.GetItemById(
                                            (res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                                            {
                                                ShowBusyIndicator
                                                    ("درحال دریافت اطلاعات ...");
                                                if (exp ==
                                                    null)
                                                {

                                                    Entity =
                                                        res;
                                                    GoodId
                                                        =
                                                        res
                                                            .
                                                            Good
                                                            .
                                                            Id;
                                                }
                                                else
                                                {
                                                    _fuelController
                                                        .
                                                        HandleException
                                                        (exp);
                                                }
                                                HideBusyIndicator
                                                    ();

                                            }),
                                            CurrentStateTypeEnum, charterId, charterItemId);
                                     }
                                 });

        }

        public void LoadGeneralItems(Action action)
        {
            ShowBusyIndicator("درحال دریافت اطلاعات ...");
            _charterInServiceWrapper.GetAllGoods((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
            {
                HideBusyIndicator();
                if (exp == null)
                {

                    foreach (var goodDto in res)
                    {
                        GoodDtos.Add(goodDto);
                    }

                    if (action != null)
                    {
                        action.Invoke();
                    }

                }
                else
                {
                    _fuelController
                        .
                        HandleException
                        (exp);
                }


            }), _fuelController.GetCurrentUser().User.CompanyDto.Id);

            //todo bzcomment

            _charterInServiceWrapper.GetByIdVessel((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
            {
                HideBusyIndicator();
                if (exp == null)
                {

                    foreach (var tank in res.TankDtos)
                    {
                        TankDtos.Add(tank);
                    }


                }
                else
                {
                    _fuelController
                        .
                        HandleException
                        (exp);
                }


            }), SelectedVesselId);

            _charterInServiceWrapper.GetAllCurrencies((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
            {
                HideBusyIndicator();
                if (exp == null)
                {
                    CurrencyDtos.Clear();
                    foreach (var cur in res)
                    {
                        CurrencyDtos.Add(cur);
                    }
                }
                else
                {
                    _fuelController.HandleException(exp);
                }


            }));

        }

        void SubmitCharterIn()
        {

            //todo bz comment

            Entity.Good = new GoodDto();
            Entity.Good.Id = GoodId;
            Entity.Good.Unit = new GoodUnitDto();
            Entity.Good.Unit.Id = UnitId;
            Entity.CharterId = CharterId;

            if (!Entity.Validate()) return;

            if (Entity.Id > 0)
            {
                _charterInServiceWrapper.UpdateItem((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {
                    ShowBusyIndicator("درحال بروزرسانی..");
                    if (exp == null)
                    {
                        Entity
                            =
                            res;

                        if (CurrentStateTypeEnum == CharterStateTypeEnum.Start)
                        {
                            _fuelController.Publish<CharterStartItemListChangeArg>(new CharterStartItemListChangeArg());
                        }
                        else
                        {
                            _fuelController.Publish<CharterEndItemListChangeArg>(new CharterEndItemListChangeArg());
                        }

                        _fuelController.Close(this);
                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }
                    HideBusyIndicator();
                }), charterId, charterItemId, CurrentStateTypeEnum, Entity);
            }
            else
            {

                _charterInServiceWrapper.AddItem((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {
                    ShowBusyIndicator("درحال انجام عملیات..");
                    if (exp == null)
                    {
                        Entity
                            =
                            res;
                        if (CurrentStateTypeEnum == CharterStateTypeEnum.Start)
                        {
                            _fuelController.Publish<CharterStartItemListChangeArg>(new CharterStartItemListChangeArg());
                        }
                        else
                        {
                            _fuelController.Publish<CharterEndItemListChangeArg>(new CharterEndItemListChangeArg());
                        }
                        _fuelController.Close(this);
                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }
                    HideBusyIndicator();
                }), CurrentStateTypeEnum, Entity);
            }
        }

        void SubmitCharterOut()
        {

            //todo bz comment

            Entity.Good = new GoodDto();
            Entity.Good.Id = GoodId;
            Entity.Good.Unit = new GoodUnitDto();
            Entity.Good.Unit.Id = UnitId;
            Entity.CharterId = CharterId;

    

            
            if (!Entity.Validate()) return;

            if (Entity.Id > 0)
            {
                _charterOutServiceWrapper.UpdateItem((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {
                    ShowBusyIndicator("درحال بروزرسانی..");
                    if (exp == null)
                    {
                        Entity
                            =
                            res;

                        if (CurrentStateTypeEnum == CharterStateTypeEnum.Start)
                        {
                            _fuelController.Publish<CharterStartItemListChangeArg>(new CharterStartItemListChangeArg());
                        }
                        else
                        {
                            _fuelController.Publish<CharterEndItemListChangeArg>(new CharterEndItemListChangeArg());
                        }

                        _fuelController.Close(this);
                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }
                    HideBusyIndicator();
                }), charterId, charterItemId, CurrentStateTypeEnum, Entity);
            }
            else
            {

                _charterOutServiceWrapper.AddItem((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {
                    ShowBusyIndicator("درحال انجام عملیات..");
                    if (exp == null)
                    {
                        Entity
                            =
                            res;
                        if (CurrentStateTypeEnum == CharterStateTypeEnum.Start)
                        {
                            _fuelController.Publish<CharterStartItemListChangeArg>(new CharterStartItemListChangeArg());
                        }
                        else
                        {
                            _fuelController.Publish<CharterEndItemListChangeArg>(new CharterEndItemListChangeArg());
                        }
                        _fuelController.Close(this);
                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }
                    HideBusyIndicator();
                }), CurrentStateTypeEnum, Entity);
            }
        }

        #endregion

    }
}
