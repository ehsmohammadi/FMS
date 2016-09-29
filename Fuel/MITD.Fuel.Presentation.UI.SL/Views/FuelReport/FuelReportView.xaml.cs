using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.FuelApp.UI.SL.Views
{
    public partial class FuelReportView : IFuelReportView
    {
        public FuelReportView()
        {
            InitializeComponent();
        }

        public FuelReportView(FuelReportVM vm)
            : this()
        {
            this.ViewModel = vm;
        }
    }
}
