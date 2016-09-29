using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IFileServiceWrapper:IServiceWrapper
    {
        void Upload(Action<bool> action, Stream dataStream, string fileName,string ext, long entityId, AttachmentType attachmentType,
            long bytesTotal);

        void Get(Action<List<AttachmentDto>, Exception> action, long entityId, AttachmentType attachmentType);
        void Delete(Action<string, Exception> action, int id);
    }
}
