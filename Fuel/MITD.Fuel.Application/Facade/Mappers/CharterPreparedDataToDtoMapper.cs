using System.Collections.ObjectModel;
using System.Linq;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Facade;
using CharterTypeInDto = MITD.Fuel.Presentation.Contracts.Enums.CharterType;
using CharterTypeInDomain = MITD.Fuel.Domain.Model.Enums.CharterType;


namespace MITD.Fuel.Application.Facade.Mappers
{
    public class CharterPreparedDataToDtoMapper : BaseFacadeMapper<CharterPreparedData, CharterDto>, ICharterPreparedDataToDtoMapper
    {

        private readonly ICharterPreparedDataItemToDtoMapper charterPreparedDataItemToDtoMapper;

        public CharterPreparedDataToDtoMapper(ICharterPreparedDataItemToDtoMapper charterPreparedDataItemToDtoMapper)
        {
            this.charterPreparedDataItemToDtoMapper = charterPreparedDataItemToDtoMapper;
        }

        public override CharterDto MapToModel(CharterPreparedData entity)
        {
            var res = new CharterDto
                      {
                Owner = (entity.Owner != null) ? base.Map(new CompanyDto(), entity.Owner) as CompanyDto : null,
                Charterer = (entity.Charterer != null) ? base.Map(new CompanyDto(), entity.Charterer) as CompanyDto : null,
                VesselInCompany = (entity.VesselInCompany != null) ? base.Map(new VesselInCompanyDto(), entity.VesselInCompany) as VesselInCompanyDto : null,
                StartDate =  entity.StartDate,
                EndDate =  entity.EndDate,
                //TODO: New type should be implemented for CharterPreparedDate.
                //CharterType = entity.CharteringType == CharteringType.In ? CharterTypeInDto.In : CharterTypeInDto.Out,
                CharterStateType = entity.CharterState == CharterTypeInDomain.Start ? CharterStateTypeEnum.Start : CharterStateTypeEnum.End,
                CharterItems = new ObservableCollection<CharterItemDto>(entity.CharterItems.Select(charterPreparedDataItemToDtoMapper.MapToModel))
                
            };

            return res;
        }
    }
}
