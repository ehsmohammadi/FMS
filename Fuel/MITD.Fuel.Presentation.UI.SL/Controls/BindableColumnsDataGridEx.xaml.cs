using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using MITD.Presentation.UI;

namespace MITD.Fuel.Presentation.UI.SL.Controls
{
    public partial class BindableColumnsDataGridEx : DataGridEx
    {
        public static readonly DependencyProperty DataGridColumnsProperty = DependencyProperty.Register("DataGridColumns", typeof(ObservableCollection<DataGridColumn>), typeof(BindableColumnsDataGridEx), new PropertyMetadata(OnDataGridColumnsPropertyChanged));

        private static void OnDataGridColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = ((BindableColumnsDataGridEx) d);

            control.Columns.Clear();

            if (e.NewValue == null) return;

            foreach (var dataGridColumn in e.NewValue as ObservableCollection<DataGridColumn>)
            {
                control.Columns.Add(dataGridColumn);
            }
        }

        public ObservableCollection<DataGridColumn> DataGridColumns
        {
            get { return this.GetValue(DataGridColumnsProperty) as ObservableCollection<DataGridColumn>; }
            set { this.SetValue(DataGridColumnsProperty, value); }
        }

        public BindableColumnsDataGridEx()
            : base()
        {
            this.DataGridColumns = new ObservableCollection<DataGridColumn>();
            InitializeComponent();
        }
    }
}
