using System;
using System.Collections;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;
namespace MITD.Fuel.ACL.StorageSpace.Mappers.Contracts
{
    public interface IOffhireToOffhireDtoMapper : IFacadeMapper<Offhire, OffhireDto>
    {
        OffhireDto MapToModel(Offhire entity, Action<Offhire, OffhireDto> action);

        IEnumerable<OffhireDto> MapToModel(IEnumerable<Offhire> entities, Action<Offhire, OffhireDto> action);
    }
}