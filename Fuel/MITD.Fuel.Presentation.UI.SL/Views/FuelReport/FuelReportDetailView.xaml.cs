using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Presentation.UI;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
namespace MITD.Fuel.Presentation.UI.SL.Views
{
    public partial class FuelReportDetailView :ViewBase, IFuelReportDetailView
    {
        public FuelReportDetailView()
        {
            InitializeComponent();
          
        }

        public FuelReportDetailView(FuelReportDetailVM vm)
            : this()
        {
            this.ViewModel = vm;
            ViewModel.View = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
        }

       
        
    }
}
