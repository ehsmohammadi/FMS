using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class MainUnitVlaueTomainUnitVlaueDtoMapper : BaseFacadeMapper<MainUnitValue, MainUnitValueDto>,
                                                         IMainUnitVlaueTomainUnitVlaueDtoMapper
    {
    }
}