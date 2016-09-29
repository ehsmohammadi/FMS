using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation.UI;


namespace MITD.Fuel.Presentation.UI.SL.Views
{
    public partial class FuelBusyIndicatorView : IFuelBusyIndicatorView
    {
        public FuelBusyIndicatorView()
        {
            InitializeComponent();
        }
        public FuelBusyIndicatorView(FuelBusyIndicatorVM vm)
            : this()
        {
            ViewModel = vm;
            //this.DataContext = vm;
            //ViewModel.View = this;
        }
    }
}
