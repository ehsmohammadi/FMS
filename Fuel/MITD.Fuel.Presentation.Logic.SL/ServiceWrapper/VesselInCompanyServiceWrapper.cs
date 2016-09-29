using System;
using System.Collections;
using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation.Contracts;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using MITD.Fuel.Presentation.Contracts.Infrastructure;

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public class VesselInCompanyServiceWrapper : IVesselInCompanyServiceWrapper
    {
        #region fields

        private string vesselAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/VesselInCompany/{0}");
        private string vesselActivationInfoAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/VesselActivationInfo/{0}");

        #endregion

        #region methods

        public void GetAll(Action<List<VesselInCompanyDto>, Exception> action, object queryParameters = null /*params Tuple<string, object>[] query*/)
        {
            var url = string.Format(vesselAddressFormatString, string.Empty);

            url += queryParameters != null 
                ? "?" + string.Join("&", queryParameters.GetPropertiesValuesDictionary().Select(p => string.Format("{0}={1}", p.Key, p.Value))) 
                : string.Empty;

            //url += query != null ? "?" + string.Join("&", query.Select(p => string.Format("{0}={1}", p.Item1, p.Item2))) : string.Empty;

            WebClientHelper.Get<List<VesselInCompanyDto>>(new Uri(url, UriKind.Absolute),
                                                                    (res, exp) => action(res, exp),
                                                                    WebClientHelper.MessageFormat.Json,ApiConfig.Headers
                );
        }

        public void GetAll(Action<PageResultDto<VesselInCompanyDto>, Exception> action, string methodName, int pageSize, int pageIndex)
        {
            var url = string.Format(vesselAddressFormatString, string.Empty)
                + "?PageSize=" + pageSize + "&PageIndex=" + pageIndex;

            WebClientHelper.Get<PageResultDto<VesselInCompanyDto>>(new Uri(url, UriKind.Absolute),
                                                                    (res, exp) => action(res, exp),
                                                                    WebClientHelper.MessageFormat.Json,ApiConfig.Headers
                );
        }

        public void GetById(Action<VesselInCompanyDto, Exception> action, int id, bool includeCompany = true, bool includeTanks = false)
        {
            var url = string.Format(vesselAddressFormatString, id);

            url = url + "?includeCompany=" + includeCompany +
                "&includeTanks=" + includeTanks;

            WebClientHelper.Get<VesselInCompanyDto>(new Uri(url, UriKind.Absolute),
                                                     action,
                                                     WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GetPagedDataByFilter(Action<PageResultDto<VesselInCompanyDto>, Exception> action, long companyId, int? pageSize, int? pageIndex, bool operatedVessels)
        {
            GetPagedDataByFilter(action, companyId, string.Empty , operatedVessels, pageSize, pageIndex);
        }

        public void GetPagedDataByFilter(Action<PageResultDto<VesselInCompanyDto>, Exception> action, string vesselCode, int? pageSize, int? pageIndex, bool operatedVessels)
        {
            GetPagedDataByFilter(action, null, vesselCode, operatedVessels, pageSize, pageIndex);
        }

        public void GetActivationInfo(Action<VesselActivationDto, Exception> action, string vesselCode)
        {
            var url = string.Format(vesselActivationInfoAddressFormatString, string.Empty);

            url += '?';

            if (!string.IsNullOrWhiteSpace(vesselCode))
                url += "&vesselCode=" + vesselCode;

            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void ActivateWarehouseIncludingRecieptsOperation(Action<VesselInCompanyDto, Exception> action, string vesselCode, long companyId, DateTime activationDate, List<VesselActivationItemDto> vesselActivationItemDtos)
        {
            var url = string.Format(vesselAddressFormatString, string.Empty);

            url += "?companyId=" + companyId;
            url += "&vesselCode=" + vesselCode;
            url += "&activationDate=" + activationDate;

            WebClientHelper.Put<VesselInCompanyDto, List<VesselActivationItemDto>>(new Uri(url, UriKind.Absolute), action, vesselActivationItemDtos, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void GetPagedDataByFilter(Action<PageResultDto<VesselInCompanyDto>, Exception> action, long? companyId, string vesselCode, bool operatedVessels, int? pageSize, int? pageIndex)
        {
            var url = string.Format(vesselAddressFormatString, string.Empty);

            url += '?';
            if(companyId != null)
                url += "&companyId=" + companyId;

            if (!string.IsNullOrWhiteSpace(vesselCode))
                url += "&vesselCode=" + vesselCode;

            url += "&operatedVessels=" + operatedVessels;

            if (pageSize.HasValue && pageIndex.HasValue)
                url += "&pageSize" + pageSize + "&pageIndex" + pageIndex;

            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        } 


        //public void Add(Action<VesselInCompanyDto, Exception> action, VesselInCompanyDto ent)
        //{
        //    var url = vesselAddressFormatString;// string.Concat(baseAddress, "/Post");
        //    WebClientHelper.Post<VesselInCompanyDto, VesselInCompanyDto>(new Uri(url, UriKind.Absolute),
        //                                                                   (res, exp) => action(res, exp), ent,
        //                                                                   WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        //}

        //public void Update(Action<VesselInCompanyDto, Exception> action, VesselInCompanyDto ent)
        //{
        //    //var url = string.Concat(baseAddress, "/Put?Id=",ent.Id);
        //    var url = string.Concat(vesselAddressFormatString, ent.Id);
        //    WebClientHelper.Put<VesselInCompanyDto, VesselInCompanyDto>(new Uri(url, UriKind.Absolute),
        //                                                                  (res, exp) => action(res, exp), ent,
        //                                                                  WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        //}

        //public void Delete(Action<string, Exception> action, int id)
        //{
        //    //var url = string.Concat(baseAddress, "/DeleteById?id=", id);
        //    var url = string.Concat(vesselAddressFormatString, id);
        //    WebClientHelper.Delete(new Uri(url, UriKind.Absolute), (res, exp) => action(res, exp));
        //}

        #endregion

    }
}
