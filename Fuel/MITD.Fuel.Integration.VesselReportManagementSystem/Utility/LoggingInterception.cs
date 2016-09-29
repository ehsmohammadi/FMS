using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace MITD.Fuel.Integration.VesselReportManagementSystem.Utility
{

    public class LoggingInterception : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch 
            {

                throw;
            }
            finally
            {
                
            }
        }
    }
}
