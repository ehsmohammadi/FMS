using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class VesselActivationItemVM : WorkspaceViewModel
    {
        #region Prop
        
        private IFuelController _fuelController;
        private ICharterInServiceWrapper _charterInServiceWrapper;

        private readonly IGoodServiceWrapper goodServiceWrapper;
        private readonly ICurrencyServiceWrapper currencyServiceWrapper;
        private IVesselServiceWrapper vesselServiceWrapper;
        private IVesselInCompanyServiceWrapper vesselInCompanyServiceWrapper;

        private VesselActivationItemDto entity;
        public VesselActivationItemDto Entity
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
                return new CommandViewModel("ذخیره", new DelegateCommand(() =>
                {
                    submit();
                }));
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

        private long vesselId;
        public long VesselId
        {
            get { return vesselId; }
            set
            {

                this.SetField(p => p.VesselId, ref vesselId, value);
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

        private Action<VesselActivationItemDto> vesselActivationItemAdded;

        #endregion

        #region Ctor

        public VesselActivationItemVM(IFuelController fuelController, ICharterInServiceWrapper charterInServiceWrapper, IGoodServiceWrapper goodServiceWrapper, ICurrencyServiceWrapper currencyServiceWrapper, IVesselServiceWrapper vesselServiceWrapper, IVesselInCompanyServiceWrapper vesselInCompanyServiceWrapper)
        {
            this._fuelController = fuelController;
            this._charterInServiceWrapper = charterInServiceWrapper;
            this.goodServiceWrapper = goodServiceWrapper;
            this.currencyServiceWrapper = currencyServiceWrapper;
            this.vesselServiceWrapper = vesselServiceWrapper;
            this.vesselInCompanyServiceWrapper = vesselInCompanyServiceWrapper;
            Entity = new VesselActivationItemDto();
            Entity.Good = new GoodDto();
            Entity.TankDto = new TankDto();
            GoodDtos = new ObservableCollection<GoodDto>();
            CurrencyDtos = new ObservableCollection<CurrencyDto>();
            TankDtos = new ObservableCollection<TankDto>();
        }

        #endregion
        
        #region Method

        public void Load(long companyId, long vesselId, string vesselCode, Action<VesselActivationItemDto> vesselActivationItemAdded)
        {
            this.VesselId = vesselId;
            this.vesselActivationItemAdded = vesselActivationItemAdded;

            ShowBusyIndicator("درحال دریافت اطلاعات ...");



            goodServiceWrapper.GetAll((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
            {
                HideBusyIndicator();
                if (exp == null)
                {

                    foreach (var goodDto in res)
                    {
                        GoodDtos.Add(goodDto);
                    }
                }
                else
                {
                    _fuelController
                        .
                        HandleException
                        (exp);
                }


            }), string.Empty, companyId);

            vesselInCompanyServiceWrapper.GetPagedDataByFilter((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
            {
                HideBusyIndicator();
                if (exp == null)
                {
                    if (res != null && res.Result.Count == 1)
                    {
                        res.Result.First().TankDtos.ForEach(TankDtos.Add);
                    }
                }
                else
                {
                    _fuelController.HandleException(exp);
                }


            }), companyId, vesselCode, true, null,null);

            currencyServiceWrapper.GetAllCurrency((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
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


        private void submit()
        {
            if (vesselActivationItemAdded != null)
                vesselActivationItemAdded(Entity);

            _fuelController.Close(this);
        }

        #endregion

    }
}
