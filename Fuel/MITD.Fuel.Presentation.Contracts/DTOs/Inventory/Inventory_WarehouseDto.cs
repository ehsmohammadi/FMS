
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{

    public partial class Inventory_WarehouseDto
    {
        public Inventory_WarehouseDto()
        {
        }

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

        long companyId;
        public long CompanyId
        {
            get { return companyId; }
            set { this.SetField(p => p.CompanyId, ref companyId, value); }
        }

        bool? isActive;
        public bool? IsActive
        {
            get { return isActive; }
            set { this.SetField(p => p.IsActive, ref isActive, value); }
        }

        int? userCreatorId;
        public int? UserCreatorId
        {
            get { return userCreatorId; }
            set { this.SetField(p => p.UserCreatorId, ref userCreatorId, value); }
        }

        private DateTime? createDate;
        public DateTime? CreateDate
        {
            get { return this.createDate; }
            set { this.SetField(p => p.CreateDate, ref this.createDate, value); }
        }

        private Inventory_CompanyDto company;
        public Inventory_CompanyDto Company
        {
            get { return company; }
            set { this.SetField(p => p.Company, ref company, value); }
        }

        private Inventory_UserDto userCreator;
        public Inventory_UserDto UserCreator
        {
            get { return userCreator; }
            set { this.SetField(p => p.UserCreator, ref userCreator, value); }
        }
    }
}
