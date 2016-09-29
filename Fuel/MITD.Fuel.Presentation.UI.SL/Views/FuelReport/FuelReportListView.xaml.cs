using System.Windows.Controls;
using System.Windows.Input;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels;
using MITD.Presentation.UI;

namespace MITD.Fuel.Presentation.FuelApp.UI.SL.Views
{
    public partial class FuelReportListView : ViewBase, IFuelReportListView
    {
        public FuelReportListView()
        {
            InitializeComponent();
        }

        public FuelReportListView(FuelReportListVM vm)
            : this()
        {

            ViewModel = vm;
            //ViewModel.View = this;
            uxFuelReportDetailListView.ViewModel = vm.FuelReportDetailListVm;
            uxFuelReportDetailListView.ViewModel.View = uxFuelReportDetailListView;
        }


        private void UxFuelReportDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;

            if (dataGrid.SelectedItem != null && dataGrid.Columns.Count > 0)
                dataGrid.ScrollIntoView(dataGrid.SelectedItem, dataGrid.Columns[0]);
        }
    }
}
