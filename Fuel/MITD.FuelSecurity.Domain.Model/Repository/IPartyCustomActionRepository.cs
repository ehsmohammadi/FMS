using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;

namespace MITD.FuelSecurity.Domain.Model.Repository
{
    public interface IPartyCustomActionRepository : IRepository<PartyCustomAction>
    {

        void DeleteAllByPartyId(long partyid);

    }
}
