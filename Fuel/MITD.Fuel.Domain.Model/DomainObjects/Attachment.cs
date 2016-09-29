using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class Attachment
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string FileName { get; set; }

        public string Extension { get; set; }
        public string Description { get; set; }

    }
}
