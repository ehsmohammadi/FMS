using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MITD.Fuel.Presentation.UI.SL.Converters
{
    public class EmptyValueConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //This method will be called on binding source value to text of TextBox.
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return string.Empty;
            }

            return value;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //This method will be called after the text of TextBox has been changed.
            var localdata = value as string;
            if (string.IsNullOrEmpty(localdata))
            {
                return null;
            }

            return value;
        }
    }
}
