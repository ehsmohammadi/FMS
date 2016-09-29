using System;
using System.Collections;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;
namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IOffhireToOffhireDtoMapper : IFacadeMapper<Offhire, OffhireDto>
    {
        OffhireDto MapToModel(Offhire entity, Action<Offhire, OffhireDto> action);

        IEnumerable<OffhireDto> MapToModel(IEnumerable<Offhire> entities, Action<Offhire, OffhireDto> action);
    }
}