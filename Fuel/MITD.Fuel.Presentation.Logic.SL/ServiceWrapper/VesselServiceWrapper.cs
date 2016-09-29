using System;
using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation.Contracts;
using System.IO;

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public class VesselServiceWrapper : IVesselServiceWrapper
    {
        #region fields

        private string vesselAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/Vessel/{0}");

        #endregion

        #region methods

        public void GetById(Action<VesselDto, Exception> action, long id)
        {
            var url = string.Format(vesselAddressFormatString, id);

            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void GetPagedVesselData(Action<PageResultDto<VesselDto>, Exception> action, int pageSize, int pageIndex)
        {
            var url = string.Format(vesselAddressFormatString, string.Empty);
            url += "?pageSize=" + pageSize + "&pageIndex=" + pageIndex;
            
            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void GetPagedVesselDataByFilter(Action<PageResultDto<VesselDto>, Exception> action, long? ownerId, int pageSize, int pageIndex)
        {
            var url = string.Format(vesselAddressFormatString, string.Empty);

            url += "?ownerId=" + ownerId;
            url += "&pageSize=" + pageSize + "&pageIndex=" + pageIndex;

            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void AddVessel(Action<VesselDto, Exception> action, VesselDto vesselDto)
        {
            var url = string.Format(vesselAddressFormatString, string.Empty);
            WebClientHelper.Post(new Uri(url, UriKind.Absolute), action, vesselDto, MITD.Presentation.WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void UpdateVessel(Action<VesselDto, Exception> action, VesselDto vesselDto)
        {
            throw new NotImplementedException();
        }

        public void DeleteVessel(Action<string, Exception> action, long id)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
