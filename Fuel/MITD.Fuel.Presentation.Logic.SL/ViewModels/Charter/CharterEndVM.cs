using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class CharterEndVM : WorkspaceViewModel, IEventHandler<CharterEndItemListChangeArg>
    {
        #region Prop



        private ICharterController _charterController;
        CharterType CurrentCharterType { get; set; }
        public UploaderVM UploaderVm { get; set; }

        private IFuelController _fuelController;
        private ICharterInServiceWrapper _charterInServiceWrapper;
        private ICharterOutServiceWrapper _charterOutServiceWrapper;
        private ICompanyServiceWrapper _companyServiceWrapper;


        private string companyName;
        public string CompanyName
        {
            get { return companyName; }
            set
            {
                this.SetField(p => p.CompanyName, ref companyName, value);

            }
        }


        private bool viewFlag;
        public bool ViewFlag
        {
            get { return viewFlag; }
            set
            {
                this.SetField(p => p.ViewFlag, ref viewFlag, value);

            }
        }


        private Visibility _viewShamsiFlag;
        public Visibility ViewShamsiFlag
        {
            get { return _viewShamsiFlag; }
            set
            {
                this.SetField(p => p.ViewShamsiFlag, ref _viewShamsiFlag, value);

            }
        }

        private Visibility _viewGerogyFlag;
        public Visibility ViewGerogyFlag
        {
            get { return _viewGerogyFlag; }
            set
            {
                this.SetField(p => p.ViewGerogyFlag, ref _viewGerogyFlag, value);

            }
        }


        private long selectedPickerTypeId;
        public long SelectedPickerTypeId
        {
            get { return selectedPickerTypeId; }
            set
            {
                this.SetField(p => p.SelectedPickerTypeId, ref selectedPickerTypeId, value);
                if (value == 2)
                {
                    ViewShamsiFlag = Visibility.Visible;
                    ViewGerogyFlag = Visibility.Collapsed;
                }
                else if (value == 1)
                {
                    ViewShamsiFlag = Visibility.Collapsed;
                    ViewGerogyFlag = Visibility.Visible;
                }

            }
        }

        private CharterDto _charterDto;
        public CharterDto Entity
        {
            get { return _charterDto; }
            set
            {
                this.SetField(p => p.Entity, ref _charterDto, value);
                DataInventoryOperation.Clear();
                if (Entity.InventoryOperationDtos != null)
                {
                    Entity.InventoryOperationDtos.ToList().ForEach(c => DataInventoryOperation.Add(c));
                }
                ViewFlag = Entity.IsFinalApproveVisiblity;
                if (UploaderVm != null)
                    if (ViewFlag)
                    {
                        UploaderVm.Visible();
                        SetUploaderValue();
                        UploaderVm.Load();

                    }
                    else
                    {
                        UploaderVm.InVisible();
                    }
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

        private ObservableCollection<CharterItemDto> _charterItemDtos;
        public ObservableCollection<CharterItemDto> CharterItemDtos
        {
            get { return _charterItemDtos; }
            set
            {
                this.SetField(p => p.CharterItemDtos, ref _charterItemDtos, value);
            }
        }

        private List<ComboBoxItm> charterTypeEnums;
        public List<ComboBoxItm> CharterTypeEnums
        {
            get { return charterTypeEnums; }
            set
            {
                this.SetField(p => p.CharterTypeEnums, ref charterTypeEnums, value);
            }
        }



        private ObservableCollection<FuelReportInventoryOperationDto> _dataInventoryOperation;
        public ObservableCollection<FuelReportInventoryOperationDto> DataInventoryOperation
        {
            get { return _dataInventoryOperation; }
            set { this.SetField(p => p.DataInventoryOperation, ref _dataInventoryOperation, value); }
        }

        private long selectedCharterEndTypeId;
        public long SelectedCharterEndTypeId
        {
            get { return selectedCharterEndTypeId; }
            set
            {
                this.SetField(p => p.SelectedCharterEndTypeId, ref selectedCharterEndTypeId, value);

            }
        }




        private bool viewEndFlag;
        public bool ViewEndFlag
        {
            get { return viewEndFlag; }
            set
            {
                this.SetField(p => p.ViewEndFlag, ref viewEndFlag, value);

            }
        }



        private long charterId;
        public long CharterId
        {
            get { return charterId; }
            set
            {
                this.SetField(p => p.CharterId, ref charterId, value);
                if (CharterId > 0)
                {
                    ViewEndFlag = true;
                    UploaderVm.Visible();
                    SetUploaderValue();
                    UploaderVm.Load();
                    Load();
                }
                else
                {
                    ViewEndFlag = false;
                    UploaderVm.InVisible();

                }



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
        private List<ComboBoxItm> _calendarType;
        public List<ComboBoxItm> CalendarType
        {
            get { return _calendarType; }
            set
            {
                this.SetField(p => p.CalendarType, ref _calendarType, value);
            }
        }

        private long selectedOwnerId;
        public long SelectedOwnerId
        {
            get { return selectedOwnerId; }
            set
            {
                this.SetField(p => p.SelectedOwnerId, ref selectedOwnerId, value);

            }
        }


        private ObservableCollection<CompanyDto> companyDtos;
        public ObservableCollection<CompanyDto> CompanyDtos
        {
            get { return companyDtos; }
            set
            {
                this.SetField(p => p.CompanyDtos, ref companyDtos, value);
            }
        }


        private CharterItemDto selectedCharterItem;
        public CharterItemDto SelectedCharterItem
        {
            get { return selectedCharterItem; }
            set
            {
                this.SetField(p => p.SelectedCharterItem, ref selectedCharterItem, value);
                //DataInventoryOperation.Clear();
                //if (selectedCharterItem != null && selectedCharterItem.InventoryOperationDtos != null)
                //{
                //    selectedCharterItem.InventoryOperationDtos.ForEach(c => DataInventoryOperation.Add(c));
                //}

            }
        }

        private ObservableCollection<VesselInCompanyDto> _vesselInCompanyDtos;
        public ObservableCollection<VesselInCompanyDto> VesselInCompanyDtos
        {
            get { return _vesselInCompanyDtos; }
            set
            {
                this.SetField(p => p.VesselInCompanyDtos, ref _vesselInCompanyDtos, value);
            }
        }

        #region Command

        private CommandViewModel addCommand;
        public CommandViewModel AddCommand
        {
            get
            {
                addCommand = new CommandViewModel("افزودن", new DelegateCommand(() =>
                {
                    if (CurrentCharterType == CharterType.In)
                    {
                        _charterController.AddCharterInItem(CharterStateTypeEnum.End, Entity.Id, 0,SelectedVesselId);
                    }
                    else
                    {
                        _charterController.AddCharterOutItem(CharterStateTypeEnum.End, Entity.Id, 0, SelectedVesselId);
                    }

                }));
                return addCommand;
            }
        }


        private CommandViewModel editCommand;
        public CommandViewModel EditCommand
        {
            get
            {
                editCommand = new CommandViewModel("ویرایش", new DelegateCommand(() =>
                {

                    if (!checkIsSelected()) return;
                    if (CurrentCharterType == CharterType.In)
                    {
                        _charterController.UpdateCharterInItem(CharterStateTypeEnum.End, Entity.Id, SelectedCharterItem.Id, SelectedVesselId);

                    }
                    else
                    {
                        _charterController.UpdateCharterOutItem(CharterStateTypeEnum.End, Entity.Id, SelectedCharterItem.Id, SelectedVesselId);

                    }

                }));
                return editCommand;
            }

        }


        private CommandViewModel deleteCommand;
        public CommandViewModel DeleteCommand
        {
            get
            {
                deleteCommand = new CommandViewModel("حذف", new DelegateCommand(() =>
                {

                    if (!checkIsSelected()) return;
                    if (_fuelController.ShowConfirmationBox("آیا برای حذف مطمئن هستید ",
                                                                                  "اخطار"))
                    {

                        if (CurrentCharterType == CharterType.In)
                        {
                            _charterInServiceWrapper.DeleteItem((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                            {
                                ShowBusyIndicator("درحال انجام حذف");
                                if (exp == null)
                                {
                                    Load();
                                }
                                else
                                {
                                    _fuelController.HandleException(exp);
                                }
                                HideBusyIndicator();
                            }), CharterStateTypeEnum.End, Entity.Id, SelectedCharterItem.Id);

                        }
                        else
                        {
                            _charterOutServiceWrapper.DeleteItem((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                            {
                                ShowBusyIndicator("درحال انجام حذف");
                                if (exp == null)
                                {
                                    Load();
                                }
                                else
                                {
                                    _fuelController.HandleException(exp);
                                }
                                HideBusyIndicator();
                            }), CharterStateTypeEnum.End, Entity.Id, SelectedCharterItem.Id);

                        }
                    }



                }));
                return deleteCommand;
            }

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

        #endregion

        #endregion




        #region Ctor

        public CharterEndVM()
        {
            this.DataInventoryOperation = new ObservableCollection<FuelReportInventoryOperationDto>();
            this.Entity = new CharterDto();
            this.Entity.VesselInCompany = new VesselInCompanyDto();
            this.Entity.Owner = new CompanyDto();
            this.Entity.Charterer = new CompanyDto();
            this.Entity.EndDate = DateTime.Now.Date;//A.H
            //todo bzcomment
            //Entity.Charterer = _fuelController.GetCurrentUser().CompanyDto;
            this.CurrentCharterType = new CharterType();
            this.CompanyDtos = new ObservableCollection<CompanyDto>();
            this.VesselInCompanyDtos = new ObservableCollection<VesselInCompanyDto>();
            this.CharterItemDtos = new ObservableCollection<CharterItemDto>();
            this.CharterTypeEnums = new List<ComboBoxItm>();
            this.DataInventoryOperation = new ObservableCollection<FuelReportInventoryOperationDto>();

        }

        public CharterEndVM(ICharterController charterController, IFuelController fuelController,
            ICharterInServiceWrapper charterInServiceWrapper, ICharterOutServiceWrapper charterOutServiceWrapper
            , ICompanyServiceWrapper companyServiceWrapper)
        {
            this._charterController = charterController;
            this._fuelController = fuelController;
            this._charterInServiceWrapper = charterInServiceWrapper;
            this._charterOutServiceWrapper = charterOutServiceWrapper;
            this._companyServiceWrapper = companyServiceWrapper;
            this.DataInventoryOperation = new ObservableCollection<FuelReportInventoryOperationDto>();
            this.Entity = new CharterDto();
            this.Entity.VesselInCompany = new VesselInCompanyDto();
            this.Entity.Owner = new CompanyDto();
            this.Entity.Charterer = new CompanyDto();
            this.Entity.EndDate = DateTime.Now.Date;//A.H
            //todo bzcomment
            //Entity.Charterer = _fuelController.GetCurrentUser().CompanyDto;
            this.CurrentCharterType = new CharterType();
            this.CompanyDtos = new ObservableCollection<CompanyDto>();
            this.VesselInCompanyDtos = new ObservableCollection<VesselInCompanyDto>();
            this.CharterItemDtos = new ObservableCollection<CharterItemDto>();
            this.CharterTypeEnums = new List<ComboBoxItm>();
            this.DataInventoryOperation = new ObservableCollection<FuelReportInventoryOperationDto>();
            CalendarType = new List<ComboBoxItm>();

            SelectedPickerTypeId = 1;
        }
        #endregion


        #region Method
        public void SetCharterType(CharterType charterType)
        {
            CurrentCharterType = charterType;
            if (charterType == CharterType.In)
            {
                this.DisplayName = "چارتر In";

            }
            else
            {
                this.DisplayName = "چارتر Out";
            }

        }

        void Load()
        {
            if (CurrentCharterType == CharterType.In)
            {
                CharterInLoad();
            }
            else
            {
                CharterOutLoad();
            }
            LoadGeneralItems();


        }

        public void LoadGeneralItems()
        {
            charterTypeEnums.Clear();
            CalendarType.Clear();
            CalendarType = new List<ComboBoxItm>()
            {
               
                 new ComboBoxItm()
                {
                   Name="تقویم میلادی",
                
                    Id=1
                },
                 new ComboBoxItm()
                {
                        Name="تقویم شمسی",
                    Id=2
                }

            };
            CharterTypeEnums =
                       MITD.Fuel.Presentation.Logic.SL.Infrastructure.EnumHelper.GetItems(typeof(CharterEndTypeEnum));

            if (CurrentCharterType == CharterType.In)
            {
                 charterTypeEnums.RemoveAt(3);
            }
            else
            {
                charterTypeEnums.RemoveAt(2);
            }



            _companyServiceWrapper.GetAll((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
            {
                ShowBusyIndicator("درحال دریافت اطلاعات مالکان");
                if (exp == null)
                {
                    companyDtos.Clear();
                    res.Result.ToList()
                        .ForEach(
                            c =>
                            companyDtos
                                .Add(c));

                    
                    Entity.Charterer = companyDtos.Where(c => c.Id == _fuelController.GetCurrentUser().User.CompanyDto.Id).SingleOrDefault();
                    if (Entity.Charterer != null)
                        CompanyName = Entity.Charterer.Name;
                }
                else
                {
                    _fuelController.HandleException(exp);
                }
                HideBusyIndicator();
            }), "");


           
            if (CurrentCharterType == CharterType.In)
            {
                ShowBusyIndicator("درحال دریافت اطلاعات کشتی ها");
                _charterInServiceWrapper.GetAllIdelVessels((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {

                    if (exp == null)
                    {
                        VesselInCompanyDtos.Clear();
                        res.Result.ToList()
                            .ForEach(
                                c =>
                                VesselInCompanyDtos.
                                    Add(c));

                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }
                    HideBusyIndicator();
                }), _fuelController.GetCurrentUser().User.CompanyDto.Id);
            }
            else
            {
                ShowBusyIndicator("درحال دریافت اطلاعات کشتی ها");
                _charterOutServiceWrapper.GetAllIdelVessels((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {

                    if (exp == null)
                    {
                        VesselInCompanyDtos.Clear();
                        res.Result.ToList()
                            .ForEach(
                                c =>
                                VesselInCompanyDtos.
                                    Add(c));

                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }
                    HideBusyIndicator();
                }), _fuelController.GetCurrentUser().User.CompanyDto.Id);
            }

        }



        void CharterInLoad()
        {
            // todo bz comment
            if (CharterId > 0)
                _charterInServiceWrapper.GetById((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {
                    ShowBusyIndicator("درحال دریافت اطلاعات...");
                    if (exp == null)
                    {
                        if (res != null)
                        {
                            Entity = res;

                            SelectedOwnerId = res.Owner.Id;
                            SelectedVesselId = res.VesselInCompany.Id;
                            SelectedCharterEndTypeId = (int)res.CharterEndType;
                            CompanyName = res.Charterer.Name;
                            CharterItemDtos = res.CharterItems;
                            DataInventoryOperation = res.InventoryOperationDtos;
                            ViewFlag = true;
                        }
                        else
                        {
                            ViewFlag = false;
                            GetCharterInStart();
                        }

                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }

                    HideBusyIndicator();


                }), CharterStateTypeEnum.End, CharterId);

        }

        void GetCharterInStart()
        {
            // todo bz comment
            if (CharterId > 0)
                _charterInServiceWrapper.GetById((res1, exp1) => _fuelController.BeginInvokeOnDispatcher(() =>
                {
                    ShowBusyIndicator("درحال دریافت اطلاعات...");
                    if (exp1 == null)
                    {

                        SelectedVesselId = res1.VesselInCompany.Id;
                        SelectedOwnerId = res1.Owner.Id;
                        CompanyName = res1.Charterer.Name;
                        Entity.Owner = res1.Owner;
                        Entity.VesselInCompany = res1.VesselInCompany;
                        Entity.Currency = res1.Currency;
                    }
                    else
                    {
                        _fuelController.HandleException(exp1);
                    }

                    HideBusyIndicator();


                }), CharterStateTypeEnum.Start, CharterId);
        }

        void GetCharterOutStart()
        {
            // todo bz comment
            if (CharterId > 0)
                _charterOutServiceWrapper.GetById((res1, exp1) => _fuelController.BeginInvokeOnDispatcher(() =>
                {
                    ShowBusyIndicator("درحال دریافت اطلاعات...");
                    if (exp1 == null)
                    {

                        SelectedVesselId = res1.VesselInCompany.Id;
                        SelectedOwnerId = res1.Charterer.Id;
                        CompanyName = res1.Owner.Name;
                        Entity.Owner = res1.Owner;
                        Entity.VesselInCompany = res1.VesselInCompany;
                        Entity.Currency = res1.Currency;
                    }
                    else
                    {
                        _fuelController.HandleException(exp1);
                    }

                    HideBusyIndicator();


                }), CharterStateTypeEnum.Start, CharterId);
        }

        void CharterOutLoad()
        {
            // todo bz comment
            if (CharterId > 0)
                _charterOutServiceWrapper.GetById((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {
                    ShowBusyIndicator("درحال دریافت اطلاعات...");
                    if (exp == null)
                    {
                        if (res != null)
                        {
                            Entity = res;

                            SelectedOwnerId = res.Charterer.Id;
                            SelectedVesselId = res.VesselInCompany.Id;
                            SelectedCharterEndTypeId = (int)res.CharterEndType;
                            
                            CompanyName = res.Owner.Name;
                            CharterItemDtos = res.CharterItems;
                            DataInventoryOperation = res.InventoryOperationDtos;
                            ViewFlag = true;


                        }
                        else
                        {
                            ViewFlag = false;
                            GetCharterOutStart();
                        }

                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }

                    HideBusyIndicator();


                }), CharterStateTypeEnum.End, CharterId);
        }

        private bool checkIsSelected()
        {
            if (SelectedCharterItem == null)
            {
                _fuelController.ShowMessage("لطفا ایتم مورد نظر را انتخاب فرمائید");
                return false;
            }
            else return true;
        }

        public void Handle(CharterEndItemListChangeArg eventData)
        {

            if (CurrentCharterType == CharterType.In)
            {
                CharterInLoad();
            }
            else
            {
                CharterOutLoad();
            }


        }


        void SubmitCharterIn()
        {

            Entity.VesselInCompany.Id = selectedVesselId;
            Entity.Owner.Id = selectedOwnerId;
            Entity.Charterer.Id = _fuelController.GetCurrentUser().User.CompanyDto.Id;
            Entity.CharterEndType = (CharterEndTypeEnum)SelectedCharterEndTypeId;
            Entity.CharterStateType = CharterStateTypeEnum.End;
            if (!Entity.Validate()) return;
            if (Entity.Id > 0)
            {
                _charterInServiceWrapper.Update((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {
                    ShowBusyIndicator("درحال بروزرسانی..");
                    if (exp == null)
                    {
                        Entity
                            =
                            res;

                        _fuelController.Publish<CharterListChangeArg>(new CharterListChangeArg());
                        _fuelController.Close(this);
                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }
                    HideBusyIndicator();
                }), Entity.Id, Entity);
            }
            else
            {

                _charterInServiceWrapper.Add((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {
                    ShowBusyIndicator("درحال انجام عملیات..");
                    if (exp == null)
                    {
                        Entity
                            =
                            res;
                        _fuelController.Publish<CharterListChangeArg>(new CharterListChangeArg());
                        _fuelController.Close(this);
                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }
                    HideBusyIndicator();
                }), Entity);
            }
        }

        void SubmitCharterOut()
        {

            // Todo Must remove bzcomment


            Entity.VesselInCompany.Id = selectedVesselId;
            Entity.Owner.Id = _fuelController.GetCurrentUser().User.CompanyDto.Id;
            Entity.Charterer.Id = selectedOwnerId;
            Entity.CharterEndType = (CharterEndTypeEnum)SelectedCharterEndTypeId;
            Entity.CharterStateType = CharterStateTypeEnum.End;
            if (!Entity.Validate()) return;
            if (Entity.Id > 0)
            {
                _charterOutServiceWrapper.Update((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {
                    ShowBusyIndicator("درحال بروزرسانی..");
                    if (exp == null)
                    {
                        Entity
                            =
                            res;

                        _fuelController.Publish<CharterListChangeArg>(new CharterListChangeArg());
                        _fuelController.Close(this);
                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }
                    HideBusyIndicator();
                }), Entity.Id, Entity);
            }
            else
            {

                _charterOutServiceWrapper.Add((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {
                    ShowBusyIndicator("درحال انجام عملیات..");
                    if (exp == null)
                    {
                        Entity
                            =
                            res;
                        _fuelController.Publish<CharterListChangeArg>(new CharterListChangeArg());
                        _fuelController.Close(this);
                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }
                    HideBusyIndicator();
                }), Entity);
            }
        }


        void SetUploaderValue()
        {
            if (CurrentCharterType == CharterType.In)
                UploaderVm.AttachmentType = AttachmentType.CharterInEnd;
            else
                UploaderVm.AttachmentType = AttachmentType.CharterOutEnd;
            UploaderVm.EntityId = CharterId;
        }

        #endregion



    }
}
