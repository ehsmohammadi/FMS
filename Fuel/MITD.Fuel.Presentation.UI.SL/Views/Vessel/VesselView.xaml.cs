using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.UI.SL.Views.Vessel
{
    public partial class VesselView : IVesselView
    {
        public VesselView()
        {
            InitializeComponent();
        }

        public VesselView(VesselVM vm)
            : this()
        {
            ViewModel = vm;
        }
    }
}
