using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Presentation;
using MITD.Presentation.UI;
using MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Contracts.DTOs;
using System.Collections.ObjectModel;

namespace MITD.Fuel.Presentation.UI.SL.Views
{
    public partial class VoyageLogView : ViewBase, IVoyageLogView
    {
        public VoyageLogView()
        {
            InitializeComponent();
        }

        public VoyageLogView(VoyageLogVM vm)
            : this()
        {
            this.ViewModel = vm;
        }
    }

}
