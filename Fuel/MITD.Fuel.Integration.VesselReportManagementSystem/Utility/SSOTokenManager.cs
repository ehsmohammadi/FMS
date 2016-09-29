using System;
using System.Configuration;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityModel.WSTrust;

namespace MITD.Fuel.ACL.Inventory
{
    public static class SSOTokenManager
    {
      
        public static Lazy<string> Token = new Lazy<string>(() =>
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var credentials = new ClientCredentials();
            credentials.UserName.UserName = "frimporter";
            credentials.UserName.Password = "frimporter1!";
            //credentials.ClientCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, "faa68b845f11a2f14592febdbcec119c96a45f42");

            var stsAddress = ConfigurationManager.AppSettings["ida:STSAddress"];
            var realm = ConfigurationManager.AppSettings["ida:Realm"];

            var token = WSTrustClient.Issue(
                new EndpointAddress(stsAddress),
                new EndpointAddress(realm),
                new UserNameWSTrustBinding(SecurityMode.TransportWithMessageCredential), 
                credentials);

            return token.ToTokenXmlString(); 
        });

        // This method returns a custom binding created from a WSHttpBinding. Alter the method 
        // to use the appropriate binding for your service, with the appropriate settings.
        public static Binding CreateCustomBinding(TimeSpan clockSkew)
        {
            //WSHttpBinding standardBinding = new WSHttpBinding(SecurityMode.Message, true);
            var binding = new UserNameWSTrustBinding(SecurityMode.TransportWithMessageCredential);

            CustomBinding myCustomBinding = new CustomBinding(binding);
            TransportSecurityBindingElement security =
                myCustomBinding.Elements.Find<TransportSecurityBindingElement>();

            security.LocalClientSettings.MaxClockSkew = clockSkew;
            security.LocalServiceSettings.MaxClockSkew = clockSkew;

            return myCustomBinding;
        }
    }
}
