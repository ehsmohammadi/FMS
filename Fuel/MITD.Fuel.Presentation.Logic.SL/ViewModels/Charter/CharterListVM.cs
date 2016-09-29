using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Castle.Core.Internal;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class CharterListVM : WorkspaceViewModel, IEventHandler<CharterListChangeArg>
    {

        #region Prop
        CharterType CurrentCharterType { get; set; }
        private ICharterInServiceWrapper _charterInServiceWrapper;
        private ICharterOutServiceWrapper _charterOutServiceWrapper;
        private ICompanyServiceWrapper _companyServiceWrapper;
        private readonly IApprovalFlowServiceWrapper _approvalFlowServiceWrapper;
        private ICharterController _charterController;
        private IFuelController _fuelController;


        private long selectedVesselId;
        public long SelectedVesselId
        {
            get { return selectedVesselId; }
            set
            {
                this.SetField(p => p.SelectedVesselId, ref selectedVesselId, value);

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

        private PagedSortableCollectionView<CharterDto> charterDtos;
        public PagedSortableCollectionView<CharterDto> CharterDtos
        {
            get { return charterDtos; }
            set
            {
                this.SetField(p => p.CharterDtos, ref charterDtos, value);
                CharterDtos.Refresh();
            }
        }

        private PagedSortableCollectionView<CharterItemDto> charterItemDtos;
        public PagedSortableCollectionView<CharterItemDto> CharterItemDtos
        {
            get { return charterItemDtos; }
            set
            {
                this.SetField(p => p.CharterItemDtos, ref charterItemDtos, value);
                CharterItemDtos.Refresh();
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

        private CharterDto selectedCharter;
        public CharterDto SelectedCharter
        {
            get { return selectedCharter; }
            set
            {
                this.SetField(p => p.SelectedCharter, ref selectedCharter, value);
                if (SelectedCharter != null)
                    if (SelectedCharter.Id > 0)
                        LoadItem(SelectedCharter.Id);
            }
        }

        private long selectedCompanyId;
        public long SelectedCompanyId
        {
            get { return selectedCompanyId; }
            set { this.SetField(p => p.SelectedCompanyId, ref selectedCompanyId, value); }
        }


        private long? selectedId;
        public long? SelectedId
        {
            get { return selectedId; }
            set { this.SetField(p => p.SelectedId, ref selectedId, value); }
        }


        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get { return _startDate; }
            set { this.SetField(p => p.StartDate, ref _startDate, value); }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get { return _endDate; }
            set { this.SetField(p => p.EndDate, ref _endDate, value); }
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
                        _charterController.AddCharterIn();
                    }
                    else
                    {
                        _charterController.AddCharterOut();
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
                                                                                          if (SelectedCharter.CharterStateType == CharterStateTypeEnum.Start)
                                                                                          {
                                                                                              _charterController.UpdateCharterIn(SelectedCharter.Id);
                                                                                          }
                                                                                          else
                                                                                          {
                                                                                              GetChaterStartId();
                                                                                          }

                                                                                      }
                                                                                      else
                                                                                      {
                                                                                          if (SelectedCharter.CharterStateType == CharterStateTypeEnum.Start)
                                                                                          {
                                                                                              _charterController.UpdateCharterOut(SelectedCharter.Id);
                                                                                          }
                                                                                          else
                                                                                          {
                                                                                              GetChaterStartId();
                                                                                          }
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
                            this._charterInServiceWrapper.Delete((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                            {
                                ShowBusyIndicator("درحال انجام حذف");
                                if (exp == null)
                                {
                                    Load(0);
                                }
                                else
                                {
                                    _fuelController.HandleException(exp);
                                }
                                HideBusyIndicator();

                            }), SelectedCharter.Id);

                        }
                        else
                        {
                            this._charterOutServiceWrapper.Delete((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                            {
                                ShowBusyIndicator("درحال انجام حذف");
                                if (exp == null)
                                {
                                    Load(0);
                                }
                                else
                                {
                                    _fuelController.HandleException(exp);
                                }
                                HideBusyIndicator();

                            }), SelectedCharter.Id);
                        }
                    }



                }));
                return deleteCommand;
            }

        }


        private CommandViewModel approveCommand;
        public CommandViewModel ApproveCommand
        {
            get
            {
                approveCommand = new CommandViewModel("تأیید", new DelegateCommand(() =>
                {


                    if (!checkIsSelected()) return;
                    ShowBusyIndicator("درحال انجام عملیات تأیید");
                    if (MessageBox.Show("آیا مطمئن هستید ؟", "Approve Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        HideBusyIndicator();
                        return;
                    }

                    approveCharter(selectedCharter.Id);
                    //if (CurrentCharterType == CharterType.In)
                    //{
                    //    _charterController.UpdateCharterIn(selectedCharter.Id);
                    //}
                    //else
                    //{
                    //    _charterController.UpdateCharterOut(selectedCharter.Id);
                    //}

                }));
                return approveCommand;
            }

        }


        private CommandViewModel rejectCommand;
        public CommandViewModel RejectCommand
        {
            get
            {
                rejectCommand = new CommandViewModel("لغو تأیید", new DelegateCommand(() =>
                {
                    if (!checkIsSelected()) return;
                    ShowBusyIndicator("درحال انجام عملیات لغو تأیید");
                    if (MessageBox.Show("آیا مطمئن هستید ؟", "Reject Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        HideBusyIndicator();
                        return;
                    }


                    rejectCharter(selectedCharter.Id);

                    //if (CurrentCharterType == CharterType.In)
                    //{
                    //    _charterController.UpdateCharterIn(selectedCharter.Id);
                    //}
                    //else
                    //{
                    //    _charterController.UpdateCharterOut(selectedCharter.Id);
                    //}

                }));
                return rejectCommand;
            }

        }


        private CommandViewModel searchCommand;
        public CommandViewModel SearchCommand
        {
            get
            {
                searchCommand = new CommandViewModel("جستجو", new DelegateCommand(() =>
                                                                                         {
                                                                                             Load(0);

                                                                                         }));
                return searchCommand;
            }
        }

        #endregion


        #endregion


        #region Ctor

        public CharterListVM()
        {


        }

        public CharterListVM(ICharterController charterController, IFuelController fuelController,
            ICharterInServiceWrapper charterInServiceWrapper,
            ICharterOutServiceWrapper charterOutServiceWrapper
            , ICompanyServiceWrapper companyServiceWrapper,
            IApprovalFlowServiceWrapper approvalFlowServiceWrapper)
        {

            this._charterController = charterController;
            this._fuelController = fuelController;
            this._charterInServiceWrapper = charterInServiceWrapper;
            this._charterOutServiceWrapper = charterOutServiceWrapper;
            this._companyServiceWrapper = companyServiceWrapper;
            this._approvalFlowServiceWrapper = approvalFlowServiceWrapper;
            this.charterDtos = new PagedSortableCollectionView<CharterDto>() { PageSize = 10 };
            this.SelectedCharter = new CharterDto();
            this.CompanyDtos = new ObservableCollection<CompanyDto>();
            this.charterItemDtos = new PagedSortableCollectionView<CharterItemDto>();
            this.VesselInCompanyDtos = new ObservableCollection<VesselInCompanyDto>();
            this.charterDtos.OnRefresh += (a, args) => Load(CharterDtos.PageIndex + 1);

        }


        #endregion


        #region Method


        void approveCharter(long id)
        {
            var actionEntity = (CurrentCharterType == CharterType.In)
                                   ? ActionEntityTypeEnum.CharterIn
                                   : ActionEntityTypeEnum.CharterOut;

            this._approvalFlowServiceWrapper.ActApproveFlow(
                       (result, exception) => this._fuelController.BeginInvokeOnDispatcher(
                               () =>
                               {
                                   this.HideBusyIndicator();
                                   if (exception == null)
                                   {
                                       Load(0);
                                   }
                                   else
                                   {
                                       this._fuelController.HandleException(exception);
                                   }
                               }), id, actionEntity);
        }

        void rejectCharter(long id)
        {
            var actionEntity = (CurrentCharterType == CharterType.In)
                                   ? ActionEntityTypeEnum.CharterIn
                                   : ActionEntityTypeEnum.CharterOut;

            this._approvalFlowServiceWrapper.ActRejectFlow(
                       (result, exception) => this._fuelController.BeginInvokeOnDispatcher(
                               () =>
                               {
                                   this.HideBusyIndicator();
                                   if (exception == null)
                                   {
                                       Load(0);
                                   }
                                   else
                                   {
                                       this._fuelController.HandleException(exception);
                                   }
                               }), id, actionEntity);
        }


        public void Load(int pageIndex)
        {
            if (CurrentCharterType == CharterType.In)
            {
                ShowBusyIndicator("درحال دریافت اطلاعات");
                this._charterInServiceWrapper.GetByFilter((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                                                                                                                 {

                                                                                                                     if (exp == null)
                                                                                                                     {
                                                                                                                         res.Result.ForEach(c =>
                                                                                                                         {
                                                                                                                             if (c.CharterStateType == CharterStateTypeEnum.End)
                                                                                                                                 c.StartDate = c.EndDate;

                                                                                                                         });
                                                                                                                         CharterDtos.SourceCollection = res.Result;
                                                                                                                         CharterDtos.TotalItemCount = res.TotalCount;
                                                                                                                         CharterDtos.PageIndex = Math.Max(0, res.CurrentPage - 1);
                                                                                                                         CharterDtos.PageSize = res.PageSize;


                                                                                                                         // res.Result.ToList().ForEach(c => CharterDtos.Add(c));  

                                                                                                                     }
                                                                                                                     else
                                                                                                                     {
                                                                                                                         _fuelController.HandleException(exp);
                                                                                                                     }
                                                                                                                     HideBusyIndicator();

                                                                                                                 }), SelectedCompanyId, (SelectedId == null) ? 0 : SelectedId.Value,
                                                                                                                 (StartDate == new DateTime(1, 1, 1)) ? null : HttpUtil.DateTimeToString(StartDate),
                                                                                                                 (EndDate == new DateTime(1, 1, 1)) ? null : HttpUtil.DateTimeToString(EndDate),
                                                                                                                      pageIndex, CharterDtos.PageSize, SelectedVesselId);

            }
            else
            {
                ShowBusyIndicator("درحال دریافت اطلاعات");
                this._charterOutServiceWrapper.GetByFilter((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {

                    if (exp == null)
                    {

                        res.Result.ForEach(c =>
                        {
                            if (c.CharterStateType == CharterStateTypeEnum.End)
                                c.StartDate = c.EndDate;

                        });

                        CharterDtos.SourceCollection = res.Result;
                        CharterDtos.TotalItemCount = res.TotalCount;
                        CharterDtos.PageIndex = Math.Max(0, res.CurrentPage - 1);
                        CharterDtos.PageSize = res.PageSize;


                        // res.Result.ToList().ForEach(c => CharterDtos.Add(c));  

                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }
                    HideBusyIndicator();

                }), SelectedCompanyId,
                   (SelectedId == null) ? 0 : SelectedId.Value,
                  (StartDate == new DateTime(1, 1, 1)) ? null : HttpUtil.DateTimeToString(StartDate),
                  (EndDate == new DateTime(1, 1, 1)) ? null : HttpUtil.DateTimeToString(EndDate)
                  , pageIndex, CharterDtos.PageSize,SelectedVesselId);


            }
        }


        public void LoadItem(long id)
        {
            if (!checkIsSelected()) return;
            ShowBusyIndicator("درحال دریافت اطلاعات");
            if (CurrentCharterType == CharterType.In)
            {
                this._charterInServiceWrapper.GetItems((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {

                    if (exp == null)
                    {
                        CharterItemDtos.SourceCollection = res.Result;
                        CharterItemDtos.TotalItemCount = res.TotalCount;
                        CharterItemDtos.PageIndex = Math.Max(0, res.CurrentPage - 1);
                        CharterItemDtos.PageSize = 8;
                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }
                    HideBusyIndicator();
                }), CharterStateTypeEnum.Start, SelectedCharter.Id, 0, 0);
            }
            else if (CurrentCharterType == CharterType.Out)
            {
                this._charterOutServiceWrapper.GetItems((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {

                    if (exp == null)
                    {
                        CharterItemDtos.SourceCollection = res.Result;
                        CharterItemDtos.TotalItemCount = res.TotalCount;
                        CharterItemDtos.PageIndex = Math.Max(0, res.CurrentPage - 1);
                        CharterItemDtos.PageSize = 8;
                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }
                    HideBusyIndicator();
                }), CharterStateTypeEnum.Start, SelectedCharter.Id, 0, 0);
            }
        }


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

            LoadCompanies();
        }

        void LoadCompanies()
        {
            ShowBusyIndicator(" درحال دریافت اطلاعات لیست شرکتها");
            this._companyServiceWrapper.GetAll((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
            {

                if (exp == null)
                {
                    CompanyDtos.Clear();
                    res.ForEach(c => CompanyDtos.Add(c));
                }
                else
                {
                    _fuelController.HandleException(exp);
                }

                if (CompanyDtos.Count == 1)
                {
                    VesselInCompanyDtos.Clear();
                    //VesselInCompanyDtos.Add(new VesselInCompanyDto(){Name = "همه",Code="0"});
                    VesselInCompanyDtos.Add(new VesselInCompanyDto() { Name = "همه", Code = "" }); //A.H
                    SelectedCompanyId = CompanyDtos[0].Id;
                    CompanyDtos[0].VesselInCompanies.ForEach(c =>
                    {
                         VesselInCompanyDtos.Add(c);
                    });
                }

                HideBusyIndicator();

            }), true);
        }

        void GetChaterStartId()
        {
            ShowBusyIndicator("درحال دریافت اطلاعات");

            if (CurrentCharterType == CharterType.In)
            {
                this._charterInServiceWrapper.GetCharterStart((res, exp) => this._fuelController.BeginInvokeOnDispatcher(() =>
                {
                    this.HideBusyIndicator();
                    if (exp == null)
                    {

                        this._charterController.UpdateCharterIn(res.Id);
                    }
                    else
                    {
                        this._fuelController.HandleException(exp);
                    }



                }), this.SelectedCharter.VesselInCompany.Id, this.selectedCharter.Charterer.Id, SelectedCharter.Id);
            }
            else
            {
                this._charterOutServiceWrapper.GetCharterStart((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                {
                    HideBusyIndicator();
                    if (exp == null)
                    {

                        _charterController.UpdateCharterOut(res.Id);
                    }
                    else
                    {
                        _fuelController.HandleException(exp);
                    }



                }), SelectedCharter.VesselInCompany.Id, selectedCharter.Owner.Id, selectedCharter.Id);
            }
        }

        private bool checkIsSelected()
        {
            if (SelectedCharter == null)
            {
                _fuelController.ShowMessage("لطفا آیتم مورد نظر را انتخاب فرمائید");
                return false;
            }
            else return true;
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            _fuelController.Close(this);
        }

        public void Handle(CharterListChangeArg eventData)
        {

            Load(0);
        }

        #endregion




    }
}
