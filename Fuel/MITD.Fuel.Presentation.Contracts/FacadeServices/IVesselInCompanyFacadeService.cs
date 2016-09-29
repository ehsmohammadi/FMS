using System;
using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
    public interface IVesselInCompanyFacadeService : IFacadeService
    {
        List<VesselInCompanyDto> GetAll(long? companyId, string vesselStates);
        List<VesselInCompanyDto> GetOwnedVessels(long companyId);
        List<VesselInCompanyDto> GetOwnedOrCharterInVessels(long companyId);
        VesselInCompanyDto GetById(long id);
        List<VesselInCompanyDto> GetCompanyVessels(long enterpriseId, bool operatedVessels);

        List<VesselInCompanyDto> GetVesselInCompanies(string vesselCode);
        List<VesselInCompanyDto> GetVesselInCompanies(long companyId, string vesselCode);

        VesselActivationDto GetVesselActivationInfo(string vesselCode);
        void ActivateWarehouseIncludingRecieptsOperation(string vesselCode, long companyId, DateTime activationDate, List<VesselActivationItemDto> vesselActivationItemDtos);
    }
}
