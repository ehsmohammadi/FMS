using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Service.Host.ServiceContracts.Facade.Mappers
{
   public interface ITransferTypeToTransferTypeDtoMapper :IFacadeMapper<TransferType,TransferTypeDto>
    {
       TransferTypeDto MapEntityToDto(TransferType transferType);
       List<TransferTypeDto> MapEntityToDto(List<TransferType> transferTypes);
       TransferType MapDtoToEntity(TransferTypeDto transferType);
       List<TransferType> MapDtoToEntity(List<TransferTypeDto> transferTypes);

    }
}
