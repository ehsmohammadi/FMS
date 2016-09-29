using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Application.Facade.Contracts.Mappers;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class TransferTypeToTransferTypeDtoMapper : ITransferTypeToTransferTypeDtoMapper
    {
        public TransferTypeDto MapEntityToDto(TransferType transferType)
        {
            var res = new TransferTypeDto()
            {
                Id = transferType.Id,
                Code = transferType.Code,
                Name = transferType.Name
            };
            return res;
        }

        public List<TransferTypeDto> MapEntityToDto(List<TransferType> transferTypes)
        {
            return transferTypes.Select(ent => MapEntityToDto(ent)).ToList();
        }

        public TransferType MapDtoToEntity(TransferTypeDto transferType)
        {
            var entity = new TransferType(transferType.Id, transferType.Name, transferType.Code);
            return entity;
        }

        public List<TransferType> MapDtoToEntity(List<TransferTypeDto> transferTypes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TransferType> MapToEntity(IEnumerable<TransferTypeDto> models)
        {
            throw new NotImplementedException();
        }

        public TransferType MapToEntity(TransferTypeDto model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TransferTypeDto> MapToModel(IEnumerable<TransferType> entities)
        {
            throw new NotImplementedException();
        }

        public TransferTypeDto MapToModel(TransferType entity)
        {
            throw new NotImplementedException();
        }

        public TransferTypeDto RemapModel(TransferTypeDto model)
        {
            throw new NotImplementedException();
        }
    }
}