using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class OffhirePreparedDataToOffhireDtoMapper : IOffhirePreparedDataToOffhireDtoMapper
    {
        //BaseFacadeMapper<OffhirePreparedData, OffhireDto>,

        private readonly IOffhirePreparedDataDetailToOffhireDetailDtoMapper offhirePreparedDetailDtoMapper;
        private readonly IFacadeMapper<Company, CompanyDto> companyDtoMapper;
        private readonly IVesselInCompanyToVesselInCompanyDtoMapper vesselInCompanyDtoMapper;
        private readonly IActivityLocationToActivityLocationDtoMapper activityLocationDtoMapper;
        private IVoyageToVoyageDtoMapper voyageDtoMapper;

        public OffhirePreparedDataToOffhireDtoMapper(
            IOffhirePreparedDataDetailToOffhireDetailDtoMapper offhirePreparedDetailDtoMapper,
            IFacadeMapper<Company, CompanyDto> companyDtoMapper,
            IVesselInCompanyToVesselInCompanyDtoMapper vesselInCompanyDtoMapper,
            IActivityLocationToActivityLocationDtoMapper activityLocationDtoMapper,
            IVoyageToVoyageDtoMapper voyageDtoMapper)
        {
            this.companyDtoMapper = companyDtoMapper;
            this.vesselInCompanyDtoMapper = vesselInCompanyDtoMapper;
            this.activityLocationDtoMapper = activityLocationDtoMapper;
            this.voyageDtoMapper = voyageDtoMapper;
            this.offhirePreparedDetailDtoMapper = offhirePreparedDetailDtoMapper;
        }

        public OffhireDto MapToModel(OffhirePreparedData entity)
        {
            var result = new OffhireDto()
                         {
                             Id = -1,
                             ReferenceNumber = entity.ReferenceNumber,
                             StartDateTime = entity.StartDateTime,
                             EndDateTime = entity.EndDateTime,
                             Introducer = companyDtoMapper.MapToModel(entity.Introducer),
                             VesselInCompany = vesselInCompanyDtoMapper.MapToModel(entity.VesselInCompany),
                             Voyage = entity.Voyage == null ? null : voyageDtoMapper.MapToModel(entity.Voyage),
                             OffhireLocation = activityLocationDtoMapper.MapToModel(entity.Location),
                             OffhireDetails = new ObservableCollection<OffhireDetailDto>(this.offhirePreparedDetailDtoMapper.MapToModel(entity.OffhireDetails)),
                             IsOffhireEditPermitted = true,
                             IsOffhireDeletePermitted = false
                         };

            return result;
        }

        public IEnumerable<OffhireDto> MapToModel(IEnumerable<OffhirePreparedData> entities)
        {
            return entities.Select(MapToModel);
        }

        #region NotImplemented

        public IEnumerable<OffhirePreparedData> MapToEntity(IEnumerable<OffhireDto> models)
        {
            throw new System.NotImplementedException();
        }

        public OffhirePreparedData MapToEntity(OffhireDto model)
        {
            throw new System.NotImplementedException();
        }

        public OffhireDto RemapModel(OffhireDto model)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}