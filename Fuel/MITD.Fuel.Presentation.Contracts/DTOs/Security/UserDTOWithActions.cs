using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class UserDTOWithActions : PartyDTOWithActions
    {
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

        private string email;
        public string Email
        {
            get { return email; }
            set { this.SetField(p => p.Email, ref email, value); }
        }

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { this.SetField(p => p.IsActive, ref isActive, value); }
        }

        private CompanyDto _companyDto;
        public CompanyDto CompanyDto
        {
            get { return _companyDto; }
            set { this.SetField(p => p.CompanyDto, ref _companyDto, value); }
        }

    }
}
