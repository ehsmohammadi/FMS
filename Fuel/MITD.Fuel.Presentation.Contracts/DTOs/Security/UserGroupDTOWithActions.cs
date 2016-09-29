using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class UserGroupDtoWithActions : PartyDTOWithActions
    {
        private string description;
        public string Description
        {
            get { return description; }
            set { this.SetField(p => p.Description, ref description, value); }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set { this.SetField(p => p.LastName, ref lastName, value); }
        }
    }
}
