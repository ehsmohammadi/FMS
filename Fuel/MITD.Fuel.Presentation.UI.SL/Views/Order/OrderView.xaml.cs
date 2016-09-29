using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation.UI;

namespace MITD.Fuel.Presentation.UI.SL.Views
{
    public partial class OrderView : IOrderView
    {
        public OrderView()
        {
            InitializeComponent();
        }

        public OrderView(OrderVM vm):this()
        {
            UploaderView.ViewModel = vm.UploaderVm;
            UploaderView.ViewModel.View = UploaderView;
            
            this.DataContext = vm;
            ViewModel.View = this;

        }
       
       
    }
}
