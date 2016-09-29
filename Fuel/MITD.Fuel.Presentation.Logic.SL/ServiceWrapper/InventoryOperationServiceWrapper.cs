using System;
using System.Collections.Generic;
using System.Windows;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation.Contracts;
using System.IO;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public class InventoryOperationServiceWrapper : IInventoryOperationServiceWrapper
    {
        private string fuelReportInventoryOperationAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/FuelReport/{0}/InventoryOperation/{1}");
        private string fuelReportDetailInventoryOperationAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/FuelReport/{0}/Detail/{1}/InventoryOperation/{2}");
        
        private string scrapInventoryOperationAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/Scrap/{0}/InventoryOperation/{1}");

        public void GetFuelReportInventoryOperations(Action<List<FuelReportInventoryOperationDto>, Exception> action, long fuelReportId)
        {
            var url = string.Format(fuelReportInventoryOperationAddressFormatString,
                        fuelReportId, string.Empty);

            WebClientHelper.Get(Contracts.SL.Infrastructure.ApiServiceAddressHelper.BuildUri(url), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void GetFuelReportDetailInventoryOperations(Action<List<FuelReportInventoryOperationDto>, Exception> action, long fuelReportId, long fuelReportDetailId)
        {
            var url = string.Format(fuelReportDetailInventoryOperationAddressFormatString,
                        fuelReportId, fuelReportDetailId, string.Empty);

            WebClientHelper.Get(Contracts.SL.Infrastructure.ApiServiceAddressHelper.BuildUri(url), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GetScrapInventoryOperations(Action<PageResultDto<FuelReportInventoryOperationDto>, Exception> action, long scrapId)
        {
            var url = string.Format(scrapInventoryOperationAddressFormatString,
                                    scrapId, string.Empty);

            WebClientHelper.Get(Contracts.SL.Infrastructure.ApiServiceAddressHelper.BuildUri(url), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }
    }
}
