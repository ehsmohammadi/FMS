using System;
using MITD.Presentation;
using MITD.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;



namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IFuelReportVesselServiceWrapper : IServiceWrapper
    {
        void GetAll(Action<PageResultDto<VesselInCompanyDto>, Exception> action, string methodName, int pageSize,
                         int pageIndex);

        void GetById(Action<VesselInCompanyDto, Exception> action, int id);
    }
}
