using System;
using System.Windows;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.FuelApp.Logic.SL.ServiceWrapper
{
    public class FuelReportVesselServiceWrapper : IFuelReportVesselServiceWrapper
    {
        #region fields

        private string baseAddress = String.Format("{0}/apiarea/Fuel/FuelReportVessel/"
                                                  , ApiConfig.HostAddress);
        #endregion

        #region methods

        public void GetAll(Action<PageResultDto<VesselInCompanyDto>, Exception> action, string methodName, int pageSize,
                           int pageIndex)
        {
            var url = string.Format(baseAddress + methodName + "?PageSize=" + pageSize + "&PageIndex=" + pageIndex);
            WebClientHelper.Get<PageResultDto<VesselInCompanyDto>>(new Uri(url, UriKind.Absolute),
                                                                    (res, exp) => action(res, exp),
                                                                    WebClientHelper.MessageFormat.Json,ApiConfig.Headers
                );
        }

        public void GetById(Action<VesselInCompanyDto, Exception> action, int id)
        {
            var url = string.Concat(baseAddress, id);
            WebClientHelper.Get<VesselInCompanyDto>(new Uri(url, UriKind.Absolute),
                                                     (res, exp) => action(res, exp),
                                                     WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        #endregion

    }
}
