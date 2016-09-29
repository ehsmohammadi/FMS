using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{

    public partial class UserDto : PartyDto
    {
        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        string code;
        [Required(AllowEmptyStrings = false, ErrorMessage = "code can't be empty")]
        public string Code
        {
            get { return code; }
            set { this.SetField(p => p.Code, ref code, value); }
        }

        string firstName;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Name can't be empty")]
        public string FirstName
        {
            get { return firstName; }
            set { this.SetField(p => p.FirstName, ref firstName, value); }
        }


        string lastName;
        // [Required(AllowEmptyStrings = false, ErrorMessage = "Name can't be empty")]
        public string LastName
        {
            get { return lastName; }
            set { this.SetField(p => p.LastName, ref lastName, value); }
        }


        string userName;
        // [Required(AllowEmptyStrings = false, ErrorMessage = "Name can't be empty")]
        public string UserName
        {
            get { return userName; }
            set { this.SetField(p => p.UserName, ref userName, value); }
        }


        string email;
        // [Required(AllowEmptyStrings = false, ErrorMessage = "Name can't be empty")]
        public string Email
        {
            get { return email; }
            set { this.SetField(p => p.Email, ref email, value); }
        }

        private bool isActive;
        public bool Active
        {
            get { return isActive; }
            set { this.SetField(p => p.Active, ref isActive, value); }
        }

        private List<UserGroupDescriptionDto> _groupDescriptionDtos = new List<UserGroupDescriptionDto>();
        public List<UserGroupDescriptionDto> Groups
        {
            get { return _groupDescriptionDtos; }
            set { this.SetField(p => p.Groups, ref _groupDescriptionDtos, value); }
        }




        private CompanyDto _companyDto;
        public CompanyDto CompanyDto
        {
            get { return _companyDto; }
            set { this.SetField(p => p.CompanyDto, ref _companyDto, value); }
        }
    }
}
