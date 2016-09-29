using System.Windows.Controls;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.UI.SL.Views.Offhire
{
    public partial class OffhireDetailView : IOffhireDetailView
    {
        public OffhireDetailView()
        {
            InitializeComponent();
        }

        public OffhireDetailView(OffhireDetailVM vm)
            : this()
        {
            ViewModel = vm;
        }
    }
}
