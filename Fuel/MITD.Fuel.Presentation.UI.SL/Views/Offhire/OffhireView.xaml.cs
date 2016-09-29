using System.Windows;
using System.Windows.Controls;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.UI.SL.Views.Offhire
{
    public partial class OffhireView : IOffhireView
    {
        public OffhireView()
        {
            InitializeComponent();
        }

        public OffhireView(OffhireVM vm)
            : this()
        {
            ViewModel = vm;
        }

        //public static void SetIsReadOnly(DependencyObject obj, bool isReadOnly)
        //{
        //    obj.SetValue(IsReadOnlyProperty, isReadOnly);
        //}

        //public static bool GetIsReadOnly(DependencyObject obj)
        //{
        //    return (bool)obj.GetValue(IsReadOnlyProperty);
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty IsReadOnlyProperty =
        //    DependencyProperty.RegisterAttached("IsReadOnly", typeof(bool), typeof(OffhireView), new PropertyMetadata(false, Callback));

        //private static void Callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((DataGridTextColumn)d).IsReadOnly = (bool)e.NewValue;
        //}
    }
}
