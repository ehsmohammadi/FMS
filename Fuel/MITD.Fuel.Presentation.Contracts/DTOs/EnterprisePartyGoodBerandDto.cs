using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Presentation;
namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class EnterprisePartyGoodBerandDto
    {
        private long id;
        private string name;

        public long Id
        {
            get { return id; }
            set { this.SetField(P => P.Id, ref id, value); }
        }
        public string Name
        {
            get { return name; }
            set { this.SetField(P => P.Name, ref name, value); }
        }
    }
}
