using System.Collections.ObjectModel;
using System.Linq;
using Castle.Core.Internal;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class FuelReportDetailListVM : WorkspaceViewModel, IEventHandler<FuelReportDetailListChangeArg>
    {
        #region Prop

        private readonly IFuelController fuelController;
        private readonly IFuelReportDetailController fuelReportDetailController;
        private readonly IInventoryTransactionController inventoryTransactionController;

        private CommandViewModel editCommand;

        private PagedSortableCollectionView<FuelReportDetailVM> fuelReportDetailVms;
        public PagedSortableCollectionView<FuelReportDetailVM> FuelReportDetailVms
        {
            get { return this.fuelReportDetailVms; }
            set
            {
                this.SetField(p => p.FuelReportDetailVms, ref this.fuelReportDetailVms, value);

            }
        }

        public ObservableCollection<FuelReportInventoryOperationVM> AllInventoryOperations
        {
            get
            {

                var dataToDisplay = new ObservableCollection<FuelReportInventoryOperationDto>();

                if (this.FuelReportVMSelected != null)
                    this.FuelReportVMSelected.Entity.InventoryOperationDtos.ForEach(dataToDisplay.Add);

                if (SelectedFuelReportDetailVm == null)
                { 
                    //Load all children InventoryOperations when no child is selected.
                    if (this.FuelReportVMSelected != null)
                        this.FuelReportVMSelected.Entity.FuelReportDetail.SelectMany(frd => frd.InventoryOperationDtos).ForEach(dataToDisplay.Add);
                }
                else
                {
                    //Display InventoryOperations of currently selected fuel report Detail.
                    this.SelectedFuelReportDetailVm.Entity.InventoryOperationDtos.ForEach(dataToDisplay.Add);
                }


                return new ObservableCollection<FuelReportInventoryOperationVM>(dataToDisplay.Select(CreateInventoryOperationItem));
            }
        }

        private FuelReportInventoryOperationVM selectedInventoryOperation;
        public FuelReportInventoryOperationVM SelectedInventoryOperation
        {
            get { return this.selectedInventoryOperation; }
            set { this.SetField(p => p.SelectedInventoryOperation, ref this.selectedInventoryOperation, value); }
        }

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                this.SetField(p => p.IsActive, ref isActive, value);
            }
        }


        private ObservableCollection<FuelReportDetailDto> fuelReportDetailDtos;
        public ObservableCollection<FuelReportDetailDto> FuelReportDetailDtos
        {
            get { return this.fuelReportDetailDtos; }
            set
            {
                this.SetField(p => p.FuelReportDetailDtos, ref this.fuelReportDetailDtos, value);

            }
        }

        private FuelReportDetailVM selectedFuelReportDetailVm;
        public FuelReportDetailVM SelectedFuelReportDetailVm
        {
            get { return selectedFuelReportDetailVm; }
            set
            {
                this.SetField(p => p.SelectedFuelReportDetailVm, ref selectedFuelReportDetailVm, value);
                this.OnPropertyChanged(p => p.AllInventoryOperations);
            }
        }

        private FuelReportVM fuelReportVMSelected;

        public FuelReportVM FuelReportVMSelected
        {
            get { return fuelReportVMSelected; }
            set
            {
                this.SetField(p => p.FuelReportVMSelected, ref fuelReportVMSelected, value);

                RefreshData();
            }
        }



        public CommandViewModel EditCommand
        {
            get
            {

                if (editCommand == null)
                {

                    editCommand = new CommandViewModel("ویرایش", new DelegateCommand(
                                                                            () =>
                                                                            {
                                                                                if (!checkIsSelected()) return;
                                                                                this.fuelReportDetailController.Update(SelectedFuelReportDetailVm.Entity);
                                                                                //this.FuelReportDetailVms.PageIndex++;
                                                                                //this.FuelReportDetailVms.Refresh();
                                                                            }));


                }
                return editCommand;

            }
        }

        #endregion

        #region Ctor

        public FuelReportDetailListVM()
        {

        }
        public FuelReportDetailListVM(IFuelController fuelController, IFuelReportDetailController fuelReportDetailController, 
                                IInventoryTransactionController inventoryTransactionController)
            : this()
        {
            this.fuelController = fuelController;
            this.fuelReportDetailController = fuelReportDetailController;
            this.FuelReportDetailDtos = new ObservableCollection<FuelReportDetailDto>();
            this.inventoryTransactionController = inventoryTransactionController;

            FuelReportDetailVms = new PagedSortableCollectionView<FuelReportDetailVM>();

            this.OnPropertyChanged(p => p.AllInventoryOperations);
        }

        #endregion

        #region Method

        public void RefreshData()
        {
            FuelReportDetailVms.Clear();
            this.OnPropertyChanged(p => p.AllInventoryOperations);

            if (fuelReportVMSelected != null && fuelReportVMSelected.Entity.Id != -1)
            {
                FuelReportDetailVms.SourceCollection = createFuelReportDetail(fuelReportVMSelected.Entity);
                IsActive = fuelReportVMSelected.Entity.EnableCommercialEditing;
            }
        }

        private FuelReportInventoryOperationVM CreateInventoryOperationItem(FuelReportInventoryOperationDto dto)
        {
            var result = new FuelReportInventoryOperationVM(this.fuelController, this.inventoryTransactionController);
            result.SetEntity(dto);
            return result;
        }

        private ObservableCollection<FuelReportDetailVM> createFuelReportDetail(FuelReportDto dto)
        {

            var result = new ObservableCollection<FuelReportDetailVM>();
            if (dto.FuelReportDetail != null && dto.FuelReportDetail.Count > 0)
            {
                var appController = ServiceLocator.Current.GetInstance<IFuelController>();

                var fuelReportServiceWrapper = ServiceLocator.Current.GetInstance<IFuelReportServiceWrapper>();

                dto.FuelReportDetail.ToList().ForEach(c =>
                                                              {

                                                                  var x = new FuelReportDetailVM(appController, fuelReportServiceWrapper);
                                                                  x.Entity = c;
                                                                  result.Add(x);

                                                              });

            }
            return result;
        }

        #endregion


        public void Handle(FuelReportDetailListChangeArg eventData)
        {
            RefreshData();
        }

        private bool checkIsSelected()
        {
            if (SelectedFuelReportDetailVm == null)
            {
                this.fuelController.ShowMessage("لطفا جزئیات گزارش مورد نظر را انتخاب نمایید");
                return false;
            }

            return true;
        }
    }
}
