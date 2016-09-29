using System;
using System.Globalization;
using System.Windows.Data;
using MITD.Core;

namespace MITD.Fuel.Presentation.Logic.SL.Converters
{
    public class DateToPersianAndGregorianTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime?)) return string.Empty;

            var valueToConvert = ((DateTime?)value).HasValue ? ((DateTime?)value) : DateTime.Now;

            int year, month, dayOfMonth;

            PDateHelper.GregorianToHijri(valueToConvert.Value.Year, valueToConvert.Value.Month, valueToConvert.Value.Day, out year, out month, out dayOfMonth);

            var hour = valueToConvert.Value.Hour;
            var minute = valueToConvert.Value.Minute;
            var second = valueToConvert.Value.Second;

            var persianResult = string.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}", year, month, dayOfMonth, hour, minute);
            var result = string.Format("{0} ({1})", persianResult, valueToConvert.Value.ToString("yyyy/MM/dd"));

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
