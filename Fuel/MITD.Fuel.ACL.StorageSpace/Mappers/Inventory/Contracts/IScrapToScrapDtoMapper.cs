using System;
using System.Collections.Generic;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface IScrapToScrapDtoMapper : IFacadeMapper<Scrap, ScrapDto>
    {
        ScrapDto MapToModel(Scrap entity, Action<Scrap, ScrapDto> action);

        IEnumerable<ScrapDto> MapToModel(IEnumerable<Scrap> entities, Action<Scrap, ScrapDto> action);
    }
}