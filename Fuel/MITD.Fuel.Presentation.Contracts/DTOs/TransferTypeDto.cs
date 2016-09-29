using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
   public partial class TransferTypeDto
    {
        private long id;
        private string name;
        private string code;



        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        public string Name
        {
            get { return name; }
            set { this.SetField(p => p.Name, ref name, value); }
        }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Code is required")]
        public string Code
        {
            get { return code; }
            set { this.SetField(p => p.Code, ref code, value); }
        }

    }
}
