using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.UI.SL.Views.Scrap
{
    public partial class ScrapListView : IScrapListView
    {
        public ScrapListView()
        {
            InitializeComponent();
        }

        public ScrapListView(ScrapListVM vm)
            : this()
        {
            ViewModel = vm;
        }
    }
}
