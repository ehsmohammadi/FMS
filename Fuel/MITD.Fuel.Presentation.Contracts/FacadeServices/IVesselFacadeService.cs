using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
    public interface IVesselFacadeService : IFacadeService
    {
        PageResultDto<VesselDto> GetPagedData(int pageSize, int pageIndex);

        PageResultDto<VesselDto> GetPagedDataByFilter(long? ownerId, int pageSize, int pageIndex);

        void Add(VesselDto data);
        VesselDto Get(long id);
    }
}
