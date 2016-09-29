using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.Factories
{
    public interface IDomainServiceFactory : IFactory
    {
        MITD.Domain.Model.IDomainService<T> GetDomainService<T>() where T : class;
    }

}
