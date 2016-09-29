using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.UI.SL.Views.BasicInfo
{
    public partial class CurrencyExchangeListView : ICurrencyExchangeListView
    {
        public CurrencyExchangeListView()
        {
            InitializeComponent();
        }

        public CurrencyExchangeListView(CurrencyExchangeListVM vm)
            : this()
        {
            ViewModel = vm;
        }
    }
}
