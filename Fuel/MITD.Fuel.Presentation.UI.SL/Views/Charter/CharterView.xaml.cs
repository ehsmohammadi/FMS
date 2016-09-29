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
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation.UI;

namespace MITD.Fuel.Presentation.UI.SL.Views.CharterIn
{
    public partial class CharterView : ViewBase, ICharterView
    {
        public CharterView()
        {
            InitializeComponent();
        }

        public CharterView(CharterVM vm):this()
        {
            this.ViewModel = vm;
            vm.View = this;
            

            uxChaterStartView.ViewModel = vm.CharterStartVm;
            uxChaterStartView.ViewModel.View = uxChaterStartView;

            uxChaterStartView.UploaderView.ViewModel = vm.CharterStartVm.UploaderVm;
            uxChaterStartView.UploaderView.ViewModel.View = uxChaterStartView.UploaderView;

            uxChaterEndView.ViewModel = vm.CharterEndVm;
            uxChaterEndView.ViewModel.View = uxChaterEndView;

            uxChaterEndView.UploaderView.ViewModel = vm.CharterEndVm.UploaderVm;
            uxChaterEndView.UploaderView.ViewModel.View = uxChaterEndView.UploaderView;

        }

    }
}
