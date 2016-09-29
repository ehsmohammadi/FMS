using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.UI.SL.Views.Offhire
{
    public partial class OffhireManagementSystemListView : IOffhireManagementSystemListView
    {
        public OffhireManagementSystemListView()
        {
            InitializeComponent();
        }

        public OffhireManagementSystemListView(OffhireManagementSystemListVM vm)
            : this()
        {
            ViewModel = vm;
        }
    }
}
