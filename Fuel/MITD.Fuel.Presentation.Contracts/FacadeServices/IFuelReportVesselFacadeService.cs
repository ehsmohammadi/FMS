using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;
using System.Collections.Generic;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices.Fuel
{
    public interface IFuelReportVesselFacadeService : IFacadeService
    {
        List<VesselInCompanyDto> GetAll();
    }
}
