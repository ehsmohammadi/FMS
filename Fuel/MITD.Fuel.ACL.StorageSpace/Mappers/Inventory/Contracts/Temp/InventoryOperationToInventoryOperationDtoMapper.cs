using MITD.Fuel.ACL.StorageSpace.InventoryServiceReference;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
{
    public class InventoryOperationToInventoryOperationDtoMapper : IInventoryOperationToInventoryOperationDtoMapper
    {
        private readonly IFuelReportDetailToFuelReportDetailDtoMapper fuelReportDetailDtoMapper;

        public InventoryOperationToInventoryOperationDtoMapper(IFuelReportDetailToFuelReportDetailDtoMapper fuelReportDetailDtoMapper)
        {
            this.fuelReportDetailDtoMapper = fuelReportDetailDtoMapper;
        }

        public IEnumerable<Domain.Model.DomainObjects.InventoryOperation> MapToEntity(IEnumerable<Presentation.Contracts.DTOs.FuelReportInventoryOperationDto> models)
        {
            throw new NotImplementedException();
        }

        public Domain.Model.DomainObjects.InventoryOperation MapToEntity(Presentation.Contracts.DTOs.FuelReportInventoryOperationDto model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FuelReportInventoryOperationDto> MapToModel(IEnumerable<Domain.Model.DomainObjects.InventoryOperation> entities)
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
                //FuelReportDetail = this.fuelReportDetailDtoMapper.MapToModel(entity.FuelReportDetail)
            };

            return dto;
        }

        public Presentation.Contracts.DTOs.FuelReportInventoryOperationDto RemapModel(Presentation.Contracts.DTOs.FuelReportInventoryOperationDto model)
        {
            throw new NotImplementedException();
        }
    }
}
