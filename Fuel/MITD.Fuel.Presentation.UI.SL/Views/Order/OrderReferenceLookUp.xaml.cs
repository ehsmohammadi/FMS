using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation.UI;

namespace MITD.Fuel.Presentation.UI.SL.Views
{
    public partial class OrderReferenceLookUp : IOrderReferenceLookUp
    {
        public OrderReferenceLookUp()
        {
            InitializeComponent();
        }

        public OrderReferenceLookUp(OrderReferenceLookUpVM vm)
            : this()
        {
            ViewModel = vm;
            ViewModel.View = this;
        }
       
       
    }
}
