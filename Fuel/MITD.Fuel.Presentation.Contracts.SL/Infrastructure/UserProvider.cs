
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;

namespace MITD.Fuel.Presentation.Contracts.SL.Infrastructure
{
    public class UserProvider : IUserProvider
    {
        public UserProvider()
        {
            
        }

        public string Token
        {
            get;
            set;
        }

        public string SamlToken { get; set; }

        public bool IsAuthenticated { get { return SamlToken != null; } }

        public bool IsAuthorized { get { return IsAuthenticated && Token != null; } }
    }
}
