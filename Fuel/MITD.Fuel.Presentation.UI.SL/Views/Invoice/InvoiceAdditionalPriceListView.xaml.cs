using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Fuel.Presentation.Logic.SL.ViewModels.Invoice;

namespace MITD.Fuel.Presentation.UI.SL.Views.Invoice
{
    public partial class InvoiceAdditionalPriceListView : IInvoiceAdditionalPriceListView
    {
        public InvoiceAdditionalPriceListView()
        {
            InitializeComponent();
        }
        public InvoiceAdditionalPriceListView(InvoiceAdditionalPriceListVM vm)
            : this()
        {
            ViewModel = vm;
            ViewModel.View = this;
        }
    }
}