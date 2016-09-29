using MITD.Core;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels;
using MITD.Fuel.Presentation.Logic.SL.Controllers.Inventory;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class InventoryTransactionController : BaseController, IInventoryTransactionController
    {
        public InventoryTransactionController(IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(viewManager, eventPublisher, deploymentManagement)
        {

        }

        public void ShowList()
        {
            var view = this.viewManager.ShowInTabControl<ITransactionListView>();
            (view.ViewModel as TransactionListVM).Load();
        }

        public void ShowReference(string referenceType, string referenceNumber, long selectedCompany, bool isPricing = false)
        {
            switch (referenceType)
            {
                case InventoryOperationReferenceTypes.CHARTER_IN_START_ADJUSTMENT_RECEIPT:
                case InventoryOperationReferenceTypes.CHARTER_IN_START_ADJUSTMENT_ISSUE:
                case InventoryOperationReferenceTypes.CHARTER_IN_START_RECEIPT:
                case InventoryOperationReferenceTypes.CHARTER_IN_END_DECREMENTAL_CORRECTION:
                case InventoryOperationReferenceTypes.CHARTER_IN_END_INCREMENTAL_CORRECTION:
                case InventoryOperationReferenceTypes.CHARTER_IN_END_ISSUE:
                    {
                        var charterInListView = this.viewManager.ShowInDialog<ICharterListView>();
                        (charterInListView.ViewModel as CharterListVM).SetCharterType(CharterType.In);
                        (charterInListView.ViewModel as CharterListVM).SelectedId = long.Parse(referenceNumber);
                        (charterInListView.ViewModel as CharterListVM).SelectedCompanyId = selectedCompany;
                        (charterInListView.ViewModel as CharterListVM).Load(0);
                    }

                    break;
                case InventoryOperationReferenceTypes.CHARTER_OUT_START_DECREMENTAL_CORRECTION:
                case InventoryOperationReferenceTypes.CHARTER_OUT_START_INCREMENTAL_CORRECTION:
                case InventoryOperationReferenceTypes.CHARTER_OUT_START_ISSUE:
                case InventoryOperationReferenceTypes.CHARTER_OUT_END_RECEIPT:
                    {
                        var charterOutListView = this.viewManager.ShowInDialog<ICharterListView>();
                        (charterOutListView.ViewModel as CharterListVM).SetCharterType(CharterType.Out);
                        (charterOutListView.ViewModel as CharterListVM).SelectedId = long.Parse(referenceNumber);
                        (charterOutListView.ViewModel as CharterListVM).SelectedCompanyId = selectedCompany;
                        (charterOutListView.ViewModel as CharterListVM).Load(0);
                    }

                    break;

                case InventoryOperationReferenceTypes.EOV_EOM_EOY_FUEL_REPORT_CONSUMPTION:

                    var fuelReportListViewForEOV = this.viewManager.ShowInDialog<IFuelReportListView>();
                    (fuelReportListViewForEOV.ViewModel as FuelReportListVM).LoadByFilter(referenceNumber, null);

                    break;


                case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_TRANSFER:
                case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION:
                case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION:

                    var fuelReportListViewForDetails = this.viewManager.ShowInDialog<IFuelReportListView>();
                    (fuelReportListViewForDetails.ViewModel as FuelReportListVM).LoadByFilter(null, referenceNumber);

                    break;

                case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_RECEIVE:

                    if (!isPricing)
                    {
                        var fuelReportListView = this.viewManager.ShowInDialog<IFuelReportListView>();
                        (fuelReportListView.ViewModel as FuelReportListVM).LoadByFilter(null, referenceNumber);
                    }
                    else
                    {
                        var referenceNumberParts = referenceNumber.Split(',');

                        var fuelReportDetailId = long.Parse(referenceNumberParts[0]);
                        var invoiceItemId = long.Parse(referenceNumberParts[1]);

                        var invoiceListViewForDetails = this.viewManager.ShowInDialog<IInvoiceListView>();
                        (invoiceListViewForDetails.ViewModel as InvoiceListVM).LoadByFilter(null, invoiceItemId.ToString());
                    }

                    break;

                case InventoryOperationReferenceTypes.INVENTORY_ISSUE_ADJUSTMENT:
                case InventoryOperationReferenceTypes.INVENTORY_RECEIPT_ADJUSTMENT:

                    var inventoryListView = this.viewManager.ShowInDialog<ITransactionListView>();
                    (inventoryListView.ViewModel as TransactionListVM).LoadByFilter(long.Parse(referenceNumber));

                    break;
            }
        }

        public void ShowByFilter(string inventoryOperationReferenceCode)
        {
            var view = this.viewManager.ShowInDialog<ITransactionListView>();
            (view.ViewModel as TransactionListVM).LoadByFilter(inventoryOperationReferenceCode);
        }
    }
}
