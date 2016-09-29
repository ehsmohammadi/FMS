using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class TankDto
    {
        private long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Code is required")]
        private string code;
        public string Code
        {
            get { return this.code; }
            set { this.SetField(p => p.Code, ref this.code, value); }
        }


        private VesselInCompanyDto vesselInCompanyDto;
        public VesselInCompanyDto VesselInCompanyDto
        {
            get { return vesselInCompanyDto; }
            set { this.SetField(p => p.VesselInCompanyDto, ref vesselInCompanyDto, value); }
        }

    }
}
