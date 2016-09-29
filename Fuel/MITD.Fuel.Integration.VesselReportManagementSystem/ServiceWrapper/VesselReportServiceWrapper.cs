using System;
using System.Collections.Generic;
using System.Configuration;
using MITD.Fuel.ACL.Inventory;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;

namespace MITD.Fuel.Integration.VesselReportManagementSystem.ServiceWrapper
{
    public class VesselReportServiceWrapper
    {
        private string vesselReportAddressController;

        private Dictionary<string, string> httpHeaders;
      
        public VesselReportServiceWrapper()
        {

            vesselReportAddressController = ConfigurationManager.AppSettings["FuelApi"] + "apiarea/Fuel/FuelReport";
  
            httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("Authorization", "SAML " + SSOTokenManager.Token.Value);
        }

        public void Add(Action<ResultFuelReportDto, Exception> action, FuelReportCommandDto fuelReportCommandDto)
        {

            var uri = vesselReportAddressController;
            WebClientHelper.Post(new Uri(uri, UriKind.Absolute), action, fuelReportCommandDto, WebClientHelper.MessageFormat.Json, httpHeaders);
        }

       

    }
}
