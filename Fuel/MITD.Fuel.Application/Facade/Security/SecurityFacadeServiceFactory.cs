using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.FacadeServices;

namespace MITD.Fuel.Application.Facade.Security
{
  public static  class SecurityFacadeServiceFactory
    {
      public static ISecurityFacadeService Create()
      {
          return ServiceLocator.Current.GetInstance<ISecurityFacadeService>();
      }

      public static void Release(ISecurityFacadeService securityFacadeService)
      {
          ServiceLocator.Current.Release(securityFacadeService);
      }

    }
}
