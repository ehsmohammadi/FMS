extern alias MITDCoreSL;

using System;
using System.Windows.Data;

namespace MITD.Fuel.Presentation.UI.SL.Converters
{
    public class GregorianDateToPersianConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is DateTime)) return string.Empty;

            var persianDateFormat = parameter as string;

            var dateTime = (DateTime)value;

            var result = string.Empty;

            try
            {

                int year, month, dayOfMonth;

                var pDateHelper = MITDCoreSL::MITD.Core.PDateHelper.GregorianToHijri(dateTime.Year, dateTime.Month, dateTime.Day, out year, out month, out dayOfMonth);

                var hour = dateTime.Hour;
                var minute = dateTime.Minute;
                var second = dateTime.Second;

                //var pCal = new GlobalPersianCalendar();

                //var year = pCal.GetYear(dateTime);
                //var month = pCal.GetMonth(dateTime);
                //var dayOfMonth = pCal.GetDayOfMonth(dateTime);
                //var hour = pCal.GetHour(dateTime);
                //var minute = pCal.GetMinute(dateTime);
                //var second = pCal.GetSecond(dateTime);



                if (persianDateFormat == null)
                {
                    result = string.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}", year, month, dayOfMonth, hour, minute);
                }
                else
                {


                    persianDateFormat = persianDateFormat.Replace("yyyy", year.ToString());
                    persianDateFormat = persianDateFormat.Replace("YYYY", year.ToString());
                    persianDateFormat = persianDateFormat.Replace("yy", (year % 100).ToString());
                    persianDateFormat = persianDateFormat.Replace("YY", (year % 100).ToString());

                    persianDateFormat = persianDateFormat.Replace("MM", month.ToString("00"));
                    persianDateFormat = persianDateFormat.Replace("M", month.ToString());

                    persianDateFormat = persianDateFormat.Replace("dd", dayOfMonth.ToString("00"));
                    persianDateFormat = persianDateFormat.Replace("d", dayOfMonth.ToString());

                    persianDateFormat = persianDateFormat.Replace("DD", dayOfMonth.ToString("00"));
                    persianDateFormat = persianDateFormat.Replace("D", dayOfMonth.ToString());


                    persianDateFormat = persianDateFormat.Replace("HH", hour.ToString("00"));
                    persianDateFormat = persianDateFormat.Replace("H", hour.ToString());
                    persianDateFormat = persianDateFormat.Replace("hh", hour.ToString("00"));
                    persianDateFormat = persianDateFormat.Replace("h", hour.ToString());

                    persianDateFormat = persianDateFormat.Replace("mm", minute.ToString("00"));
                    persianDateFormat = persianDateFormat.Replace("m", minute.ToString());

                    persianDateFormat = persianDateFormat.Replace("SS", second.ToString("00"));
                    persianDateFormat = persianDateFormat.Replace("S", second.ToString());
                    persianDateFormat = persianDateFormat.Replace("ss", second.ToString("00"));
                    persianDateFormat = persianDateFormat.Replace("s", second.ToString());

                    result = persianDateFormat;
                }
            }
            catch
            {
                result = string.Empty;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

