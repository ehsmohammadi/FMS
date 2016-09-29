using System;
using System.Collections;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;
namespace MITD.Fuel.ACL.StorageSpace.Mappers.Contracts
{
    public interface IOffhirePreparedDataToOffhireDtoMapper : IFacadeMapper<OffhirePreparedData, OffhireDto>
    {

    }
}