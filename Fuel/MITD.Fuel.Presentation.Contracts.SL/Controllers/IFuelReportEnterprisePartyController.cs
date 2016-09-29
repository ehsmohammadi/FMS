using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface IFuelReportCompanyController
    {
        void Add();
        void Edit(CompanyDto dto);
        void ShowList();

       
    }
}
