using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class UserDescriptionDTO
    {

        private string partyName;
        public string PartyName
        {
            get { return partyName; }
            set { this.SetField(p => p.PartyName, ref partyName, value); }
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






    }
}
