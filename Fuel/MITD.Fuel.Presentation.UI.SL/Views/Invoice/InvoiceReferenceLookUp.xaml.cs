using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels.Invoice;
using MITD.Presentation.UI;

namespace MITD.Fuel.Presentation.UI.SL.Views.Invoice
{
    public partial class InvoiceReferenceLookUp : ViewBase, IInvoiceReferenceLookUp
    {
        public InvoiceReferenceLookUp()
        {
            InitializeComponent();
        }

        public InvoiceReferenceLookUp(InvoiceReferenceLookUpVM vm)
            : this()
        {
            ViewModel = vm;
            ViewModel.View = this;
        }
       
       
    }
}
