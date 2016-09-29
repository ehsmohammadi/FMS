using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MITD.Fuel.Presentation.UI.SL.Controls
{
    public partial class MultiTypeDateTimeControl : UserControl
    {
        public static readonly DependencyProperty DisplayOrientationProperty = DependencyProperty.Register("DisplayOrientation", typeof(Orientation), typeof(MultiTypeDateTimeControl),
                                                                                            new PropertyMetadata(Orientation.Horizontal, DisplayOrientationPropertyChangedCallback));

        public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register("SelectedDateTime", typeof(DateTime?), typeof(MultiTypeDateTimeControl),
                                                                                            new PropertyMetadata((DateTime?)null, SelectedDateTimePropertyChangedCallback));

        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register("DisplayMode", typeof(DisplayMode), typeof(MultiTypeDateTimeControl),
                                                                                            new PropertyMetadata(DisplayMode.Date, DisplayModePropertyChangedCallback));

        public static readonly DependencyProperty CalendarTypeProperty = DependencyProperty.Register("CalendarType", typeof(CalendarType), typeof(MultiTypeDateTimeControl),
                                                                                            new PropertyMetadata(CalendarType.Persian, CalendarTypePropertyChangedCallback));

        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(MultiTypeDateTimeControl),
                                                                                            new PropertyMetadata((DateTime?)null, SelectedDatePropertyChangedCallback));

        public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register("SelectedTime", typeof(DateTime?), typeof(MultiTypeDateTimeControl),
                                                                                            new PropertyMetadata((DateTime?)null, SelectedTimePropertyChangedCallback));

        public static readonly DependencyProperty IsReadonlyProperty = DependencyProperty.Register("IsReadonly", typeof(bool), typeof(MultiTypeDateTimeControl),
                                                                                            new PropertyMetadata(false, IsReadonlyPropertyChangedCallback));

        private static void DisplayOrientationPropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            #region Commented
            //var sender = (MultiTypeDateTimeControl)o;
            //sender.MainStackPanel.Orientation = (Orientation)args.NewValue;

            //switch (((MultiTypeDateTimeControl)o).MainStackPanel.Orientation)
            //{
            //    case Orientation.Vertical:
            //        sender.DatePickerGrid.Margin = new Thickness(0, 0, 0, 3);
            //        break;
            //    case Orientation.Horizontal:
            //        sender.DatePickerGrid.Margin = new Thickness(0, 0, 3, 0);
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException("DisplayOrientation");
            //} 
            #endregion
        }

        private static void SelectedDateTimePropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var sender = (MultiTypeDateTimeControl)o;

            manageSelectedDateTimeSetValue(sender);
        }

        private static void manageSelectedDateTimeSetValue(MultiTypeDateTimeControl sender)
        {
            if (sender.SelectedDateTime.HasValue)
            {
                switch (sender.DisplayMode)
                {
                    case DisplayMode.Date:
                        sender.SelectedDate = sender.SelectedDateTime.Value.Date;
                        sender.SelectedTime = null;
                        break;
                    case DisplayMode.Time:
                        sender.SelectedDate = null;
                        sender.SelectedTime = sender.SelectedDateTime.Value;
                        break;
                    case DisplayMode.DateTime:
                        sender.SelectedDate = sender.SelectedDateTime.Value.Date;
                        sender.SelectedTime = sender.SelectedDateTime.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                sender.SelectedDate = null;
                sender.SelectedTime = null;
            }
        }

        private static void SelectedTimePropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var sender = (MultiTypeDateTimeControl)o;

            manageSelectedTimeSetValue(sender);
        }

        private static void IsReadonlyPropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var sender = (MultiTypeDateTimeControl)o;
        }

        private static void manageSelectedTimeSetValue(MultiTypeDateTimeControl sender)
        {
            if (sender.SelectedTime.HasValue)
            {
                switch (sender.DisplayMode)
                {
                    case DisplayMode.Date:
                        //sender.DisplayDateTime = null;
                        break;
                    case DisplayMode.Time:
                        sender.SelectedDateTime = sender.SelectedTime.Value;
                        break;
                    case DisplayMode.DateTime:
                        sender.SelectedDateTime = sender.SelectedDate.HasValue ? (DateTime?)sender.SelectedDate.Value.Date.Add(sender.SelectedTime.Value.TimeOfDay) : null;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                if (sender.DisplayMode == DisplayMode.Time)
                    sender.SelectedDateTime = null;
                else
                    sender.SelectedDateTime = sender.SelectedDate.HasValue ? (DateTime?)sender.SelectedDate.Value.Date : null;
            }
        }

        private static void SelectedDatePropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var sender = (MultiTypeDateTimeControl)o;

            manageSelectedDateSetValue(sender);
        }

        private static void manageSelectedDateSetValue(MultiTypeDateTimeControl sender)
        {
            if (sender.SelectedDate.HasValue)
            {
                switch (sender.DisplayMode)
                {
                    case DisplayMode.Date:
                        sender.SelectedDateTime = sender.SelectedDate.Value.Date;
                        break;
                    case DisplayMode.Time:
                        //sender.DisplayDateTime = sender.SelectedTime.HasValue ? ;
                        break;
                    case DisplayMode.DateTime:
                        sender.SelectedDateTime = sender.SelectedTime.HasValue ? sender.SelectedDate.Value.Date.Add(sender.SelectedTime.Value.TimeOfDay) : sender.SelectedDate.Value.Date;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                if (sender.DisplayMode != DisplayMode.Time)
                    sender.SelectedDateTime = null; //The value of Date is mandatory in case of Date and DateTime modes.
            }
        }

        private static void DisplayModePropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var sender = (MultiTypeDateTimeControl)o;
            //var selectedDisplayMode = (DisplayMode)args.NewValue;

            setControlsVisibility(sender);
        }

        private static void setCalendarControlsVisibility(MultiTypeDateTimeControl sender, Visibility visible)
        {
            sender.DatePickerGrid.Visibility = Visibility.Collapsed;
            sender.PersianDatePicker.Visibility = Visibility.Collapsed;
            sender.GregorianDatePicker.Visibility = Visibility.Collapsed;

            if (visible == Visibility.Visible)
            {
                switch (sender.CalendarType)
                {
                    case CalendarType.Gregorian:
                        sender.DatePickerGrid.Visibility = Visibility.Visible;
                        sender.GregorianDatePicker.Visibility = Visibility.Visible;
                        break;
                    case CalendarType.Persian:
                        sender.DatePickerGrid.Visibility = Visibility.Visible;
                        sender.PersianDatePicker.Visibility = Visibility.Visible;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static void CalendarTypePropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var sender = (MultiTypeDateTimeControl)o;

            setControlsVisibility(sender);
        }

        private static void setControlsVisibility(MultiTypeDateTimeControl sender)
        {
            switch (sender.DisplayMode)
            {
                case DisplayMode.Date:
                    setCalendarControlsVisibility(sender, Visibility.Visible);
                    sender.TimePicker.Visibility = Visibility.Collapsed;

                    break;
                case DisplayMode.Time:
                    setCalendarControlsVisibility(sender, Visibility.Collapsed);
                    sender.TimePicker.Visibility = Visibility.Visible;

                    break;
                case DisplayMode.DateTime:
                    setCalendarControlsVisibility(sender, Visibility.Visible);
                    sender.TimePicker.Visibility = Visibility.Visible;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [DefaultValue(Orientation.Horizontal)]
        public Orientation DisplayOrientation
        {
            get
            {
                return (Orientation)this.GetValue(DisplayOrientationProperty);
            }
            set
            {
                this.SetValue(DisplayOrientationProperty, value);
            }
        }

        [TypeConverter(typeof(DateTimeTypeConverter))]
        public DateTime? SelectedDateTime
        {
            get
            {
                return (DateTime?)this.GetValue(SelectedDateTimeProperty);
            }
            set
            {
                this.SetValue(SelectedDateTimeProperty, value);
            }
        }

        [TypeConverter(typeof(DateTimeTypeConverter))]
        public DateTime? SelectedDate
        {
            get
            {
                return (DateTime?)this.GetValue(SelectedDateProperty);
            }
            set
            {
                this.SetValue(SelectedDateProperty, value);
            }
        }

        [TypeConverter(typeof(DateTimeTypeConverter))]
        public DateTime? SelectedTime
        {
            get
            {
                return (DateTime?)this.GetValue(SelectedTimeProperty);
            }
            set
            {
                this.SetValue(SelectedTimeProperty, value);
            }
        }

        [DefaultValue(DisplayMode.Date)]
        public DisplayMode DisplayMode
        {
            get
            {
                return (DisplayMode)this.GetValue(DisplayModeProperty);
            }
            set
            {
                this.SetValue(DisplayModeProperty, value);
            }
        }

        [DefaultValue(CalendarType.Persian)]
        public CalendarType CalendarType
        {
            get
            {
                return (CalendarType)this.GetValue(CalendarTypeProperty);
            }
            set
            {

                this.SetValue(CalendarTypeProperty, value);
            }
        }


        [DefaultValue(false)]
        public bool IsReadonly
        {
            get
            {
                return (bool)this.GetValue(IsReadonlyProperty);
            }
            set
            {

                this.SetValue(IsReadonlyProperty, value);
            }
        }

        public MultiTypeDateTimeControl()
            : base()
        {
            InitializeComponent();

            this.CalendarTypeComboBox.ItemsSource = Enum.GetValues(typeof(CalendarType));  //typeof (CalendarType).ToComboItemList();
        }

        private void MultiTypeDateTimeControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            manageSelectedDateSetValue(this);
            manageSelectedDateTimeSetValue(this);
            manageSelectedTimeSetValue(this);
            setControlsVisibility(this);
        }
    }

    public enum DisplayMode
    {
        Date,
        Time,
        DateTime,
    }

    public enum CalendarType
    {
        [Description("میلادی")]
        Gregorian = 1,
        [Description("شمسی")]
        Persian
    }

    public class OrientationToCalendarGridMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Orientation)) return null;

            switch ((Orientation)value)
            {
                case Orientation.Vertical:
                    return new Thickness(0, 0, 0, 3);
                    break;
                case Orientation.Horizontal:
                    return new Thickness(0, 0, 3, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    public class IsReadonlyToIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !( value is bool)) return null;

            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
