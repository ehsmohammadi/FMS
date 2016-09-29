using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.UI.SL.Views.Vessel
{
    public partial class VesselListView : IVesselListView
    {
        public VesselListView()
        {
            InitializeComponent();
        }

        public VesselListView(VesselListVM vm)
            : this()
        {
            ViewModel = vm;
        }
    }
}
