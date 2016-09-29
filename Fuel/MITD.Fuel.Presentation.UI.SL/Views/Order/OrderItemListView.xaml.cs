using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation.UI;

namespace MITD.Fuel.Presentation.UI.SL.Views
{
    public partial class OrderItemListView : ViewBase, IOrderItemListView
    {
        public OrderItemListView()
        {
            InitializeComponent();
        }

       

        public OrderItemListView(OrderItemListVM vm):this()
        {
           
            ViewModel = vm;
            ViewModel.View = this;
        }
    }
}
