using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;
using System.Collections.Generic;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices.Fuel
{
    public interface IFuelReportCompanyFacadeService : IFacadeService
    {
        //CompanyDto Add(CompanyDto data);
        //CompanyDto Update(CompanyDto data);
        //void Delete(CompanyDto data);
        //CompanyDto GetById(int id);
        //PageResultDto<CompanyDto> GetAll(int pageSize, int pageIndex);
        //void DeleteById(int id);


        List<CompanyDto> GetAll();

        List<CompanyDto> GetByUserId(long userId);
        List<CompanyDto> GetByCurrentUserId();
    }
}
