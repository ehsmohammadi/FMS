using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.UI.SL.Views.Inventory
{
    public partial class TransactionListView : ITransactionListView
    {
        public TransactionListView()
        {
            InitializeComponent();
        }

        public TransactionListView(TransactionListVM vm)
            : this()
        {
            ViewModel = vm;
        }
    }
}
