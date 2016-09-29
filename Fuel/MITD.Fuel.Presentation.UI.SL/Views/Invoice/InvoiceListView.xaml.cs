using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation.UI;

namespace MITD.Fuel.Presentation.UI.SL.Views.Invoice
{
    public partial class InvoiceListView : ViewBase, IInvoiceListView
    {


        public InvoiceListView()
        {
            InitializeComponent();
        }
        public InvoiceListView(InvoiceListVM vm, InvoiceItemListVM invoiceItemListVM)
            : this()
        {

            ViewModel = vm;
            ViewModel.View = this;
            uxInvoiceItemList.ViewModel =invoiceItemListVM;
        }

      
    }
}
