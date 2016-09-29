using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class AttachmentDto :ViewModelBase
    {
       
        public string Url
        {
            get { return ApiConfig.HostAddress + "FileDownload.ashx?id=" + Id.ToString(); }
           
        }
    }
}
