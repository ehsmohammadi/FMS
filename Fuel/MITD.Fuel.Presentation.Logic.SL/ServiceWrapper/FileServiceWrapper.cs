using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public class FileServiceWrapper : IFileServiceWrapper
    {
        private string addressUrl = ApiConfig.HostAddress + "FileUpload.ashx?append={0}&filename={1}&entityId={2}&entityType={3}&ext={4}";
        private string url = ApiConfig.HostAddress + "apiarea/Fuel/Attachment/{0}";
        
        private bool isSuccess;
        private Action<bool> _action; 
        public void Upload(Action<bool>action, Stream dataStream,string fileName,string ext,long entityId,AttachmentType attachmentType, long bytesTotal)
        {


          string  Url = String.Format(addressUrl, 0, fileName, entityId.ToString(), ((int)attachmentType).ToString(),ext); 

            byte[] fileContent = new byte[bytesTotal];
            int bytesRead = dataStream.Read(fileContent, 0, Int32.Parse(bytesTotal.ToString()));
            dataStream.Flush();


            WebClient wc = new WebClient();

            wc.OpenWriteCompleted += new OpenWriteCompletedEventHandler(wc_OpenWriteCompleted);
            Uri u = new Uri(Url);

            wc.OpenWriteAsync(u, null, new object[] { fileContent, bytesRead });

            _action = action;

        }



        void wc_OpenWriteCompleted(object sender, OpenWriteCompletedEventArgs e)
        {

            if (e.Error == null)
            {

                object[] objArr = e.UserState as object[];
                byte[] fileContent = objArr[0] as byte[];
                int bytesRead = Convert.ToInt32(objArr[1]);
                Stream outputStream = e.Result;
                outputStream.Write(fileContent, 0, bytesRead);
                outputStream.Close();
                isSuccess = true;

            }
            else
            {
                isSuccess = false;
            }

            _action.Invoke(isSuccess);
        }



        public void Get(Action<List<AttachmentDto>, Exception> action, long entityId, AttachmentType attachmentType)
        {
            var uri = String.Format(url, entityId);
            StringBuilder stringBuilder = new StringBuilder(uri);
            stringBuilder.Append("?typid=" + (int)attachmentType);
            WebClientHelper.Get<List<AttachmentDto>>(new Uri(stringBuilder.ToString(), UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);

        }


        public void Delete(Action<string, Exception> action, int id)
        {
            string uri = String.Format(url, id);
            WebClientHelper.Delete(new Uri(uri, UriKind.Absolute), action, ApiConfig.Headers);
        }
    }
}
