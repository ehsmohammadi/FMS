using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts
{
    public class UserCriteria : ViewModelBase
    {
        private string partyName;
        public string PartyName
        {
            get { return partyName; }
            set
            {
                this.SetField(p => p.PartyName, ref partyName, value);
            }
        }

        private string fname;
        public string Fname
        {
            get { return fname; }
            set
            {
                this.SetField(p => p.Fname, ref fname, value);
            }
        }

        private string lname;
        public string Lname
        {
            get { return lname; }
            set
            {
                this.SetField(p => p.Lname, ref fname, value);
            }
        }

    }
}
