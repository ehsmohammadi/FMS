using System;
using System.IO;
using System.Text;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public class ScrapServiceWrapper : IScrapServiceWrapper
    {
        private readonly string scrapAddressFormatString;

        private readonly string scrapDetailAddressFormatString;

        public ScrapServiceWrapper()
        {
            this.scrapAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/Scrap/{0}");

            this.scrapDetailAddressFormatString = string.Concat(this.scrapAddressFormatString, "/Detail/{1}");
        }

        //================================================================================

        public void GetById(Action<ScrapDto, Exception> action, long id)
        {
            var url = string.Format(this.scrapAddressFormatString, id);

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(url), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void GetPagedScrapData(Action<PageResultDto<ScrapDto>, Exception> action, int pageSize, int pageIndex)
        {
            var url = string.Format(this.scrapAddressFormatString, string.Empty);

            var sbUrl = new StringBuilder(url);
            sbUrl.Append(string.Concat("?pageSize=", pageSize));
            sbUrl.Append(string.Concat("&pageIndex=", pageIndex));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sbUrl), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void GetPagedScrapDataByFilter(Action<PageResultDto<ScrapDto>, Exception> action, long? companyId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex)
        {
            var url = string.Format(this.scrapAddressFormatString, string.Empty);

            var sbUrl = new StringBuilder(url);
            sbUrl.Append(string.Concat("?companyId=", companyId));
            sbUrl.Append(string.Concat("&fromDate=", HttpUtil.DateTimeToString(fromDate)));
            sbUrl.Append(string.Concat("&toDate=", HttpUtil.DateTimeToString(toDate)));
            sbUrl.Append(string.Concat("&pageSize=", pageSize));
            sbUrl.Append(string.Concat("&pageIndex=", pageIndex));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sbUrl), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void AddScrap(Action<ScrapDto, Exception> action, ScrapDto dto)
        {
            var url = string.Format(this.scrapAddressFormatString, string.Empty);

            WebClientHelper.Post(ApiServiceAddressHelper.BuildUri(url), action, dto, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void UpdateScrap(Action<ScrapDto, Exception> action, ScrapDto dto)
        {
            var url = string.Format(this.scrapAddressFormatString, dto.Id);

            WebClientHelper.Put(ApiServiceAddressHelper.BuildUri(url), action, dto, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void Delete(Action<string, Exception> action, long id)
        {
            var url = string.Format(this.scrapAddressFormatString, id);

            WebClientHelper.Delete(ApiServiceAddressHelper.BuildUri(url), action);
        }

        //================================================================================

        public void GetPagedScrapDetailData(Action<PageResultDto<ScrapDetailDto>, Exception> action, long scrapId, int pageSize, int pageIndex)
        {
            var url = string.Format(this.scrapDetailAddressFormatString, scrapId, string.Empty);

            var sbUrl = new StringBuilder(url);
            sbUrl.Append(string.Concat("?pageSize=", pageSize));
            sbUrl.Append(string.Concat("&pageIndex=", pageIndex));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sbUrl), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void GetScrapDetail(Action<ScrapDetailDto, Exception> action, long scrapId, long scrapDetailId)
        {
            var url = string.Format(this.scrapDetailAddressFormatString, scrapId, scrapDetailId);

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(url), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void AddScrapDetail(Action<ScrapDetailDto, Exception> action, long scrapId, ScrapDetailDto detailDto)
        {
            var url = string.Format(this.scrapDetailAddressFormatString, scrapId, string.Empty);

            WebClientHelper.Post(ApiServiceAddressHelper.BuildUri(url), action, detailDto, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void UpdateScrapDetail(Action<ScrapDetailDto, Exception> action, ScrapDetailDto detailDto)
        {
            var url = string.Format(this.scrapDetailAddressFormatString, detailDto.Scrap.Id, detailDto.Id);

            WebClientHelper.Put(ApiServiceAddressHelper.BuildUri(url), action, detailDto, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void DeleteScrapDetail(Action<string, Exception> action, long scrapId, long scrapDetailId)
        {
            var url = string.Format(this.scrapDetailAddressFormatString, scrapId, scrapDetailId);

            WebClientHelper.Delete(ApiServiceAddressHelper.BuildUri(url), action, ApiConfig.Headers);
        }

        //================================================================================
    }
}