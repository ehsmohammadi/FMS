using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.Factories
{
    public class DomainServiceFactory : IDomainServiceFactory
    {
        public IDomainService<T> GetDomainService<T>() where T : class
        {
            return ServiceLocator.Current.GetInstance<IDomainService<T>>();
        }
    }
}
