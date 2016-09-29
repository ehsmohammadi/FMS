#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;

#endregion

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface ITankDomainService : IDomainService
    {
        Tank Get(long id);
    }
}
