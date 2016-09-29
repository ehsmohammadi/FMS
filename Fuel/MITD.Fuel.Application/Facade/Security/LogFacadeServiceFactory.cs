using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.FacadeServices;

namespace MITD.Fuel.Application.Facade.Security
{
    public static class LogServiceFacadeFactory
    {
        public static ILogFacadeService Create()
        {
            return ServiceLocator.Current.GetInstance<ILogFacadeService>();
        }


        public static void Release(ILogFacadeService instance)
        {
            ServiceLocator.Current.Release(instance);
        }
    }
}
