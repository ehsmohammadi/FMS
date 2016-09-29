using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Fuel.Presentation.Logic.SL.ViewModels.Invoice;
using MITD.Presentation.UI;

namespace MITD.Fuel.Presentation.UI.SL.Views.Invoice
{
    public partial class InvoiceAdditionalPriceView : ViewBase, IInvoiceAdditionalPriceView
    {
        public InvoiceAdditionalPriceView()
        {
            InitializeComponent();
        }

        public InvoiceAdditionalPriceView(InvoiceAdditionalPriceVM vm)
            : this()
        {
            ViewModel = vm;
            ViewModel.View = this;

        }
       
       
    }
}
