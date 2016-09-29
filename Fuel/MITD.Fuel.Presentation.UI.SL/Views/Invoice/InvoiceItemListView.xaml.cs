using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation.UI;

namespace MITD.Fuel.Presentation.UI.SL.Views.Invoice
{
    public partial class InvoiceItemListView : ViewBase, IInvoiceItemListView
    {
        public InvoiceItemListView()
        {
            InitializeComponent();
        }

       

        public InvoiceItemListView(InvoiceItemListVM vm)
            :this()
        {
           
            ViewModel = vm;
            ViewModel.View = this;
        }
    }
}
