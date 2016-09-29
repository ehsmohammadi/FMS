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
    public partial class VesselDto
    {
        private long _id;
        private string _code;
        private long _ownerId;
        private CompanyDto owner;

        private string _name;
        private string _description;

        public long Id
        {
            get { return _id; }
            set { this.SetField(p => p.Id, ref _id, value); }
        }

        public string Code
        {
            get { return _code; }
            set { this.SetField(p => p.Code, ref _code, value); }
        }

        public long OwnerId
        {
            get { return _ownerId; }
            set { this.SetField(p => p.OwnerId, ref _ownerId, value); }
        }

        public CompanyDto Owner
        {
            get { return owner; }
            set { this.SetField(p => p.Owner, ref owner, value); }
        }

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
    }
}