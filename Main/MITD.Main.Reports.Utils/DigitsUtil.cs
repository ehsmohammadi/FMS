using System;
using System.Collections.Generic;
using System.Globalization;

namespace MITD.Main.Reports.Utils
{
    /// <summary>
    /// This class contains methods that are used in SSRS reports. The dll of current assembly is referenced by SSRS report files (.rdl) at server level.
    /// The dll should also be published at an accessible folder on server hosting SSRS.
    /// </summary>
    public static class DigitsUtil
    {
        public static string GetPersianDate(DateTime? dateToConvert)
        {
            var result = string.Empty;
            if (dateToConvert != null)
            {
                var pCal = new PersianCalendar();
                try
                {
                    result = string.Format("{0}/{1}/{2}", pCal.GetYear(dateToConvert.Value), pCal.GetMonth(dateToConvert.Value), pCal.GetDayOfMonth(dateToConvert.Value));
                }
                catch { }
            }
            return result;
        }

        public static string ConvertDigitsToFarsiDigits(string text)
        {
            var result = string.Empty;

            var digitsLookup = new Dictionary<Char, Char>();
            digitsLookup.Add('0', '۰');
            digitsLookup.Add('1', '۱');
            digitsLookup.Add('2', '۲');
            digitsLookup.Add('3', '۳');
            digitsLookup.Add('4', '۴');
            digitsLookup.Add('5', '۵');
            digitsLookup.Add('6', '۶');
            digitsLookup.Add('7', '۷');
            digitsLookup.Add('8', '۸');
            digitsLookup.Add('9', '۹');

            for (int index = 0; index < text.Length; index++)
            {
                if (digitsLookup.ContainsKey(text[index]))
                    result += digitsLookup[text[index]];
                else
                    result += text[index];
            }

            return result;
        }

        public static string GetPersianDateWithFarsiDigits(DateTime dateToConvert)
        {
            return ConvertDigitsToFarsiDigits(GetPersianDate(dateToConvert));
        }
    }
}
