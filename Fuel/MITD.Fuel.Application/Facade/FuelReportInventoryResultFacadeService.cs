using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class FuelReportInventoryResultFacadeService : IFuelReportInventoryResultFacadeService
    {
        private readonly IFuelReportApplicationService fuelReportApplicationService;

        public FuelReportInventoryResultFacadeService(IFuelReportApplicationService fuelReportApplicationService)
        {
            this.fuelReportApplicationService = fuelReportApplicationService;
        }

        public void IsHandleResultPossible(long fuelReportId)
        {
            fuelReportApplicationService.IsSetFuelReportInventoryResultPossible(fuelReportId);
        }

        public void HandleResult(Presentation.Contracts.DTOs.FuelReportInventoryResultDto dto)
        {
            InventoryResultCommand bag = mapInventoryResultDtoToResultBag(dto);

            fuelReportApplicationService.SetFuelReportInventoryResults(bag);
        }

        private InventoryResultCommand mapInventoryResultDtoToResultBag(Presentation.Contracts.DTOs.FuelReportInventoryResultDto dto)
        {
            InventoryResultCommand bag = new InventoryResultCommand();

            bag.FuelReportId = dto.FuelReportId;

            bag.Items = new List<InventoryResultCommandItem>();

            bag.Items.AddRange(
                dto.Items.Select(
                    i => new InventoryResultCommandItem
                    {
                        FuelReportDetailId = i.FuelReportDetailId,
                        ActionNumber = i.ActionNumber,
                        ActionType = (Domain.Model.Enums.InventoryActionType)i.ActionType,
                        ActionDate = i.ActionDate
                    }));

            return bag;
        }
    }
}
