using System.Web.Mvc;

namespace MITD.Fuel.Service.Host.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return this.View();
        }
    }
}