using System.Collections.Generic;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class CompanyDto
    {
        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        string code;
        public string Code
        {
            get { return code; }
            set { this.SetField(p => p.Code, ref code, value); }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { this.SetField(p => p.Name, ref name, value); }
        }

        bool isMemberOfHolding;
        public bool IsMemberOfHolding
        {
            get { return isMemberOfHolding; }
            set { this.SetField(p => p.IsMemberOfHolding, ref isMemberOfHolding, value); }
        }

        bool basisId;
        public bool BasisId
        {
            get { return basisId; }
            set { this.SetField(p => p.BasisId, ref basisId, value); }
        }

        List<VesselInCompanyDto> _vesselInCompanies;
        public List<VesselInCompanyDto> VesselInCompanies
        {
            get { return _vesselInCompanies; }
            set { this.SetField(p => p.VesselInCompanies, ref _vesselInCompanies, value); }
        }

        public CompanyDto()
        {
            _vesselInCompanies = new List<VesselInCompanyDto>();
        }
    }
}