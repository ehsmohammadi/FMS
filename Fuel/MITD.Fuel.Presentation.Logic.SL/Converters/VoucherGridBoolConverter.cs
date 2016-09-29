using System;
using System.Globalization;
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
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.Converters
{
    public class VoucherGridBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var res = new System.Boolean();
            var val = (int) value;
            if (val==1)
            {
                res = false;
                return res;
            }
            else if(val==2 ||val==0)
            {
                res = true;
                return res;
            }
            else
            {
                res = true;
                return res;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
