using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Mvc;
using MITD.Fuel.Service.Host.Infrastructure;
using Thinktecture.IdentityModel.Tokens.Http;

namespace MITD.Fuel.Service.Host.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            /*
             ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(
    delegate
    {
        return true;
    });

             System.Net.ServicePointManager.ServerCertificateValidationCallback +=
    (se, cert, chain, sslerror) =>
        {
            return true;
        };
             System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };

             */


            var authConfig = new AuthenticationConfiguration
            {
                RequireSsl = false,
                ClaimsAuthenticationManager = new ClaimsTransformer(),
                EnableSessionToken = true,
                //SessionToken = new SessionTokenConfiguration()
                //               {
                //                   EndpointAddress = "apiArea/fuel/token"
                //               }
            };
            var registry = new ConfigurationBasedIssuerNameRegistry();
            registry.AddTrustedIssuer(System.Configuration.ConfigurationManager.AppSettings["SigningThumbPrint"], System.Configuration.ConfigurationManager.AppSettings["IssuerURI"]);
            var handlerConfig = new SecurityTokenHandlerConfiguration();
            handlerConfig.AudienceRestriction.AllowedAudienceUris.Add(new Uri(System.Configuration.ConfigurationManager.AppSettings["AudianceUri"]));
            handlerConfig.IssuerNameRegistry = registry;
            handlerConfig.CertificateValidator = X509CertificateValidator.None;
            handlerConfig.ServiceTokenResolver = new X509CertificateStoreTokenResolver(StoreName.My, StoreLocation.LocalMachine);
            handlerConfig.MaxClockSkew = TimeSpan.FromHours(6);
            handlerConfig.SaveBootstrapContext = true;
            authConfig.AddSaml2(handlerConfig, AuthenticationOptions.ForAuthorizationHeader("SAML"), AuthenticationScheme.SchemeOnly("SAML"));

            config.MessageHandlers.Add(new AuthenticationHandler(authConfig));


            config.Routes.MapHttpRoute(
               name: "ApplyActionOnAllFuelReportsRoute",
               routeTemplate: "apiArea/{area}/BatchWorkflow/{id}",
               defaults: new
               {
                   controller = "BatchWorkflow",
                   action = "ApplyBatch"
               }
           );

            config.Routes.MapHttpRoute(
              name: "TransactionDetailPriceRoute",
              routeTemplate: "apiArea/{area}/InventoryTransaction/{id}/Detail/{detailId}/Price/{detailPriceId}",
              defaults: new
              {
                  controller = "InventoryTransactionDetailPrice",
                  detailPriceId = RouteParameter.Optional
              }
          );

            config.Routes.MapHttpRoute(
              name: "TransactionDetailRoute",
              routeTemplate: "apiArea/{area}/InventoryTransaction/{id}/Detail/{detailId}",
              defaults: new
              {
                  controller = "InventoryTransactionDetail",
                  detailId = RouteParameter.Optional
              }
          );

            config.Routes.MapHttpRoute(
               name: "TransactionPricingRoute",
               routeTemplate: "apiArea/{area}/InventoryTransaction/{id}/TransactionPricing",
               defaults: new
               {
                   controller = "InventoryTransaction",
                   id = RouteParameter.Optional,
                   action = "TransactionPricing"
               }
           );
            
            config.Routes.MapHttpRoute(
               name: "RevertFuelReportInventoryOperationsRoute",
               routeTemplate: "apiArea/{area}/FuelReport/{id}/Revert",
               defaults: new
               {
                   controller = "FuelReportRevert",
               }
           );
            config.Routes.MapHttpRoute(
               name: "UpdateVoyagesFromRotationDataRoute",
               routeTemplate: "apiArea/{area}/Voyage/{id}/UpdateVoyagesFromRotationData",
               defaults: new
               {
                   controller = "Voyage",
                   id = RouteParameter.Optional,
                   action = "UpdateVoyagesFromRotationData",
               }
           );

            config.Routes.MapHttpRoute(
               name: "TransactionRegisterVoucherRoute",
               routeTemplate: "apiArea/{area}/InventoryTransaction/{id}/RegisterVoucher",
               defaults: new
               {
                   controller = "InventoryTransaction",
                   id = RouteParameter.Optional,
                   action = "RegisterVoucher"
               }
           );

            config.Routes.MapHttpRoute(
                name: "TransactionRoute",
                routeTemplate: "apiArea/{area}/InventoryTransaction/{id}",
                defaults: new
                {
                    controller = "InventoryTransaction",
                    id = RouteParameter.Optional
                }
            );


            config.Routes.MapHttpRoute(
               name: "CurrentUserRoute",
               routeTemplate: "apiArea/{area}/Users/{id}/Current",
               defaults: new
               {
                   controller = "CurrentUser",
                   id = RouteParameter.Optional,
                   action = "GetCurrentFuelUser"
               }
           );

            config.Routes.MapHttpRoute(
             name: "CurrentUserCompanyIdRoute",
             routeTemplate: "apiArea/{area}/Users/{id}/CurrentCompanyId",
             defaults: new
             {
                 controller = "CurrentUser",
                 id = RouteParameter.Optional,
                 action = "GetCurrentFuelUserCompanyId"
             }
         );
            config.Routes.MapHttpRoute(
             name: "CurrentUserAccessToHoldingRoute",
             routeTemplate: "apiArea/{area}/Users/{id}/CurrentUserAccessToHolding",
             defaults: new
             {
                 controller = "CurrentUser",
                 id = RouteParameter.Optional,
                 action = "GetCurrentFuelUserAccessToHolding"
             }
         );

            config.Routes.MapHttpRoute(
         name: "ActionTypes",
         routeTemplate: "apiArea/{area}/ActionTypes/{id}",
         defaults: new
         {
             controller = "ActionTypes",
             id = RouteParameter.Optional,

         }
     );

         
            config.Routes.MapHttpRoute(
          name: "User",
          routeTemplate: "apiArea/{area}/User/{id}",
          defaults: new
          {
              controller = "Users",
              id = RouteParameter.Optional,
             
          }
      );


            config.Routes.MapHttpRoute(
      name: "UserGroups",
      routeTemplate: "apiArea/{area}/UserGroups/{id}",
      defaults: new
      {
          controller = "UserGroups",
          id = RouteParameter.Optional,

      }
  );


            config.Routes.MapHttpRoute(
               name: "CompanyRoute",
               routeTemplate: "apiArea/{area}/InventoryCompany/{id}",
               defaults: new
               {
                   controller = "InventoryCompany",
                   id = RouteParameter.Optional
               }
           );

            config.Routes.MapHttpRoute(
               name: "WarehouseRoute",
               routeTemplate: "apiArea/{area}/InventoryCompany/{id}/Warehouse/{warehouseId}",
               defaults: new
               {
                   controller = "InventoryWarehouse",
                   warehouseId = RouteParameter.Optional
               }
           );

            config.Routes.MapHttpRoute(
                name: "OffhirePricingValueRoute",
                routeTemplate: "apiArea/{area}/OffhirePricingValue/",
                defaults: new
                {
                    controller = "OffhirePricingValue"
                }
            );

            //{startDateTime:datetime:regex(\d{4}\d{2}\d{2} \d{2}\d{2}\d{2})}

            //{startDateTime:datetime:regex(\\d{4}\\d{2}\\d{2}\\d{2}\\d{2}\\d{2})}

            //     [Route("date/{pubdate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
            //[Route("date/{*pubdate:datetime:regex(\\d{4}/\\d{2}/\\d{2})}")]

            config.Routes.MapHttpRoute(
                name: "OffhireManagementSystemPreparedDataRoute",
                routeTemplate: "apiArea/{area}/OffhireManagementSystem/{referenceNumber}/PreparedData/{introducerId}",
                defaults: new
                {
                    controller = "OffhireManagementSystemPreparedData"
                }
            );

            config.Routes.MapHttpRoute(
                name: "OffhireManagementSystemRoute",
                routeTemplate: "apiArea/{area}/OffhireManagementSystem/{referenceNumber}",
                defaults: new
                {
                    controller = "OffhireManagementSystem",
                    referenceNumber = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "OffhireDetailRoute",
                routeTemplate: "apiArea/{area}/Offhire/{id}/Detail/{detailId}",
                defaults: new
                {
                    controller = "OffhireDetail",
                    detailId = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "OffhireRoute",
                routeTemplate: "apiArea/{area}/Offhire/{id}",
                defaults: new
                {
                    controller = "Offhire",
                    vesselId = RouteParameter.Optional
                }
            );




            config.Routes.MapHttpRoute(
                name: "CompanyOwnedVesselRoute",
                routeTemplate: "apiArea/{area}/Company/{id}/OwnedVessel/{vesselId}",
                defaults: new
                {
                    controller = "CompanyOwnedVessel",
                    vesselId = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
               name: "ScrapInventoryOperationRoute",
               routeTemplate: "apiArea/{area}/Scrap/{id}/InventoryOperation/{operationId}",
               defaults: new
               {
                   controller = "ScrapInventoryOperation",
                   operationId = RouteParameter.Optional
               }
            );

            config.Routes.MapHttpRoute(
                name: "ScrapDetailRoute",
                routeTemplate: "apiArea/{area}/Scrap/{id}/Detail/{detailId}",
                defaults: new
                {
                    controller = "ScrapDetail",
                    detailId = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "ScrapRoute",
                routeTemplate: "apiArea/{area}/Scrap/{id}",
                defaults: new
                {
                    controller = "Scrap",
                    id = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
               name: "VoyageLogRoute",
               routeTemplate: "apiArea/{area}/Voyage/{voyageId}/Log/",
               defaults: new
               {
                   controller = "VoyageLog",
               }
            );

            config.Routes.MapHttpRoute(
               name: "VoyageRoute",
               routeTemplate: "apiArea/{area}/Voyage/{id}",
               defaults: new
               {
                   controller = "Voyage",
                   id = RouteParameter.Optional
               }
            );

            config.Routes.MapHttpRoute(
               name: "FuelReportCharterPreparedDataRoute",
               routeTemplate: "apiArea/{area}/FuelReport/{id}/CharterPreparedData/",
               defaults: new
               {
                   controller = "FuelReportCharterPreparedData",
               }
            );

            config.Routes.MapHttpRoute(
               name: "FuelReportInventoryResultRoute",
               routeTemplate: "apiArea/{area}/FuelReport/{id}/InventoryResult/",
               defaults: new
               {
                   controller = "FuelReportInventoryResult",
               }
            );

            config.Routes.MapHttpRoute(
               name: "FuelReportDetailInventoryOperationRoute",
               routeTemplate: "apiArea/{area}/FuelReport/{id}/Detail/{detailId}/InventoryOperation/{operationId}",
               defaults: new
               {
                   controller = "FuelReportDetailInventoryOperation",
                   operationId = RouteParameter.Optional
               }
            );

            config.Routes.MapHttpRoute(
               name: "FuelReportInventoryOperationRoute",
               routeTemplate: "apiArea/{area}/FuelReport/{id}/InventoryOperation/{operationId}",
               defaults: new
               {
                   controller = "FuelReportInventoryOperation",
                   operationId = RouteParameter.Optional
               }
            );

            config.Routes.MapHttpRoute(
                name: "FuelReportDetailRoute",
                routeTemplate: "apiArea/{area}/FuelReport/{id}/Detail/{detailId}",
                defaults: new
                {
                    controller = "FuelReportDetail",
                    detailId = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "FuelReportRoute",
                routeTemplate: "apiArea/{area}/FuelReport/{id}",
                defaults: new
                {
                    controller = "FuelReport",
                    id = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "OrderItemRoute",
                routeTemplate: "apiArea/{area}/Order/{id}/OrderItem/{orderItemId}",
                defaults: new
                {
                    controller = "OrderItem",
                    orderItemId = RouteParameter.Optional
                }
            );


            config.Routes.MapHttpRoute(
                name: "InvoiceItemRoute",
                routeTemplate: "apiArea/{area}/Invoice/{id}/InvoiceItem/{invoiceItemId}",
                defaults: new
                {
                    controller = "InvoiceItem",
                    invoiceItemId = RouteParameter.Optional
                }
            );


            config.Routes.MapHttpRoute(
                name: "MainUnitValue",
                routeTemplate: "apiArea/{area}/MainUnit/",
                defaults: new
                {
                    controller = "OrderItem",
                });

            config.Routes.MapHttpRoute(
                name: "CharterItemRoute",
                routeTemplate: "apiArea/{area}/Charter/{id}/CharterItem/{charterItemId}",
                defaults: new
                {
                    controller = "CharterItem",
                    charterItemId = RouteParameter.Optional
                }
                );


            config.Routes.MapHttpRoute(
              name: "VoucherRoute",
              routeTemplate: "apiArea/{area}/Voucher/{id}",
              defaults: new
              {
                  controller = "Voucher",
                  id = RouteParameter.Optional
              }
          );


            config.Routes.MapHttpRoute(
                name: "VoucherSeting",
                routeTemplate: "apiArea/{area}/VoucherSeting/{id}",
                defaults: new
                          {
                              controller = "VoucherSeting",
                              id = RouteParameter.Optional
                          }
                );

            config.Routes.MapHttpRoute(
            name: "VoucherSetingDetail",
            routeTemplate: "apiArea/{area}/VoucherSetingDetail/{id}",
            defaults: new
            {
                controller = "VoucherSetingDetail",
                id = RouteParameter.Optional
            }
            );


            config.Routes.MapHttpRoute(
                name: "AccountRoute",
                routeTemplate: "apiArea/{area}/Account/{id}",
                defaults: new
                {
                    controller = "Account",
                    id = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
               name: "RefreshFuelReportVoyagesRoute",
               routeTemplate: "apiArea/{area}/RefreshFuelReportsVoyage",
               defaults: new
               {
                   controller = "RefreshFuelReportsVoyage",

               }
            );



            config.Routes.MapHttpRoute(
             name: "Attachment",
             routeTemplate: "apiArea/{area}/Attachment/{id}",
             defaults: new
             {
                 controller = "Attachment",
                 id = RouteParameter.Optional
             }
         );
            config.Routes.IgnoreRoute("Uploader", "{handler}.ashx");


            config.Routes.MapHttpRoute(
                name: "Default_apiArea",
                routeTemplate: "apiArea/{area}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                      name: "apiAreaWithAction",
                      routeTemplate: "apiArea/{area}/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                  );

            config.Routes.MapHttpRoute(
                             name: "Default_areaApi",

                routeTemplate: "apiArea/{area}/{controller}/{action}/{id}",
                // "apiArea/Order/{id}/OrderDetail/{DetailId}",
                defaults: new { action = "UpdateOrderItem", id = RouteParameter.Optional }
                         );

            config.Routes.MapHttpRoute(
                  name: "DefaultApi",
                  routeTemplate: "api/{controller}/{id}",
                  defaults: new
                  {
                      id = RouteParameter.Optional
                  }
              );

            config.Filters.Add(DependencyResolver.Current.GetService<GlobalExceptionHandlingAttribute>());
        }
    }
}
