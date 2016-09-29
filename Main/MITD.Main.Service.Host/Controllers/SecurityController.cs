using System;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MITD.Main.Service.Host.Controllers
{
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class SecurityController : Controller
    {
        //
        // GET: /Security/

        public ActionResult Index()
        {
            var result = "";
            var context = (this.User.Identity as ClaimsIdentity).BootstrapContext as BootstrapContext;
            if (context != null)
            {
                if (context.Token != null)
                    result = context.Token;
                else
                {
                    StringBuilder sb = new StringBuilder();
                    using (var writer = XmlWriter.Create(sb))
                    {
                        context.SecurityTokenHandler.WriteToken(writer, context.SecurityToken);
                    }
                    result = sb.ToString();
                }

            }
            else
            {
                var message = FederatedAuthentication.WSFederationAuthenticationModule.CreateSignInRequest("passive", this.Request.RawUrl, false);
                return new RedirectResult(message.RequestUrl);
            }
            return this.Content(result, "application/xml", Encoding.UTF8);
        }

        public ActionResult LogOut()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                WSFederationAuthenticationModule authModule = FederatedAuthentication.WSFederationAuthenticationModule;
                string signoutUrl = (WSFederationAuthenticationModule.GetFederationPassiveSignOutUrl(authModule.Issuer, authModule.Realm, null));
                var baseUrl = VirtualPathUtility.AppendTrailingSlash(this.Request.Url.GetLeftPart(UriPartial.Authority) + this.Request.ApplicationPath);
                WSFederationAuthenticationModule.FederatedSignOut(new Uri(authModule.Issuer), new Uri(baseUrl + "FMS.aspx"));
            }


            //A.H : Added by Hatefi to fix the problem of Successive log-ins, but it didn't solve the problem.
            /*
            if (User.Identity.IsAuthenticated)
            {
                FederatedAuthentication.SessionAuthenticationModule.SignOut();


                var baseUrl = VirtualPathUtility.AppendTrailingSlash(this.Request.Url.GetLeftPart(UriPartial.Authority) + this.Request.ApplicationPath);


                var signOutRequest =
                    new SignOutRequestMessage(new Uri(
                                                  FederatedAuthentication.WSFederationAuthenticationModule.Issuer))
                    {
                        Reply = baseUrl + "FMS.aspx"
                    };
                Response.Redirect(signOutRequest.WriteQueryString());
            }
            */
            return new EmptyResult();

        }


    }
}
