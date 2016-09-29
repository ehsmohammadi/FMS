using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class FuelUserDto 
    {
        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }


        private UserDto user;
        public UserDto User
        {
            get { return user; }
            set { this.SetField(p => p.User, ref user, value); }
        }
    }
}
