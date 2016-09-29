using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class UploaderController : ApiController
    {
        // GET: api/Uploader
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Uploader/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Uploader
        public void Post([FromBody]Stream value)
        {

            using (FileStream fs = File.Open(@"d:\\x.pdf", FileMode.Append))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = value.Read(buffer, 0, buffer.Length)) != 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                }
                fs.Flush();
                fs.Close();

            }


            var x = value;
        }

        // PUT: api/Uploader/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Uploader/5
        public void Delete(int id)
        {
        }
    }
}
