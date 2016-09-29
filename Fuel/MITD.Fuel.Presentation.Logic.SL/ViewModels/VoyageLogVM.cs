using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels
{
    public class VoyageLogVM : WorkspaceViewModel, IEventHandler<VoyageLogChangeArg>
    {
        #region props

        private IFuelController fuelMainController;

        private IVoyageServiceWrapper voyageServiceWrapper;

        private PagedSortableCollectionView<VoyageLogDto> pagedData;
        public PagedSortableCollectionView<VoyageLogDto> PagedData
        {
            get { return this.pagedData; }
            set { this.SetField(p => p.PagedData, ref this.pagedData, value); }
        }

        private VoyageDto selectedVoyage;
        public VoyageDto SelectedVoyage
        {
            get { return this.selectedVoyage; }
            set
            {
                if (this.selectedVoyage != value) this.clearList();
                this.SetField(p => p.SelectedVoyage, ref this.selectedVoyage, value);

            }
        }

        private VoyageLogDto selectedVoyageLog;
        public VoyageLogDto SelectedVoyageLog
        {
            get { return this.selectedVoyageLog; }
            set
            {
                this.SetField(p => p.SelectedVoyageLog, ref this.selectedVoyageLog, value);

            }
        }

        #endregion

        #region ctor

        public VoyageLogVM()
        {
        }

        public VoyageLogVM(
            IFuelController fuelMainController,
            IVoyageServiceWrapper voyageServiceWrapper)
            : this()
        {
            this.Init(
                fuelMainController,
                voyageServiceWrapper);
        }

        #endregion

        #region methods

        private void Init(
            IFuelController fuelMainController,
            IVoyageServiceWrapper voyageServiceWrapper)
        {
            this.voyageServiceWrapper = voyageServiceWrapper;
            this.fuelMainController = fuelMainController;

            this.DisplayName = "تاریخچه تغییرات سفرها";
            this.PagedData = new PagedSortableCollectionView<VoyageLogDto>()
                             {
                                 PageIndex = 0,
                                 PageSize = 5
                             };

            this.PagedData.PageChanged += PagedData_PageChanged;
        }



        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            this.fuelMainController.Close(this);
        }

        #region loading data

        void PagedData_PageChanged(object sender, EventArgs e)
        {
            ShowLog();
        }

        public void ShowLog()
        {
            this.ShowBusyIndicator("درحال دریافت اطلاعات ....");

            if (this.SelectedVoyage == null)
            {
                this.HideBusyIndicator();

                this.fuelMainController.ShowMessage("از لیست سفرها یک ردیف انتخاب گردد.");

                return;
            }

            this.voyageServiceWrapper.GetChenageHistory(this.getChenageHistoryCallback,
                this.SelectedVoyage.Id, this.PagedData.PageSize, this.PagedData.PageIndex);
        }

        private void getChenageHistoryCallback(PageResultDto<VoyageLogDto> pageResult, Exception caughtException)
        {
            this.fuelMainController.BeginInvokeOnDispatcher(() =>
                {
                    this.HideBusyIndicator();
                    if (caughtException == null)
                    {
                        this.PagedData.SourceCollection = pageResult.Result;
                        this.PagedData.ItemCount = pageResult.TotalCount;
                    }
                    else
                    {
                        this.fuelMainController.HandleException(caughtException);
                    }
                });
        }

        private void clearList()
        {
            this.PagedData.Clear();
        }

        #endregion

        #endregion

        public void Handle(VoyageLogChangeArg eventData)
        {
            ShowLog();
        }
    }
}
