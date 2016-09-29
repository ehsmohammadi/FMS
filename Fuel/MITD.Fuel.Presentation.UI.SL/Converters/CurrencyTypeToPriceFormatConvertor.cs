using System;
using System.Globalization;
using System.Windows.Data;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.UI.SL.Converters
{
    public class CurrencyTypeToPriceFormatConvertor : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CurrencyDto)
            {
                var currency = (CurrencyDto)value;

                return currency.Abbreviation == "IRR" ? "{0:n0}" : "{0:n2}";
            }
            else if(value is string )
                return (value as string) == "IRR" ? "{0:n0}" : "{0:n2}";

            return "{0:n2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
