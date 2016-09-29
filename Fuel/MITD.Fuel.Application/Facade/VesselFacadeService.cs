#region

using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service.Contracts;

#endregion

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class VesselFacadeService : IVesselFacadeService
    {

        #region props

        private readonly IVesselDomainService _vesselDomainService;
        private readonly IVesselApplicationService _vesselApplicationService;

        private IVesselToVesselDtoMapper VesselMapper { get; set; }

        #endregion

        #region ctor

        public VesselFacadeService(IVesselDomainService vesselService,
            IVesselToVesselDtoMapper vesselMapper,
            IVesselApplicationService vesselApplicationService) 
        {
            _vesselDomainService = vesselService;
            _vesselApplicationService = vesselApplicationService;
            VesselMapper = vesselMapper;
        }

        #endregion

        #region methods

        public PageResultDto<VesselDto> GetPagedData(int pageSize, int pageIndex)
        {
            var pageResult = _vesselDomainService.GetPagedData(pageSize, pageIndex);
            return mapPageResult(pageResult);
        }

        public PageResultDto<VesselDto> GetPagedDataByFilter(long? ownerId, int pageSize, int pageIndex)
        {
            var pageResult = _vesselDomainService.GetPagedDataByFilter(ownerId, pageSize, pageIndex);
            return mapPageResult(pageResult);
        }

        public void Add(VesselDto data)
        {
            _vesselApplicationService.AddVessel(data);
        }

        public VesselDto Get(long id)
        {
            var result = _vesselDomainService.Get(id);
            return VesselMapper.MapToModel(result);

        }

        #endregion

        private PageResultDto<VesselDto> mapPageResult(PageResult<Vessel> pageResult)
        {
            return new PageResultDto<VesselDto>()
            {
                CurrentPage = pageResult.CurrentPage,
                PageSize = pageResult.PageSize,
                TotalCount = pageResult.TotalCount,
                TotalPages = pageResult.TotalPages,
                Result = VesselMapper.MapToModel(pageResult.Result).ToList()
            };
        }

    }
}