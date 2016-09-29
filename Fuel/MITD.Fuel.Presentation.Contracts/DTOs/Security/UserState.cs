using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class UserStateDTO
    {
        private long userId;
        public long UserId
        {
            get { return userId; }
            set { this.SetField(p => p.UserId, ref userId, value); }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set { this.SetField(p => p.Username, ref username, value); }
        }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { this.SetField(p => p.FirstName, ref firstName, value); }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set { this.SetField(p => p.LastName, ref lastName, value); }
        }

        private string employeeNo;
        public string EmployeeNo
        {
            get { return employeeNo; }
            set { this.SetField(p => p.EmployeeNo, ref employeeNo, value); }
        }

        private List<long> jobPositionIds;
        public List<long> JobPositionIds
        {
            get { return jobPositionIds; }
            set { this.SetField(p => p.JobPositionIds, ref jobPositionIds, value); }
        }

        private string jobPositionNames;
        public string JobPositionNames
        {
            get { return jobPositionNames; }
            set { this.SetField(p => p.JobPositionNames, ref jobPositionNames, value); }
        }

        private List<string> roleNames = new List<string>();
        public List<string> RoleNames
        {
            get { return roleNames; }
            set { this.SetField(p => p.RoleNames, ref roleNames, value); }
        }


        private List<ActionTypeDto> permittedActions = new List<ActionTypeDto>();
        public List<ActionTypeDto> PermittedActions
        {
            get { return permittedActions; }
            set { this.SetField(p => p.PermittedActions, ref permittedActions, value); }
        }

        private string currentWorkListUserName = "";
        public string CurrentWorkListUserName
        {
            get { return currentWorkListUserName; }
            set { this.SetField(p => p.CurrentWorkListUserName, ref currentWorkListUserName, value); }
        }

        private List<UserDescriptionDTO> permittedUsersOnMyWorkList = new List<UserDescriptionDTO>();
        public List<UserDescriptionDTO> PermittedUsersOnMyWorkList
        {
            get { return permittedUsersOnMyWorkList; }
            set { this.SetField(p => p.PermittedUsersOnMyWorkList, ref permittedUsersOnMyWorkList, value); }
        }


        public string UserTitle
        {
            get
            {
                string res = "";
                if (IsEmployee)
                    res += "کارمند ";
                if (IsAdmin)
                    res += "مدیر ";
                return res;
            }


        }

        public bool IsAdmin
        {
            get
            {
                if (RoleNames.Contains("Admin"))
                    return true;
                else
                    return false;
            }
        }

        public bool IsEmployee
        {
            get
            {
                if (RoleNames.Contains("Employee"))
                    return true;
                else
                    return false;
            }
        }

    }
}
