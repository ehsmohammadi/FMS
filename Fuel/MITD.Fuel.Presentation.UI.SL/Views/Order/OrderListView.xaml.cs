using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation.UI;

namespace MITD.Fuel.Presentation.UI.SL.Views
{
    public partial class OrderListView : ViewBase, IOrderListView
    {


        public OrderListView()
        {
            InitializeComponent();
        }
        public OrderListView(OrderListVM vm)
            : this()
        {

            ViewModel = vm;
            ViewModel.View = this;
            uxOrderItemListView.ViewModel = vm.OrderItemListVM;
            uxOrderItemListView.ViewModel.View = uxOrderItemListView;

        }

    }
}
