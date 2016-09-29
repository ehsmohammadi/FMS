using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Application.Facade.Contracts.Mappers;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class RecieveTypeToRecieveTypeDtoMapper : IRecieveTypeToRecieveTypeDtoMapper
    {
        public ReceiveTypeDto MapEntityToDto(ReceiveType fuelReportDetail)
        {
            var res = new ReceiveTypeDto()
                          {
                              Id = fuelReportDetail.Id,
                              Code=fuelReportDetail.Code,
                              Name=fuelReportDetail.Name
                          };
            return res;
        }

        public List<ReceiveTypeDto> MapEntityToDto(List<ReceiveType> fuelReportDetails)
        {
            return fuelReportDetails.Select(ent => MapEntityToDto(ent)).ToList();
        }

        public ReceiveType MapDtoToEntity(ReceiveTypeDto fuelReportDetail)
        {
            var entity = new ReceiveType(fuelReportDetail.Id,fuelReportDetail.Name,fuelReportDetail.Code);
            return entity;
        }

        public List<ReceiveType> MapDtoToEntity(List<ReceiveTypeDto> fuelReportDetails)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReceiveType> MapToEntity(IEnumerable<ReceiveTypeDto> models)
        {
            throw new NotImplementedException();
        }

        public ReceiveType MapToEntity(ReceiveTypeDto model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReceiveTypeDto> MapToModel(IEnumerable<ReceiveType> entities)
        {
            throw new NotImplementedException();
        }

        public ReceiveTypeDto MapToModel(ReceiveType entity)
        {
            throw new NotImplementedException();
        }

        public ReceiveTypeDto RemapModel(ReceiveTypeDto model)
        {
            throw new NotImplementedException();
        }
    }
}