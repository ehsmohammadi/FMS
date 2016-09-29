
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{

    public partial class Inventory_StoreTypeDto
    {
        public Inventory_StoreTypeDto()
        {
        }

        int id;
        public int Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        short code;
        public short Code
        {
            get { return code; }
            set { this.SetField(p => p.Code, ref code, value); }
        }

        byte type;
        public byte Type
        {
            get { return type; }
            set { this.SetField(p => p.Type, ref type, value); }
        }

        string inputName;
        public string InputName
        {
            get { return inputName; }
            set { this.SetField(p => p.InputName, ref inputName, value); }
        }

        string outputName;
        public string OutputName
        {
            get { return outputName; }
            set { this.SetField(p => p.OutputName, ref outputName, value); }
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
