using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation.UI;

namespace MITD.Fuel.Presentation.UI.SL.Views.Invoice
{
    public partial class InvoiceView : ViewBase, IInvoiceView
    {
        public InvoiceView()
        {
            InitializeComponent();
        }

        public InvoiceView(InvoiceVM vm)
            : this()
        {
            UploaderView.ViewModel = vm.UploaderVm;
            UploaderView.ViewModel.View = UploaderView;
            
            ViewModel = vm;
            ViewModel.View = this;

            this.buttonBackgroundStoryboard.Begin();
        }
    }
}
