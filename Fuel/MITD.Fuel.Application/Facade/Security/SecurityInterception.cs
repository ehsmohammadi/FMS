using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using MITD.Fuel.Application.Facade.Security;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.FuelSecurity.Domain.Model.ErrorException;
using MITD.Presentation;

namespace MITD.Fuel.Application
{
   public class SecurityInterception:IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var securityService = SecurityFacadeServiceFactory.Create();
            try
            {
                var user = ClaimsPrincipal.Current;

                if (invocation.Method.DeclaringType.Name == "IApprovalFlowFacadeService")
                {

                    //var dto = invocation.Arguments[0] as ApprovmentDto;
                    //if (securityService.IsAuthorize(dto.ActionEntityType, dto.DecisionType, user))
                    //{
                    invocation.Proceed();
                    //    logServicesAccess(invocation, user);
                    //}
                    //else
                    //{
                    //    throw new FuelSecurityAccessException(7001, "Access Error");
                    //}
                }
                else
                {
                    if (securityService.IsAuthorize(invocation.Method.DeclaringType.Name, invocation.Method.Name, user))
                    {
                        invocation.Proceed();
                        logServicesAccess(invocation, user);

                    }
                    else
                    {
                        throw new FuelSecurityAccessException(7001, "Access Error");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                SecurityFacadeServiceFactory.Release(securityService);
            }
        }


        private void logServicesAccess(IInvocation invocation, ClaimsPrincipal user)
        {
            var srvManagerLog =LogServiceFacadeFactory.Create();
            try
            {
                if(invocation.Method.Name.ToUpper().StartsWith("GET") ||
                    invocation.Method.Name.ToUpper().StartsWith("FIND") ||
                    (invocation.Method.Name.ToUpper().StartsWith("ADD") &&
                    invocation.Method.DeclaringType.Name.Equals(typeof(IFuelReportFacadeService).Name)
                    )
                )
                    return;
                
                var logSrv = srvManagerLog;//.GetService();

                string code = "Interceptor_AfterProceed";
                string title = "Clalling Facade Service " + invocation.Method.DeclaringType.Name;
                string userName = (user != null) ? user.Identity.Name : "";
                string logLevel = "AccessControl";
                string className = invocation.Method.DeclaringType.Name;
                string methodName = invocation.Method.Name;
                string messages = "user Authorized to call this method";
                logSrv.AddEventLog(title, code, logLevel, className, methodName, messages, userName);
            }
            finally
            {
                LogServiceFacadeFactory.Release(srvManagerLog);
            }
        }
    }
}
