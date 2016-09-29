using System;
using System.Windows;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface IVesselDataReportController
    {
        void ShowReport();
        void ShowVesselRunningValuesReport();
    }
}
