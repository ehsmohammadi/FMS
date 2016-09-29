using MITD.Presentation;
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

namespace MITD.Main.Presentation.UI.SL
{
    public partial class DialogView : ChildWindow, IDialogView
    {
        public DialogView()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public new string Title
        {
            get
            {
                return base.Title.ToString();
            }
            set
            {
                base.Title=value;
            }
        }

        public object DialogContent
        {
            get
            {
                return LayoutRoot.Children[0];
            }
            set
            {
                LayoutRoot.Children.Clear();
                LayoutRoot.Children.Add(value as UIElement);
            }
        }
    }
}

