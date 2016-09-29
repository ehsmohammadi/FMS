using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation.UI;

namespace MITD.Fuel.Presentation.UI.SL.Views.Invoice
{

    public partial class InvoiceItemView : ViewBase, IInvoiceItemView
    {
        public InvoiceItemView()
        {
            InitializeComponent();
        }

        public InvoiceItemView(InvoiceItemVM vm)
            :this()
        {
            ViewModel = vm;
            ViewModel.View = this;
        }
    }
}
