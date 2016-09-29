using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.FuelSecurity.Domain.Model.Service
{
   public interface ILoggerServiceFactory
    {
        ILoggerService Create(string key);
        void Release(ILoggerService loggerService);
    }
}
