using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class InventoryOperationToInventoryOperationDtoMapper : IInventoryOperationToInventoryOperationDtoMapper
    {
        private readonly IFuelReportDetailToFuelReportDetailDtoMapper fuelReportDetailDtoMapper;

        public InventoryOperationToInventoryOperationDtoMapper()
        {
            
        }

        public IEnumerable<Domain.Model.DomainObjects.InventoryOperation> MapToEntity(IEnumerable<Presentation.Contracts.DTOs.FuelReportInventoryOperationDto> models)
        {
            throw new NotImplementedException();
        }

        public Domain.Model.DomainObjects.InventoryOperation MapToEntity(Presentation.Contracts.DTOs.FuelReportInventoryOperationDto model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Presentation.Contracts.DTOs.FuelReportInventoryOperationDto> MapToModel(IEnumerable<Domain.Model.DomainObjects.InventoryOperation> entities)
        {
            var dtos = entities.Select(MapToModel).ToList();

            return dtos;
        }

        public FuelReportInventoryOperationDto MapToModel(Domain.Model.DomainObjects.InventoryOperation entity)
        {
            var dto = new FuelReportInventoryOperationDto()
            {
                Id = entity.Id,
                Code = entity.ActionNumber,
                ActionType = entity.ActionType.ToString(),
                ActionDate = entity.ActionDate,
            };

            return dto;
        }

        public Presentation.Contracts.DTOs.FuelReportInventoryOperationDto RemapModel(Presentation.Contracts.DTOs.FuelReportInventoryOperationDto model)
        {
            throw new NotImplementedException();
        }
    }
}
