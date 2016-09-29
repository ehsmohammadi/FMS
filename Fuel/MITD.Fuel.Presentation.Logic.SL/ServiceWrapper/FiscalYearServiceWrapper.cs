#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Presentation.Contracts;

#endregion

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public class FiscalYearServiceWrapper : IFiscalYearServiceWrapper
    {
        private readonly string fiscalYearsAddressFormatString;
        
        public FiscalYearServiceWrapper()
        {
            fiscalYearsAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/FiscalYear/{0}");
        }

        public void GetFiscalYears(Action<List<FiscalYearDto>, Exception> action)
        {
            string url = string.Format(fiscalYearsAddressFormatString, string.Empty);

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(url), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }
    }
}