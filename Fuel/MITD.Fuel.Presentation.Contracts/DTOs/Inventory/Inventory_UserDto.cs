
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{

    public partial class Inventory_UserDto
    {
        public Inventory_UserDto()
        {
        }

        int id;
        public int Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        int code;
        public int Code
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

        string userName;
        public string UserName
        {
            get { return userName; }
            set { this.SetField(p => p.UserName, ref userName, value); }
        }

        string password;
        public string Password
        {
            get { return password; }
            set { this.SetField(p => p.Password, ref password, value); }
        }

        string emailAddress;
        public string EmailAddress
        {
            get { return emailAddress; }
            set { this.SetField(p => p.EmailAddress, ref emailAddress, value); }
        }

        string ipAddress;
        public string IpAddress
        {
            get { return ipAddress; }
            set { this.SetField(p => p.IpAddress, ref ipAddress, value); }
        }

        bool? login;
        public bool? Login
        {
            get { return login; }
            set { this.SetField(p => p.Login, ref login, value); }
        }

        string sessionId;
        public string SessionId
        {
            get { return sessionId; }
            set { this.SetField(p => p.SessionId, ref sessionId, value); }
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
