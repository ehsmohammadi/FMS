using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface IFuelReportVesselInCompanyController
    {
        void Add();
        void Edit(VesselInCompanyDto dto);
        void ShowList();
    }
}
