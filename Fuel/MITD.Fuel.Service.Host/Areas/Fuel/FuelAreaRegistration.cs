using System.Web.Mvc;

namespace MITD.Fuel.Service.Host.Areas.Fuel
{
    public class FuelAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Fuel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Fuel_default",
                "Fuel/{controller}/{action}/{id}",
                new { action = "Get", id = UrlParameter.Optional }
            );
        }
    }
}
