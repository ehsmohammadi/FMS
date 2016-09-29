using System;
using System.Collections.Generic;
using MITD.Presentation;
using MITD.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IVesselServiceWrapper : IServiceWrapper
    {
        void GetById(Action<VesselDto, Exception> action, long id);
        void GetPagedVesselData(Action<PageResultDto<VesselDto>, Exception> action, int pageSize, int pageIndex);
        void GetPagedVesselDataByFilter(Action<PageResultDto<VesselDto>, Exception> action, long? companyId, int pageSize, int pageIndex);

        void AddVessel(Action<VesselDto, Exception> action, VesselDto vesselDto);
        void UpdateVessel(Action<VesselDto, Exception> action, VesselDto vesselDto);
        void DeleteVessel(Action<string, Exception> action, long id);
    }
}
