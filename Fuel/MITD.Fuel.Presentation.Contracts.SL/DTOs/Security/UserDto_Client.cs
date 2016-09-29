using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    
    public partial class UserDto
    {
        public string FullName {
            get
            {
                return FirstName + "  " + LastName;
            }
        }
    }
}
