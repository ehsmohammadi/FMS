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
using System.Linq;
using MITD.Fuel.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.Enums;
using System.Reflection;

namespace MITD.Fuel.Presentation.UI.SL.Converters
{
    public class TransactionStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result= from Enum enumValue in System.Enum.GetValues(typeof (TransactionStatusEnum))
                        where (TransactionStatusEnum)enumValue == (TransactionStatusEnum)(byte)value
                select enumValue.GetDescription();

            return result.FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
