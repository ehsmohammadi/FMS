using System;
using System.Collections;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;
namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IScrapToScrapDtoMapper : IFacadeMapper<Scrap, ScrapDto>
    {
        ScrapDto MapToModel(Scrap entity, Action<Scrap, ScrapDto> action);

        IEnumerable<ScrapDto> MapToModel(IEnumerable<Scrap> entities, Action<Scrap, ScrapDto> action);
    }
}