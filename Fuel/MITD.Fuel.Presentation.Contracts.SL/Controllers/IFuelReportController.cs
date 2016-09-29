using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface IFuelReportController
    {
        void Add();
        void Edit(FuelReportDto dto);
        void ShowList();

        void ShowOriginalVesselReport(string reportCode);

        //void ShowWin();
    }
}
