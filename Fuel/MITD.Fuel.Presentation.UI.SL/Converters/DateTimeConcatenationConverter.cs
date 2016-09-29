using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MITD.Fuel.Presentation.UI.SL.Converters
{
    public class DateTimeConcatenationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTimes = value as IEnumerable<DateTime>;

            if (dateTimes != null)
                return string.Join(" , ", dateTimes.Select(dt=>dt.ToString(string.IsNullOrWhiteSpace(parameter as string) ? "yyyy/MM/dd" : parameter as string)));

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
