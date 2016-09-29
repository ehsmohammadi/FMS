using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IRecieveTypeToRecieveTypeDtoMapper : IFacadeMapper<ReceiveType, ReceiveTypeDto>
    {
        ReceiveTypeDto MapEntityToDto(ReceiveType receiveType);
        List<ReceiveTypeDto> MapEntityToDto(List<ReceiveType> receiveTypes);
        ReceiveType MapDtoToEntity(ReceiveTypeDto receiveType);
        List<ReceiveType> MapDtoToEntity(List<ReceiveTypeDto> receiveTypes);
    }
}
