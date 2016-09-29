
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class UserDescriptionDTO : ViewModelBase
    {
        public string FullName
        {
            get { return FirstName + " " + LastName; }

        }
    }
}
