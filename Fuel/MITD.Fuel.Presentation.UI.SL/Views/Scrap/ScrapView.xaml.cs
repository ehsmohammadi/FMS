using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.UI.SL.Views.Scrap
{
    public partial class ScrapView : IScrapView
    {
        public ScrapView()
        {
            InitializeComponent();
        }

        public ScrapView(ScrapVM vm)
            : this()
        {
            UploaderView.ViewModel = vm.UploaderVm;
            UploaderView.ViewModel.View = UploaderView;
            ViewModel = vm;
        }
    }
}
