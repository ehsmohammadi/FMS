using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Presentation.Contracts.SL.Infrastructure
{
    public interface IUserProvider
    {
        string Token { get; set; }
        string SamlToken { get; set; }
        bool IsAuthenticated { get; }
        bool IsAuthorized { get; }
    }
}
