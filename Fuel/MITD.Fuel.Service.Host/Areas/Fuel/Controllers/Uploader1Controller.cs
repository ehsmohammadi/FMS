using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class UploaderController : Controller
    {
        // GET: Fuel/Uploader1
        public ActionResult Index()
        {
            return View();
        }

        // GET: Fuel/Uploader1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Fuel/Uploader1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fuel/Uploader1/Create
        [HttpPost]
        public void POST(FormCollection collection)
        {
            var context = HttpContext;
            
           
                if (context.Request.InputStream.Length == 0)
                    throw new ArgumentException("No file input");
                if (context.Request.QueryString["fileName"] == null)
                    throw new Exception("Parameter fileName not set!");

                string fileName = context.Request.QueryString["fileName"];
                string filePath = @HostingEnvironment.ApplicationPhysicalPath + "/" + fileName;
                bool appendToFile = context.Request.QueryString["append"] != null && context.Request.QueryString["append"] == "1";

                FileMode fileMode;
                if (!appendToFile)
                {
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                    fileMode = FileMode.Create;
                }
                else
                {
                    fileMode = FileMode.Append;
                }
                bool uploadSuccesful = false;
                while (!uploadSuccesful)
                {
                    try
                    {
                        using (FileStream fs = System.IO.File.Open(filePath, fileMode))
                        {
                            byte[] buffer = new byte[4096];
                            int bytesRead;
                            while ((bytesRead = context.Request.InputStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                fs.Write(buffer, 0, bytesRead);
                            }
                            fs.Flush();
                            fs.Close();
                            uploadSuccesful = true;
                        }
                    }
                    catch (IOException)
                    {
                    }
                }
         
          
        }

        // GET: Fuel/Uploader1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Fuel/Uploader1/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Fuel/Uploader1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Fuel/Uploader1/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
  //using (FileStream fs = File.Open(@"d:\\x.pdf", FileMode.Append))
  //          {
  //              byte[] buffer = new byte[4096];
  //              int bytesRead;
  //              while ((bytesRead = value.Read(buffer, 0, buffer.Length)) != 0)
  //              {
  //                  fs.Write(buffer, 0, bytesRead);
  //              }
  //              fs.Flush();
  //              fs.Close();

  //          }