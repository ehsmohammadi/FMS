using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.UI.SL.Views.Offhire
{
    public partial class OffhireListView : IOffhireListView
    {
        public OffhireListView()
        {
            InitializeComponent();
        }

        public OffhireListView(OffhireListVM vm)
            : this()
        {
            ViewModel = vm;
        }
    }
}
