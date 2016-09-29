using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MITD.Fuel.Data.EF.FileStreaming;
using MITD.Fuel.Presentation.Contracts.DTOs;
using NHibernate.Linq;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class AttachmentController : ApiController
    {
        // GET: api/Attachment
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Attachment/5
        public List<AttachmentDto> Get(long id, int typid)
        {
            var result = new List<AttachmentDto>();

            var res = (new FileStreamingEntities()).Attachments.Where(c => c.EntityId == id && c.EntityType == typid)
                .Select(c => new { c.RowID, c.AttachmentName, c.AttachmentExt });
            res.ForEach(c => result.Add(new AttachmentDto()
            {
                AttachmentName = c.AttachmentName,
                Ext = c.AttachmentExt,
                Id = c.RowID
            }));

            return result;
        }

        // POST: api/Attachment
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Attachment/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Attachment/5
        public void Delete(int id)
        {
            var db = new FileStreamingEntities();
            var x = db.Attachments.Where(c => c.RowID == id).SingleOrDefault();
            db.Attachments.Remove(x);
            db.SaveChanges();
        }
    }
}
