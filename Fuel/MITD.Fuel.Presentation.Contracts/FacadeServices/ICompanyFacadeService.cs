using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
    public interface ICompanyFacadeService : IFacadeService
    {

        List<CompanyDto> GetAll();
        
        PageResultDto<VesselInCompanyDto> GetOwnedVessels(long companyId);

        List<CompanyDto> GetByUserId(long userId);

        List<CompanyDto> GetByCurrentUserId();

        List<CompanyDto> GetAll(bool byCurrentUserId, bool operatedVessels);
    }
}
