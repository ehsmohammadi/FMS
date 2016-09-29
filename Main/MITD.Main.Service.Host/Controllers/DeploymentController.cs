using System.Web.Mvc;
using MITD.Services;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MITD.Main.Services.Host.Controllers
{
    public class DeploymentController : Controller
    {
        public ActionResult GetXapVersion(string fileName)
        {
            return this.Content(DeploymentServiceHelper.GetXapFileVersion(fileName));
        }

        [System.Web.Http.HttpPost]
        public ActionResult GetXapVersions(string fileNames)
        {
            var j = JArray.Parse(fileNames);
            var res = JsonConvert.DeserializeObject<string[]>(j.ToString());
            return this.Json(DeploymentServiceHelper.GetXapFileVersion(res));
        }
    }
}
