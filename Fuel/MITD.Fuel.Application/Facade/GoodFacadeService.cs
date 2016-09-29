using System.Collections.Generic;
using Castle.Core;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Fuel.Application.Facade.Contracts.Mappers;

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class GoodFacadeService : IGoodFacadeService
    {
        IGoodDomainService GoodDomainService { get; set; }
        IGoodToGoodDtoMapper GoodToGoodDtoMapper { get; set; }
        //ICompanyDomainService domainService
        public GoodFacadeService(IGoodDomainService goodDomainService, IGoodToGoodDtoMapper goodToGoodDtoMapper)
        {
            this.GoodToGoodDtoMapper = goodToGoodDtoMapper;
            GoodDomainService = goodDomainService;
        }

        public List<GoodDto> GetAll(long companyId)
        {
            
            var list = GoodDomainService.GetCompanyGoodsWithUnits(companyId);
            var dtos = GoodToGoodDtoMapper.MapEntityToDtoWithUnits(list);
            return dtos;
        }
    }
}