using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;

namespace MITD.Fuel.Domain.Model.Repositories
{
    public interface ICharterItemHistoryRepository : IRepository<CharterItemHistory>
    {
    }
}
