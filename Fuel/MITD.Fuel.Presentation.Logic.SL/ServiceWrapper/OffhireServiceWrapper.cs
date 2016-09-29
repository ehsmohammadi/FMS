using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Presentation.Contracts;
using ApiServiceAddressHelper = MITD.Fuel.Presentation.Contracts.SL.Infrastructure.ApiServiceAddressHelper;

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public class OffhireServiceWrapper : IOffhireServiceWrapper
    {
        private readonly string offhireAddressFormatString;

        private readonly string offhireDetailAddressFormatString;

        private readonly string offhireOffhireManagementSystemAddressFormatString;

        private readonly string offhireManagementSystemPreparedDataAddressFormatString;

        private readonly string offhirePricingValueAddressFormatString;


        public OffhireServiceWrapper()
        {
            this.offhireAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/Offhire/{0}");

            this.offhireDetailAddressFormatString = string.Concat(this.offhireAddressFormatString, "/Detail/{1}");

            this.offhireOffhireManagementSystemAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/OffhireManagementSystem/{0}");

            this.offhireManagementSystemPreparedDataAddressFormatString = string.Concat(this.offhireOffhireManagementSystemAddressFormatString, "/PreparedData/{1}");

            this.offhirePricingValueAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/OffhirePricingValue/");

        }

        //================================================================================

        public void GetById(Action<OffhireDto, Exception> action, long id)
        {
            var url = string.Format(this.offhireAddressFormatString, id);

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(url), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void GetPagedOffhireData(Action<PageResultDto<OffhireDto>, Exception> action, int pageSize, int pageIndex)
        {
            var url = string.Format(this.offhireAddressFormatString, string.Empty);

            var sbUrl = new StringBuilder(url);
            sbUrl.Append(string.Concat("?pageSize=", pageSize));
            sbUrl.Append(string.Concat("&pageIndex=", pageIndex));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sbUrl), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void GetPagedOffhireDataByFilter(Action<PageResultDto<OffhireDto>, Exception> action, long? companyId, long? vesselId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex)
        {
            var url = string.Format(this.offhireAddressFormatString, string.Empty);

            var sbUrl = new StringBuilder(url);
            sbUrl.Append(string.Concat("?companyId=", companyId));
            sbUrl.Append(string.Concat("&vesselId=", vesselId));
            sbUrl.Append(string.Concat("&fromDate=", HttpUtil.DateTimeToString(fromDate)));
            sbUrl.Append(string.Concat("&toDate=", HttpUtil.DateTimeToString(toDate)));
            sbUrl.Append(string.Concat("&pageSize=", pageSize));
            sbUrl.Append(string.Concat("&pageIndex=", pageIndex));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sbUrl), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void GetOffhirePreparedData(Action<OffhireDto, Exception> action, long referenceNumber, long introducerId)
        {
            var url = string.Format(this.offhireManagementSystemPreparedDataAddressFormatString, referenceNumber, introducerId);

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(url), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void GetOffhireManagementSystemPagedData(Action<PageResultDto<OffhireManagementSystemDto>, Exception> action, long companyId, long vesselId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex)
        {
            var url = string.Format(this.offhireOffhireManagementSystemAddressFormatString, string.Empty);

            var sbUrl = new StringBuilder(url);
            sbUrl.Append(string.Concat("?companyId=", companyId));
            sbUrl.Append(string.Concat("&vesselId=", vesselId));
            sbUrl.Append(string.Concat("&fromDate=", HttpUtil.DateTimeToString(fromDate)));
            sbUrl.Append(string.Concat("&toDate=", HttpUtil.DateTimeToString(toDate)));
            sbUrl.Append(string.Concat("&pageSize=", pageSize));
            sbUrl.Append(string.Concat("&pageIndex=", pageIndex));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sbUrl), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void AddOffhire(Action<OffhireDto, Exception> action, OffhireDto dto)
        {
            var url = string.Format(this.offhireAddressFormatString, string.Empty);

            WebClientHelper.Post(ApiServiceAddressHelper.BuildUri(url), action, dto, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void UpdateOffhire(Action<OffhireDto, Exception> action, OffhireDto dto)
        {
            var url = string.Format(this.offhireAddressFormatString, dto.Id);

            WebClientHelper.Put(ApiServiceAddressHelper.BuildUri(url), action, dto, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void Delete(Action<string, Exception> action, long id)
        {
            var url = string.Format(this.offhireAddressFormatString, id);

            WebClientHelper.Delete(ApiServiceAddressHelper.BuildUri(url), action);
        }

        //================================================================================

        public void GetPagedOffhireDetailData(Action<PageResultDto<OffhireDetailDto>, Exception> action, long offhireId, int pageSize, int pageIndex)
        {
            var url = string.Format(this.offhireDetailAddressFormatString, offhireId, string.Empty);

            var sbUrl = new StringBuilder(url);
            sbUrl.Append(string.Concat("?pageSize=", pageSize));
            sbUrl.Append(string.Concat("&pageIndex=", pageIndex));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sbUrl), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void GetOffhireDetail(Action<OffhireDetailDto, Exception> action, long offhireId, long offhireDetailId)
        {
            var url = string.Format(this.offhireDetailAddressFormatString, offhireId, offhireDetailId);

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(url), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void GetPricingValues(Action<List<PricingValueDto>, Exception> action, long introducerId, long vesselId, DateTime startDateTime)
        {
            var url = this.offhirePricingValueAddressFormatString +
                "?introducerId=" + introducerId +
                "&vesselId=" + vesselId +
                "&startDateTime=" + HttpUtil.DateTimeToString(startDateTime);

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(url), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void AddOffhireDetail(Action<OffhireDetailDto, Exception> action, OffhireDetailDto detailDto)
        {
            var url = string.Format(this.offhireDetailAddressFormatString, string.Empty);

            WebClientHelper.Post(ApiServiceAddressHelper.BuildUri(url), action, detailDto, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void UpdateOffhireDetail(Action<OffhireDetailDto, Exception> action, OffhireDetailDto detailDto)
        {
            var url = string.Format(this.offhireDetailAddressFormatString, detailDto.Offhire.Id, detailDto.Id);

            WebClientHelper.Put(ApiServiceAddressHelper.BuildUri(url), action, detailDto, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void DeleteOffhireDetail(Action<string, Exception> action, long offhireId, long offhireDetailId)
        {
            var url = string.Format(this.offhireDetailAddressFormatString, offhireId, offhireDetailId);

            WebClientHelper.Delete(ApiServiceAddressHelper.BuildUri(url), action, ApiConfig.Headers);
        }

        //================================================================================
    }
}