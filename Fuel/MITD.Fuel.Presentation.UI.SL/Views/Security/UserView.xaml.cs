using MITD.Fuel.Presentation.Contracts.SL.Views.Security;
using MITD.Fuel.Presentation.Logic.SL.ViewModels.Security;
using MITD.Presentation.UI;
using MITD.Fuel.Presentation.Logic;
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

namespace MITD.Fuel.Presentation.UI.SL.Views.Security
{
    public partial class UserView : ViewBase, IUserView
    {
        public UserView()
        {
            InitializeComponent();
        }

        public UserView(UserVM vm)
        {
            InitializeComponent();
            ViewModel = vm;

        }
    }
}
