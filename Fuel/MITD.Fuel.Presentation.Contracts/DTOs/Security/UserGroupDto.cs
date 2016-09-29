using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class UserGroupDto : PartyDto
    {
        string description;

        public string Description
        {
            get { return description; }
            set { this.SetField(p => p.Description, ref description, value); }
        }
    }
}
