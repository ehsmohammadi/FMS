using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;

namespace MITD.FuelSecurity.Domain.Model.Service
{
    public class LoggerServiceFactory : ILoggerServiceFactory
    {
        public ILoggerService Create(string key)
        {
            
            return ServiceLocator.Current.GetInstance<ILoggerService>(key);
        }

        public void Release(ILoggerService loggerService)
        {
            ServiceLocator.Current.Release(loggerService);
        }

    }
}
