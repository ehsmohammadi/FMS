using MITD.Presentation.UI;
using MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels;
using MITD.Fuel.Presentation.Contracts.SL.Views;

namespace MITD.Fuel.Presentation.UI.SL.Views
{
    public partial class VoyageListView : ViewBase, IVoyageListView
    {
        public VoyageListView()
        {
            InitializeComponent();
        }

        public VoyageListView(VoyageListVM vm)
            : this()
        {
            this.ViewModel = vm;
        }
    }

}
