using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
   public partial class AttachmentDto
    {
       int id;
       public int Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

       string _attachmentName;
        public string AttachmentName
        {
            get { return _attachmentName; }
            set { this.SetField(p => p.AttachmentName, ref _attachmentName, value); }
        }
      
        string _ext;
        public string Ext
        {
            get { return _ext; }
            set { this.SetField(p => p.Ext, ref _ext, value); }
        }

        byte[] _attachmentContent;
        public byte[] AttachmentContent
        {
            get { return _attachmentContent; }
            set { this.SetField(p => p.AttachmentContent, ref _attachmentContent, value); }
        }
    }
}
