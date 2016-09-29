#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

#endregion

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class VesselInCompanyDto
    {
        private string _code;
        private string _description;
        private string _name;
        private List<TankDto> _tankDtos;
        private CompanyDto company;
        private long id;
        private VesselStateEnum vesselState;
        private long companyId;

        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "VoyageNumber can't be empty")]
        public string Code
        {
            get { return _code; }
            set { this.SetField(p => p.Code, ref _code, value); }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "name can't be empty")]
        public string Name
        {
            get { return _name; }
            set { this.SetField(p => p.Name, ref _name, value); }
        }

        public string Description
        {
            get { return _description; }
            set { this.SetField(p => p.Description, ref _description, value); }
        }

        public CompanyDto Company
        {
            get { return company; }
            set { this.SetField(p => p.Company, ref company, value); }
        }
        public long CompanyId
        {
            get { return companyId; }
            set { this.SetField(p => p.CompanyId, ref companyId, value); }
        }

        public VesselStateEnum VesselState
        {
            get { return this.vesselState; }
            set { this.SetField(p => p.VesselState, ref vesselState, value); }
        }

        public List<TankDto> TankDtos
        {
            get { return _tankDtos; }
            set { this.SetField(p => p.TankDtos, ref _tankDtos, value); }
        }
    }
}