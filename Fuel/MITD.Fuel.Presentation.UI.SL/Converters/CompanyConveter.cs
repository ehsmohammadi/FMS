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
    public class CompanyConveter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var i = (long) value;
            switch (i)
            {
                case 1:
                {
                    return "IRISL";
                    break;
                }
                case 2:
                {
                    return "SAPID";
                    break;
                }
                case 3:
                {
                    return "HAFIZ";
                    break;
                }
                case 4:
                {
                    return "IMSENGCO";
                    break;
                }
                case 5:
                {
                    return "HAFEZ";
                    break;
                }
              
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
