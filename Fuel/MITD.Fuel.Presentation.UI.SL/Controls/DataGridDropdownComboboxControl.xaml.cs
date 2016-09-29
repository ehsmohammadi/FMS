using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Castle.Core.Internal;
using MITD.Presentation.UI;
using MITD.StorageSpace.Presentation.Contracts.SL.Events;

namespace MITD.Fuel.Presentation.UI.SL.Controls
{
    public partial class DataGridDropdownComboboxControl : UserControl
    {
        public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(DataGridDropdownComboboxControl), new PropertyMetadata((object)string.Empty, OnSelectedValuePathPropertyChanged));
        
        private static void OnSelectedValuePathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //((DataGridDropdownComboboxControl)d).ComboBox.SelectedValuePath = e.NewValue as string;
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(DataGridDropdownComboboxControl), new PropertyMetadata(ItemsSourceChanged));

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //((DataGridDropdownComboboxControl)d).ComboBox.ItemsSource = e.NewValue as IEnumerable;
        }

        public static readonly DependencyProperty SelectedValueProperProperty = DependencyProperty.Register("SelectedValueProper", typeof(object), typeof(DataGridDropdownComboboxControl), new PropertyMetadata(SelectedValueProperPropertyChangedCallback));

        private static void SelectedValueProperPropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs dp)
        {
            //((DataGridDropdownComboboxControl)o).ComboBox.SelectedValueProper = dp.NewValue;
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(DataGridDropdownComboboxControl), new PropertyMetadata(OnSelectedItemChanged));

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //((DataGridDropdownComboboxControl)d).ComboBox.SelectedItem = e.NewValue;
            //((DataGridDropdownComboboxControl) d).ComboBox.SelectedValueProper = ((DataGridDropdownComboboxControl) d).ComboBox.SelectedValue;
            //((DataGridDropdownComboboxControl)d).ComboBox.SelectedValueProper = 
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(DataGridDropdownComboboxControl), new PropertyMetadata(OnItemTemplatePropertyChanged));

        private static void OnItemTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataGridDropdownComboboxControl)d).ComboBox.ItemTemplate = e.NewValue as DataTemplate;
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(ItemTemplateProperty);
            }
            set
            {
                this.SetValue(ItemTemplateProperty, value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        public object SelectedItem
        {
            get
            {
                return this.GetValue(SelectedItemProperty);
            }
            set
            {
                this.SetValue(SelectedItemProperty, value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        public object SelectedValueProper
        {
            get
            {
                return this.GetValue(SelectedValueProperProperty);
            }
            set
            {
                this.SetValue(SelectedValueProperProperty, value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        public string SelectedValuePath
        {
            get
            {
                return (string)this.GetValue(SelectedValuePathProperty);
            }
            set
            {
                this.SetValue(SelectedValuePathProperty, (object)value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        public IEnumerable ItemsSource
        {
            get
            {
                return this.GetValue(ItemsSourceProperty) as IEnumerable;
            }
            set
            {
                this.SetValue(ItemsSourceProperty, (object)value);
            }
        }

        public static readonly DependencyProperty DataGridColumnsProperty = DependencyProperty.Register("DataGridColumns", typeof(ObservableCollection<DataGridColumn>), typeof(DataGridDropdownComboboxControl), new PropertyMetadata(OnDataGridColumnsPropertyChanged));

        private static void OnDataGridColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //((BindableColumnsDataGridEx)d).Columns.Clear();

            //if (e.NewValue == null) return;

            //foreach (var dataGridColumn in e.NewValue as ObservableCollection<DataGridColumn>)
            //{
            //    ((BindableColumnsDataGridEx)d).Columns.Add(dataGridColumn);
            //}
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        public ObservableCollection<DataGridColumn> DataGridColumns
        {
            get { return this.GetValue(DataGridColumnsProperty) as ObservableCollection<DataGridColumn>; }
            set { this.SetValue(DataGridColumnsProperty, value); }
        }

        public DataGridDropdownComboboxControl()
            : base()
        {
            this.DataGridColumns = new ObservableCollection<DataGridColumn>();
            InitializeComponent();

        }

        private void DropDownDataGrid_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ComboBox.IsDropDownOpen = false;
        }

        private void DropDownDataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            var dd = sender as BindableColumnsDataGridEx;
            (sender as BindableColumnsDataGridEx).Columns.ForEach(c => c.HeaderStyle = this.GridColumnHeaderStyle);
        }
    }
}
