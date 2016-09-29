using System;

namespace MITD.Fuel.Presentation.Logic.SL.Infrastructure
{

    public class HttpUtil
    {
        public const string WEBAPI_DATE_PARAMETER_STRING_FORMAT = "yyyy/MM/dd";
        public const string WEBAPI_DATETIME_PARAMETER_STRING_FORMAT = "yyyy/MM/dd HH:mm:ss";

        public static string DateToString(DateTime? dt)
        {
            return dt.HasValue ? Uri.EscapeDataString(dt.Value.ToString(WEBAPI_DATE_PARAMETER_STRING_FORMAT)) : string.Empty;
        }

        public static string DateTimeToString(DateTime? dt)
        {
            return dt.HasValue ? Uri.EscapeDataString(dt.Value.ToString(WEBAPI_DATETIME_PARAMETER_STRING_FORMAT)) : string.Empty;
        }
    }


}
