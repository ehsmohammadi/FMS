using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Providers.Entities;
using MITD.Core;
//using MITD.PMS.Application.Contracts;
//using MITD.PMS.Interface;
//using MITD.PMSSecurity.Domain;
//using MITD.FuelSecurity.Exceptions;
using MITD.Fuel.Application.Facade;
using MITD.Fuel.Application.Facade.Security;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;

namespace MITD.Fuel.Service.Host
{

    public class FakeIdentity : IIdentity
    {
        // for without security
        public string Name { get; private set; }
        public string AuthenticationType { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public FakeIdentity(string name, string authenticationType, bool isAuthenticated)
        {
            Name = name;
            AuthenticationType = authenticationType;
            IsAuthenticated = isAuthenticated;
        }
    }
    public class ClaimsTransformer : ClaimsAuthenticationManager
    {
        private ISecurityFacadeService _securityFacadeService;

        public ClaimsTransformer()
        {
            _securityFacadeService = ServiceLocator.Current.GetInstance<ISecurityFacadeService>();
        }
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (!incomingPrincipal.Identity.IsAuthenticated)
            {
                var res = base.Authenticate(resourceName, incomingPrincipal);
                return res;
            }
            //throw new Exception("hkjljhlkjhlkjhlkj");
            return configureAuthorizationClaim(incomingPrincipal);

        }
        private ClaimsPrincipal createIncomingPrincipal()
        {
            var identity = new FakeIdentity("ali", AuthenticationTypes.Basic, true);

            var incomingPrincipal = new ClaimsPrincipal(identity);
            incomingPrincipal.Identities.First().AddClaims(new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                //new Claim(ClaimTypes.Role, "Employee"),
               // new Claim("CurrentUserId", "13"),
              //  new Claim("CurrentUserCompanyId", "5"),
              //  new Claim("CurrentUserRoles", "Admin"),
              //  new Claim("CurrentUserActions", mergStringList(new List<string>(){"Admin"}).ToString()),

                new Claim("http://identityserver.thinktecture.com/claims/profileclaims/firstname", "احسان"),
                new Claim("http://identityserver.thinktecture.com/claims/profileclaims/lastname", "محمدی"),
                new Claim("http://identityserver.thinktecture.com/claims/profileclaims/jobpositionnames", "مدیر فنی"),
            });
            return incomingPrincipal;
        }

        private ClaimsPrincipal configureAuthorizationClaim(ClaimsPrincipal incomingPrincipal)
        {
            string currentUsername;
            var roles = new List<string>();
            var currentUserActions = new List<string>();

            //if (isCurrentUserSameAsLogonUser())
            //{
                currentUsername = incomingPrincipal.Identity.Name;
                var claimsRoles = incomingPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role);
                if (claimsRoles != null && claimsRoles.Any())
                    roles = claimsRoles.Select(c => c.Value).ToList();
            //}
//            else
//            {
//                currentUsername = HttpContext.Current.Request.QueryString["CurrentWorkListUserName"];
//                CheckIsValidWorkListCurrentUser(incomingPrincipal.Identity.Name, currentUsername);
//                var userService = ServiceLocator.Current.GetInstance<ISecurityApplicationService>();
                
////                roles = userService.GetRolesForUser(currentUsername).ToList();
//            }


            var userdto = _securityFacadeService.GetUser(currentUsername);
            if (userdto != null && userdto.Id>0)
            {
                incomingPrincipal.Identities.First().AddClaim(new Claim("CurrentUserId", userdto.Id.ToString()));
                incomingPrincipal.Identities.First()
                    .AddClaim(new Claim("CurrentUserCompanyId", userdto.CompanyDto.Id.ToString()));
            }
            incomingPrincipal.Identities.First().AddClaim(new Claim("CurrentUsername", currentUsername));
            var strRolesBuilder =mergStringList(roles);
            incomingPrincipal.Identities.First().AddClaim(new Claim("CurrentUserRoles", strRolesBuilder.ToString()));

          //  if (isCurrentUserSameAsLogonUser())
            addUpdateLogonUser(incomingPrincipal);

            currentUserActions = getCurrentUserActions(incomingPrincipal);
            var strActionsbuilder = mergStringList(currentUserActions);
            incomingPrincipal.Identities.First().AddClaim(new Claim("CurrentUserActions", strActionsbuilder.ToString()));
            
           

            return incomingPrincipal;
        }

        //private void CheckIsValidWorkListCurrentUser(string logonUsername,string currentUsername)
        //{
        //    var securityService = SecurityServiceFacadeFactory.Create();
        //    try
        //    {
        //        var workListUsernames = securityService.GetPermittedWorkListUserNameFor(logonUsername);
        //        if (logonUsername.ToLower() != currentUsername.ToLower()
        //            && !workListUsernames.Contains(currentUsername))
        //            throw new PMSSecurityException(" دسترسی به کارتابل کاربر " + currentUsername + " مجاز برای کاربر لاگین مجاز نمی باشد ");

        //    }
        //    finally
        //    {
        //        SecurityServiceFacadeFactory.Release(securityService);
        //    }
        //}
        private bool isCurrentUserSameAsLogonUser()
        {
            if (HttpContext.Current.Request.QueryString.AllKeys.Contains("CurrentWorkListUserName"))
                return false;
            else
                return true;
        }

        private void addUpdateLogonUser(ClaimsPrincipal  incomingPrincipal)
        {
            var securityService = SecurityFacadeServiceFactory.Create();
          //  try
          //  {
                securityService.AddUpdateUser(incomingPrincipal);
          //  }
          //  finally
          //  {
            //    SecurityFacadeServiceFactory.Release(securityService);
           // }
        }

        private List<string> getCurrentUserActions(ClaimsPrincipal incomingPrincipal)
        {
            var currentUserActions = new List<string>();
            var securityService = SecurityFacadeServiceFactory.Create();
            try
            {
                var actions = securityService.GetUserAuthorizedActions(incomingPrincipal);
                if (actions != null && actions.Any())
                {
                    actions.ForEach(c => currentUserActions.Add(c.Name));
                    //currentUserActions = actions.Where(c=>c.)Select(c=>c.).ToList();
                }
            }
            finally
            {
                SecurityFacadeServiceFactory.Release(securityService);
            }
            return currentUserActions;
        }

        private StringBuilder mergStringList(IEnumerable<string> listString)
        {
            var builder = new StringBuilder();
            foreach (var str in listString)
            {
                builder.Append(str + ",");
            }

            if (builder.Length > 0)
                builder.Remove(builder.Length - 1, 1);
            return builder;
        }
    }
}
