using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.UI.SL.Views.Vessel
{
    public partial class VesselActivationView : IVesselActivationView
    {
        public VesselActivationView()
        {
            InitializeComponent();
        }

        public VesselActivationView(VesselActivationVM vm)
            : this()
        {
            ViewModel = vm;
        }
    }
}
