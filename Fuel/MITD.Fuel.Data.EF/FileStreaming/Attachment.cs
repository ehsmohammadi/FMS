//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MITD.Fuel.Data.EF.FileStreaming
{
    using System;
    using System.Collections.Generic;
    
    public partial class Attachment
    {
        public int RowID { get; set; }
        public byte[] AttachmentContent { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentExt { get; set; }
        public long EntityId { get; set; }
        public int EntityType { get; set; }
        public System.Guid RowGUID { get; set; }
    }
}
