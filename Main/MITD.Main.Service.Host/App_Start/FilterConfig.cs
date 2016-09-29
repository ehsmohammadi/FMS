using System.Web;
using System.Web.Mvc;

namespace MITD.Main.Services.Host
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}