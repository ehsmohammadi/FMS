using System;
using System.Net;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface IVesselActivationController
    {
        void AddVesselActivationItem(VesselDto toVessel, Action<VesselActivationItemDto> vesselActivationItemAdded);
    }
}
