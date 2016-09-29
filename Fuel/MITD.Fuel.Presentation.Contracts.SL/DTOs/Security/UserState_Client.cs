using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class UserStateDTO : ViewModelBase
    {

        public string FullName
        {
            get { return firstName + " " + LastName; }
        }

    }


}
