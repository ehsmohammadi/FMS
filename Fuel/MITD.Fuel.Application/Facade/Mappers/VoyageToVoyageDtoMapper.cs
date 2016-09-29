using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;
namespace MITD.Fuel.Application.Facade.Mappers
{
    public class VoyageToVoyageDtoMapper : BaseFacadeMapper<Voyage, VoyageDto>, IVoyageToVoyageDtoMapper
    {
        public VoyageToVoyageDtoMapper()
        {

        }

        public override VoyageDto MapToModel(Voyage entity)
        {
            var dto = base.MapToModel(entity);

            dto.Code = entity.VoyageNumber;

            return dto;
        }
    }
}