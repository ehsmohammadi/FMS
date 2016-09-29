using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using MITD.Core;
using MITD.Fuel.Data.EF.FileStreaming;
using MITD.Fuel.Presentation.Contracts.FacadeServices;


namespace MITD.Fuel.Service.Host
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class FileUpload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.InputStream.Length == 0)
                throw new ArgumentException("No file input");
            if (context.Request.QueryString["fileName"] == null)
                throw new Exception("Parameter fileName not set!");

            string fileName = context.Request.QueryString["fileName"];
            string ext = context.Request.QueryString["ext"];
            long entityId =Convert.ToInt64(context.Request.QueryString["entityId"]);
            int entityType = Convert.ToInt32(context.Request.QueryString["entityType"]);
            var fileData = new byte[context.Request.InputStream.Length];
            context.Request.InputStream.Read(fileData, 0, Convert.ToInt32(context.Request.InputStream.Length));
            var attachmentService = ServiceLocator.Current.GetInstance<IAttachmentFacadeService>();
            
            attachmentService.Upload(0,fileData,fileName,ext,entityId,entityType,Guid.NewGuid());
          
            //context.Request.InputStream.Read(fileData, 0, Convert.ToInt32(context.Request.InputStream.Length));
            //using (var dataContext = new FileStreamingEntities())
            //{
            //    var entity = new Attachment()
            //    {
            //        AttachmentName = fileName,
            //        RowGUID = Guid.NewGuid(),
            //        AttachmentContent = fileData,
            //        EntityId = entityId,
            //        EntityType = entityType,
            //        AttachmentExt=ext
                

            //    };
            //    dataContext.Entry(entity).State = EntityState.Added;
            //    dataContext.SaveChanges();
               
            //}
            

            

            //bool appendToFile = context.Request.QueryString["append"] != null && context.Request.QueryString["append"] == "1";

            //FileMode fileMode;
            //if (!appendToFile)
            //{
            //    if (File.Exists(filePath))
            //        File.Delete(filePath);
            //    fileMode = FileMode.Create;
            //}
            //else
            //{
            //    fileMode = FileMode.Append;
            //}
            //bool uploadSuccesful = false;
            //while (!uploadSuccesful)
            //{
            //    try
            //    {
            //        using (FileStream fs = File.Open(filePath, fileMode))
            //        {
            //            byte[] buffer = new byte[4096];
            //            int bytesRead;
            //            while ((bytesRead = context.Request.InputStream.Read(buffer, 0, buffer.Length)) != 0)
            //            {
            //                fs.Write(buffer, 0, bytesRead);
            //            }
            //            fs.Flush();
            //            fs.Close();
            //            uploadSuccesful = true;
            //        }
            //    }
            //    catch (IOException ix)
            //    {
            //        var x = ix;
            //    }
            //}

           



        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}