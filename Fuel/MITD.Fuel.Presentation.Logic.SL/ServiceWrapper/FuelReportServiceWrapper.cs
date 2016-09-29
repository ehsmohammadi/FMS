using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation.Contracts;
using System.IO;
using MITD.Fuel.Presentation.Contracts.DTOs.Report;

namespace MITD.Fuel.Presentation.FuelApp.Logic.SL.ServiceWrapper
{
    public class FuelReportServiceWrapper : IFuelReportServiceWrapper
    {
        #region fields

        private string fuelReportAddressFormatString;

        private string fuelReportDetailAddressFormatString;

        private string currencyAddressFormatString;

        private string refreshFuelReportsVoyageAddressFormatString;

        private string vesselEventDataFormatString;

        private string fuelReportRevertAddressFormatString;

        #endregion

        #region methods

        public FuelReportServiceWrapper()
        {
            fuelReportAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/FuelReport/{0}");

            fuelReportDetailAddressFormatString = string.Concat(fuelReportAddressFormatString, "/Detail/{1}");

            currencyAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/Currency/{0}");

            refreshFuelReportsVoyageAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/RefreshFuelReportsVoyage");

            vesselEventDataFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/VesselEventData/{0}");

            fuelReportRevertAddressFormatString = string.Concat(fuelReportAddressFormatString, "/Revert");

        }

        public void GetByFilter(Action<PageResultDto<FuelReportDto>, Exception> action, long? companyId, long? vesselInCompanyId, string vesselReportCode, DateTime? fromDate, DateTime? toDate, string fuelReportIds, string fuelReportDetailIds, int pageSize, int pageIndex)
        {
            var url = string.Format(fuelReportAddressFormatString, string.Empty);

            var sbUrl = new StringBuilder(url);
            sbUrl.Append(string.Concat("?companyId=", companyId));
            sbUrl.Append(string.Concat("&vesselInCompanyId=", vesselInCompanyId));
            sbUrl.Append(string.Concat("&vesselReportCode=", vesselReportCode));
            sbUrl.Append(string.Concat("&fromDate=", HttpUtil.DateTimeToString(fromDate)));
            sbUrl.Append(string.Concat("&toDate=", HttpUtil.DateTimeToString(toDate)));
            sbUrl.Append(string.Concat("&fuelReportIds=", fuelReportIds));
            sbUrl.Append(string.Concat("&fuelReportDetailIds=", fuelReportDetailIds));
            sbUrl.Append(string.Concat("&pageSize=", pageSize));
            sbUrl.Append(string.Concat("&pageIndex=", pageIndex));

            WebClientHelper.Get<PageResultDto<FuelReportDto>>(new Uri(sbUrl.ToString(), UriKind.Absolute),
                                                                    (res, exp) => action(res, exp),
                                                                    WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void RefreshFuelReportsVoyage(Action<object, Exception> action, long companyId, long? vesselInCompanyId)
        {
            var url = refreshFuelReportsVoyageAddressFormatString + "?companyId=" + companyId + "&vesselInCompanyId=" + vesselInCompanyId;

            WebClientHelper.Put<object,object>(new Uri(url, UriKind.Absolute),action,
                null, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void GetAll(Action<PageResultDto<FuelReportDto>, Exception> action, string methodName, int pageSize, int pageIndex)
        {
            var url = string.Format(fuelReportAddressFormatString, string.Empty)
                + "?PageSize=" + pageSize + "&PageIndex=" + pageIndex;
            WebClientHelper.Get<PageResultDto<FuelReportDto>>(new Uri(url, UriKind.Absolute),
                                                                    (res, exp) => action(res, exp),
                                                                    WebClientHelper.MessageFormat.Json, ApiConfig.Headers
                );
        }

        public void GetById(Action<FuelReportDto, Exception> action, long id, bool includeReferencesLookup = false)
        {
            var url = string.Format(fuelReportAddressFormatString, id);
            url += "?includeReferencesLookup=" + includeReferencesLookup;

            WebClientHelper.Get<FuelReportDto>(new Uri(url, UriKind.Absolute),
                                                     (res, exp) => action(res, exp),
                                                     WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void GetFuelReportDetailById(Action<FuelReportDetailDto, Exception> action, long id, long detailId, bool includeReferencesLookup = false)
        {
            var url = string.Format(fuelReportDetailAddressFormatString, id, detailId);
            url += "?includeReferencesLookup=" + includeReferencesLookup;

            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void Add(Action<FuelReportDto, Exception> action, FuelReportDto ent)
        {
            var url = string.Format(fuelReportAddressFormatString, string.Empty);

            WebClientHelper.Post<FuelReportDto, FuelReportDto>(new Uri(url, UriKind.Absolute),
                                                                           (res, exp) => action(res, exp), ent,
                                                                           WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void Update(Action<FuelReportDto, Exception> action, FuelReportDto ent)
        {
            var url = string.Format(fuelReportAddressFormatString, ent.Id);

            WebClientHelper.Put<FuelReportDto, FuelReportDto>(new Uri(url, UriKind.Absolute),
                                                                          (res, exp) => action(res, exp), ent,
                                                                          WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void Delete(Action<string, Exception> action, long id)
        {
            var url = string.Format(fuelReportAddressFormatString, id);
            WebClientHelper.Delete(new Uri(url, UriKind.Absolute), (res, exp) => action(res, exp), ApiConfig.Headers);
        }

        public void UpdateFuelReportDetail(Action<FuelReportDetailDto, Exception> action, FuelReportDetailDto ent)
        {
            var url = string.Format(fuelReportDetailAddressFormatString, ent.FuelReportId, ent.Id);

            WebClientHelper.Put<FuelReportDetailDto, FuelReportDetailDto>(new Uri(url, UriKind.Absolute),
                                                                          (res, exp) => action(res, exp), ent,
                                                                          WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void GetAllCurrency(Action<List<CurrencyDto>, Exception> action)
        {
            var url = string.Format(currencyAddressFormatString, string.Empty);
            WebClientHelper.Get<List<CurrencyDto>>(new Uri(url, UriKind.Absolute),
                                                                    (res, exp) => action(res, exp),
                                                                    WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void GetVesselEventData(Action<VesselEventReportViewDto, Exception> action, string eventReportCode)
        {
            var url = string.Format(vesselEventDataFormatString, eventReportCode);
            WebClientHelper.Get<VesselEventReportViewDto>(new Uri(url, UriKind.Absolute),
                                                                    (res, exp) => action(res, exp),
                                                                    WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void RevertFuelReportInventoryOperations(Action<object, Exception> action, long fuelReportId)
        {
            var url = string.Format(fuelReportRevertAddressFormatString, fuelReportId);
            WebClientHelper.Put(new Uri(url, UriKind.Absolute),action, fuelReportId, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        #endregion
    }
}