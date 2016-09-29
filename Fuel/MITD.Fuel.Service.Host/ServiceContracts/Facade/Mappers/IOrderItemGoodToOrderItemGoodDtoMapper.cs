using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Service.Host.ServiceContracts.Facade.Mappers
{
    public interface IGoodToGoodDtoMapper : IFacadeMapper<Good, GoodDto>
    {
        GoodDto MapEntityToDto( Good good);
        List<GoodDto> MapEntityToDto(List<Good> entities);

    }
}