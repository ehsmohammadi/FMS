using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class ScrapDetailListVM : WorkspaceViewModel
    {
        private readonly IFuelController fuelMainController;
        private readonly IScrapServiceWrapper scrapServiceWrapper;

        public ScrapDetailListVM()
        {

        }

        public ScrapDetailListVM(IFuelController fuelMainController, IScrapServiceWrapper scrapServiceWrapper)
        {
            this.fuelMainController = fuelMainController;
            this.scrapServiceWrapper = scrapServiceWrapper;
        }
    }
}
