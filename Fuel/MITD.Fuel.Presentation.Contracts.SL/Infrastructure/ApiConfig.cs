
using System.Collections.Generic;

namespace MITD.Fuel.Presentation.Contracts.SL.Infrastructure
{
    public static class ApiConfig
    {
        public static string HostAddress { get; set; }
        public static Dictionary<string, string> Headers { get; set; }

        public static Dictionary<string, string> CreateHeaderDic(string token)
        {
            return new Dictionary<string, string> { { "SilverLight", "1" }, { "Authorization", "Session " + token } };
        }
    }
}
