using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
   public interface ITransferTypeToTransferTypeDtoMapper :IFacadeMapper<TransferType,TransferTypeDto>
    {
       TransferTypeDto MapEntityToDto(TransferType transferType);
       List<TransferTypeDto> MapEntityToDto(List<TransferType> transferTypes);
       TransferType MapDtoToEntity(TransferTypeDto transferType);
       List<TransferType> MapDtoToEntity(List<TransferTypeDto> transferTypes);

    }
}
