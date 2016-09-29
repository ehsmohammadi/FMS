using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;
namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public class OffhireManagementSystemEntityToOffhireManagementSystemDtoMapper : BaseFacadeMapper<OffhireManagementSystemEntity, OffhireManagementSystemDto>, IOffhireManagementSystemEntityToOffhireManagementSystemDtoMapper
    {
        private IOffhireManagementSystemEntityDetailToOffhireManagementSystemDetailDtoMapper offhireManagementSystemDetailDtoMapper;
        private IActivityLocationToActivityLocationDtoMapper activityLocationDtoMapper;
        private IVesselInCompanyToVesselInCompanyDtoMapper vesselInCompanyDtoMapper;

        public OffhireManagementSystemEntityToOffhireManagementSystemDtoMapper(
            IOffhireManagementSystemEntityDetailToOffhireManagementSystemDetailDtoMapper offhireManagementSystemDetailDtoMapper,
            IVesselInCompanyToVesselInCompanyDtoMapper vesselInCompanyDtoMapper,
            IActivityLocationToActivityLocationDtoMapper activityLocationDtoMapper)
        {
            this.offhireManagementSystemDetailDtoMapper = offhireManagementSystemDetailDtoMapper;
            this.vesselInCompanyDtoMapper = vesselInCompanyDtoMapper;
            this.activityLocationDtoMapper = activityLocationDtoMapper;
        }

        public override OffhireManagementSystemDto MapToModel(OffhireManagementSystemEntity entity)
        {
            var dto = base.MapToModel(entity);

            dto.OffhireDetails = new ObservableCollection<OffhireManagementSystemDetailDto>(offhireManagementSystemDetailDtoMapper.MapToModel(entity.OffhireDetails));

            dto.VesselInCompany = vesselInCompanyDtoMapper.MapToModel(entity.VesselInCompany);

            dto.OffhireLocation = activityLocationDtoMapper.MapToModel(entity.Location);

            return dto;
        }

        public override IEnumerable<OffhireManagementSystemDto> MapToModel(IEnumerable<OffhireManagementSystemEntity> entities)
        {
            return entities.Select(MapToModel);
        }

        #region Not Implemented

        public OffhireManagementSystemEntity MapToEntity(OffhireManagementSystemDto model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OffhireManagementSystemEntity> MapToEntity(IEnumerable<OffhireManagementSystemDto> models)
        {
            throw new NotImplementedException();
        }

        public OffhireManagementSystemDto RemapModel(OffhireManagementSystemDto model)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class OffhireManagementSystemEntityDetailToOffhireManagementSystemDetailDtoMapper :
        IOffhireManagementSystemEntityDetailToOffhireManagementSystemDetailDtoMapper
    {
        private readonly IGoodToGoodDtoMapper goodMapper;
        private readonly ICompanyGoodUnitToGoodUnitDtoMapper goodUnitMapper;
        private readonly ITankToTankDtoMapper tankMapper;

        public OffhireManagementSystemEntityDetailToOffhireManagementSystemDetailDtoMapper(
            IGoodToGoodDtoMapper goodMapper,
            ICompanyGoodUnitToGoodUnitDtoMapper goodUnitMapper,
            ITankToTankDtoMapper tankMapper)
        {
            this.goodMapper = goodMapper;
            this.goodUnitMapper = goodUnitMapper;
            this.tankMapper = tankMapper;
        }

        public OffhireManagementSystemDetailDto MapToModel(OffhireManagementSystemEntityDetail entity)
        {
            var dto = new OffhireManagementSystemDetailDto()
                      {
                          Quantity = entity.QuantityAmount,
                          Good = goodMapper.MapToModel(entity.Good),
                          Tank = tankMapper.MapToModel(entity.Tank),
                          Unit = goodUnitMapper.MapToModel(entity.Unit)
                      };

            return dto;
        }

        public IEnumerable<OffhireManagementSystemDetailDto> MapToModel(IEnumerable<OffhireManagementSystemEntityDetail> entities)
        {
            return entities.Select(MapToModel);
        }

        #region Not Implemented

        public OffhireManagementSystemEntityDetail MapToEntity(OffhireManagementSystemDetailDto model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OffhireManagementSystemEntityDetail> MapToEntity(IEnumerable<OffhireManagementSystemDetailDto> models)
        {
            throw new NotImplementedException();
        }

        public OffhireManagementSystemDetailDto RemapModel(OffhireManagementSystemDetailDto model)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}