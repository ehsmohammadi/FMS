using System;
using System.Net;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface IVesselController
    {
        void ShowList();
        void Add();
        void Edit(VesselDto vesseDto);
        void ActivateVessel(VesselDto vesseDto);
    }
}
