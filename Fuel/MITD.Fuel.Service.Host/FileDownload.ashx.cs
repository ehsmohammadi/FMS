using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MITD.Core;
using MITD.Fuel.Data.EF.FileStreaming;
using MITD.Fuel.Presentation.Contracts.FacadeServices;

namespace MITD.Fuel.Service.Host
{
    /// <summary>
    /// Summary description for FileDownload
    /// </summary>
    public class FileDownload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {


            var attachmentService = ServiceLocator.Current.GetInstance<IAttachmentFacadeService>();
            int entityId = Convert.ToInt32(context.Request.QueryString["id"]);

            var res = attachmentService.Download(entityId);
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.AppendHeader("Content-Length", res.AttachmentContent.Length.ToString());

            context.Response.ContentType = "application/octet-stream";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + res.AttachmentName + "." + res.Ext);


            context.Response.BinaryWrite(res.AttachmentContent);
            context.Response.End();
            
            
           
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