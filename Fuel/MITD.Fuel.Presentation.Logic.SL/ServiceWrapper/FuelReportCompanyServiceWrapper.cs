using System;
using System.Collections.Generic;
using System.Windows;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation.Contracts;
using System.IO;

namespace MITD.Fuel.Presentation.FuelApp.Logic.SL.ServiceWrapper
{
    public class FuelReportCompanyServiceWrapper : IFuelReportCompanyServiceWrapper
    {
        #region fields

        private string fuelReportCompanyAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/FuelReportCompany/{0}");

        #endregion

        #region methods

        public void GetAll(Action<List<CompanyDto>, Exception> action, long userId)
        {
            var url = string.Format(fuelReportCompanyAddressFormatString, string.Empty)
                + "?userId=" + userId;

            WebClientHelper.Get<List<CompanyDto>>(new Uri(url, UriKind.Absolute),
                                                                    (res, exp) => action(res, exp),
                                                                    WebClientHelper.MessageFormat.Json,ApiConfig.Headers
                );
        }

        public void GetById(Action<CompanyDto, Exception> action, int id)
        {
            var url = string.Format(fuelReportCompanyAddressFormatString, id);
            WebClientHelper.Get<CompanyDto>(new Uri(url, UriKind.Absolute),
                                                     (res, exp) => action(res, exp),
                                                     WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //public void Add(Action<CompanyDto, Exception> action, CompanyDto ent)
        //{
        //    var url = string.Concat(fuelReportCompanyAddressFormatString, HttpVerb.POST);
        //    WebClientHelper.Post<CompanyDto, CompanyDto>(new Uri(url, UriKind.Absolute),
        //                                                                   (res, exp) => action(res, exp), ent,
        //                                                                   WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        //}

        //public void Update(Action<CompanyDto, Exception> action, CompanyDto ent)
        //{
        //    var url = string.Concat(fuelReportCompanyAddressFormatString, HttpVerb.PUT + "/" + ent.Id);
        //    WebClientHelper.Put<CompanyDto, CompanyDto>(new Uri(url, UriKind.Absolute),
        //                                                                  (res, exp) => action(res, exp), ent,
        //                                                                  WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        //}

        //public void Delete(Action<string, Exception> action, int id)
        //{
        //    var url = string.Concat(fuelReportCompanyAddressFormatString, HttpVerb.DELETE + "/" + id);// "/DeleteById?id=", id);
        //    WebClientHelper.Delete(new Uri(url, UriKind.Absolute), (res, exp) => action(res, exp));
        //}

        #endregion


        public void GetAll(Action<List<CompanyDto>, Exception> action, bool filterByUser)
        {
            var url = string.Format(fuelReportCompanyAddressFormatString, string.Empty)
               + "?filterByUser=" + filterByUser;

            WebClientHelper.Get(new Uri(url, UriKind.Absolute),action,WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }
    }
}
