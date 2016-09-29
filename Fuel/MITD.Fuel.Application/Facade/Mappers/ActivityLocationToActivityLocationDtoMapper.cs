using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public class ActivityLocationToActivityLocationDtoMapper : IActivityLocationToActivityLocationDtoMapper
    {
        public ActivityLocationDto MapToModel(ActivityLocation entity)
        {
            var result = new ActivityLocationDto
                         {
                             Id = entity.Id,
                             Abbreviation = entity.Abbreviation,
                             Name = entity.Name,
                             Code = entity.Code,
                             Latitude = entity.Latitude,
                             Longititude = entity.Longitude,
                             CountryName = entity.CountryName
                         };

            return result;
        }

        public IEnumerable<ActivityLocationDto> MapToModel(IEnumerable<ActivityLocation> entities)
        {
            return entities.Select(MapToModel);
        }

        #region Not Implemented

        public ActivityLocation MapToEntity(ActivityLocationDto model)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ActivityLocation> MapToEntity(IEnumerable<ActivityLocationDto> models)
        {
            throw new System.NotImplementedException();
        }

        public ActivityLocationDto RemapModel(ActivityLocationDto model)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}