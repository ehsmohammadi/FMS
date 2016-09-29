﻿using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IVesselInCompanyToVesselInCompanyDtoMapper : IFacadeMapper<VesselInCompany, VesselInCompanyDto>
    {
        
    }

    public interface IVesselInInventoryToVesselDtoMapper : IFacadeMapper<VesselInInventory, VesselInCompanyDto>
    {

    }
}