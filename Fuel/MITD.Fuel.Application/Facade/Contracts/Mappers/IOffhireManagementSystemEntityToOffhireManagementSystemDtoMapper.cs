using System;
using System.Collections;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;
namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IOffhireManagementSystemEntityToOffhireManagementSystemDtoMapper : IFacadeMapper<OffhireManagementSystemEntity, OffhireManagementSystemDto>
    {
        //OffhireManagementSystemDto MapToModel(OffhireManagementSystemEntity entity, Action<OffhireManagementSystemEntity, OffhireManagementSystemDto> action);

        //IEnumerable<OffhireManagementSystemDto> MapToModel(IEnumerable<OffhireManagementSystemEntity> entities, Action<OffhireManagementSystemEntity, OffhireManagementSystemDto> action);
    }

    public interface IOffhireManagementSystemEntityDetailToOffhireManagementSystemDetailDtoMapper : IFacadeMapper<OffhireManagementSystemEntityDetail, OffhireManagementSystemDetailDto>
    {
        //OffhireManagementSystemDto MapToModel(OffhireManagementSystemEntity entity, Action<OffhireManagementSystemEntity, OffhireManagementSystemDto> action);

        //IEnumerable<OffhireManagementSystemDto> MapToModel(IEnumerable<OffhireManagementSystemEntity> entities, Action<OffhireManagementSystemEntity, OffhireManagementSystemDto> action);
    }
}