
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{

    public partial class Inventory_UnitDto
    {
        public Inventory_UnitDto()
        {
        }

        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        string abbreviation;
        public string Abbreviation
        {
            get { return abbreviation; }
            set { this.SetField(p => p.Abbreviation, ref abbreviation, value); }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { this.SetField(p => p.Name, ref name, value); }
        }

        bool? isCurrency;
        public bool? IsCurrency
        {
            get { return isCurrency; }
            set { this.SetField(p => p.IsCurrency, ref isCurrency, value); }
        }

        bool? isBaseCurrency;
        public bool? IsBaseCurrency
        {
            get { return isBaseCurrency; }
            set { this.SetField(p => p.IsBaseCurrency, ref isBaseCurrency, value); }
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

        private Inventory_UserDto userCreator;
        public Inventory_UserDto UserCreator
        {
            get { return userCreator; }
            set { this.SetField(p => p.UserCreator, ref userCreator, value); }
        }
    }
}
