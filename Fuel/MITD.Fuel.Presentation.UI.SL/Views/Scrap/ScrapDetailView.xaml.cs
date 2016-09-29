using System.Windows.Controls;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.UI.SL.Views.Scrap
{
    public partial class ScrapDetailView : IScrapDetailView
    {
        public ScrapDetailView()
        {
            InitializeComponent();
        }

        public ScrapDetailView(ScrapDetailVM vm)
            : this()
        {
            ViewModel = vm;
        }
    }
}
