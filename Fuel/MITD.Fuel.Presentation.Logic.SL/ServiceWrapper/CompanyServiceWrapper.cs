using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.FuelApp.Logic.SL.ServiceWrapper
{
    public class CompanyServiceWrapper : ICompanyServiceWrapper
    {
        private readonly string companyAddressFormatString;
        private readonly string companyOwnedVesselsAddressFormatString;

        public CompanyServiceWrapper()
        {
            companyAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/Company/{0}");
            companyOwnedVesselsAddressFormatString = string.Concat(companyAddressFormatString, "/OwnedVessel/{1}");
        }

        #region methods

        public void GetAll(Action<PageResultDto<CompanyDto>, Exception> action, string methodName = null)
        {
            var url = string.Format(companyAddressFormatString, string.Empty);

            WebClientHelper.Get<PageResultDto<CompanyDto>>(new Uri(url, UriKind.Absolute),
                                                                    (res, exp) => action(res, exp),
                                                                    WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GetById(Action<CompanyDto, Exception> action, int id)
        {
            var url = string.Format(companyAddressFormatString, id);

            WebClientHelper.Get<CompanyDto>(new Uri(url, UriKind.Absolute),
                                                     (res, exp) => action(res, exp),
                                                     WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GetOwnedVessels(Action<PageResultDto<VesselInCompanyDto>, Exception> action, long companyId)
        {
            var url = string.Format(companyOwnedVesselsAddressFormatString, companyId, string.Empty);

            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GetAll(Action<List<CompanyDto>, Exception> action, bool filterByUser)
        {
            var url = string.Format(companyAddressFormatString, string.Empty)
               + "?filterByUser=" + filterByUser;

            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void GetAll(Action<List<CompanyDto>, Exception> action, bool filterByUser, bool operatedVessels)
        {
            var url = string.Format(companyAddressFormatString, string.Empty)
               + "?filterByUser=" + filterByUser + "&operatedVessels=" + operatedVessels;

            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        #endregion

    }
}
