using System;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels
{
    public class FuelReportInventoryOperationVM : WorkspaceViewModel
    {
        #region properties

        private IFuelController mainController;
        private readonly IInventoryTransactionController inventoryTransactionController;

        private FuelReportInventoryOperationDto _entity;
        public FuelReportInventoryOperationDto Entity
        {
            get { return _entity; }
            private  set
            {
                this.SetField(p => p.Entity, ref _entity, value);
            }
        }

        private CommandViewModel viewInventoryOperationCommand;

        public CommandViewModel ViewInventoryOperationCommand
        {
            get { return this.viewInventoryOperationCommand; }
            set { this.SetField(p => p.ViewInventoryOperationCommand, ref this.viewInventoryOperationCommand, value); }
        }

        #endregion

        #region ctor

        public FuelReportInventoryOperationVM()
        {
            Entity = new FuelReportInventoryOperationDto() { Id = -1 };

            this.ViewInventoryOperationCommand = new CommandViewModel("...", new DelegateCommand(() =>
            {
                inventoryTransactionController.ShowByFilter(this.Entity.Code);
            }));
        }

        public FuelReportInventoryOperationVM(IFuelController mainController, IInventoryTransactionController inventoryTransactionController)
            : this()
        {
            this.mainController = mainController;
            this.inventoryTransactionController = inventoryTransactionController;

            this.RequestClose += FuelReportVM_RequestClose;
        }

        #endregion

        #region methods

        void FuelReportVM_RequestClose(object sender, EventArgs e)
        {
            this.mainController.Close(this);
        }

        public void SetEntity(FuelReportInventoryOperationDto entity)
        {
            this.Entity = entity;
        }

        #endregion



    }
}
