using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Data.EF.FileStreaming;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Data.EF.Repositories
{
   public class AttachmentRepositorycs:IAttachmentRepository
    {
       public bool LazyLoadingEnabled { get; set; }
       public bool ProxyCreationEnabled { get; set; }

       public void Upload(int rowId, byte[] attachmentContent, string attachmentName, string attachmentExt, long entityId,
           int entityType, Guid rowGUID)
       {

           var entity = new Attachment()
           {
               AttachmentName = attachmentName,
               RowGUID = rowGUID,
               AttachmentContent = attachmentContent,
               EntityId = entityId,
               EntityType = entityType,
               AttachmentExt = attachmentExt


           };
           
           using (var dataContext = new FileStreamingEntities())
           {

               dataContext.Entry(entity).State = EntityState.Added;
               dataContext.SaveChanges();

           }


       }


       public Tuple<string, string, byte[]> Download(int entityId)
       {
           
           var db = new FileStreamingEntities();
           var x = db.Attachments.Where(c => c.RowID == entityId).SingleOrDefault();

           return new Tuple<string, string, byte[]>(x.AttachmentName,x.AttachmentExt,x.AttachmentContent);
       }
    }
}
