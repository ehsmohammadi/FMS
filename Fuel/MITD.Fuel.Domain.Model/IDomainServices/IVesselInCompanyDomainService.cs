#region

using System;
using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

#endregion

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IVesselInCompanyDomainService : IDomainService<VesselInCompany>
    {
        List<Voyage> Find(long companyId, long vesselInCompanyId);
        List<VesselInCompany> Get(List<long> idList);
        List<VesselInCompany> GetCompanyVessels(long companyId);
        VesselInCompany GetVesselInCompany(long companyId, string vesselCode);
        List<VesselInCompany> GetVesselInCompanies(string vesselCode);
        List<VesselInCompany> GetVesselInCompanies(long? companyId, string vesselStates);
        List<VesselInCompany> GetOwnedOrCharterInVessels(long companyId);
        List<VesselInCompany> GetInactiveVessels(long companyId);
        List<VesselInCompany> GetOwnedVessels(long companyId);

        List<VesselActivationItem> GetVesselActivationInfo(string vesselCode, out DateTime vesselActivationDate);

        VesselInCompany GetWithTanks(long id);
        //void ChangeVesselStateBackToOwner();

        VesselStates GetVesselCurrentState(long id);
        List<VesselStates> GetVesselStatesLog(long id, DateTime? startDate, DateTime? toDate);

        void ScrapVessel(long vesselInCompanyId);

        void ActivateVessel(long vesselInCompanyId);

        void DeactivateVessel(long vesselInCompanyId);

        void BeginCharteringInVessel(long vesselInCompanyId, DateTime charterInStartDateTime, long userId);

        void FinishCharteringInVessel(long vesselInCompanyId);

        void BeginCharteringOutVessel(long vesselInCompanyId, DateTime charterOutstartDateTime, long userId);

        void FinishCharteringOutVessel(long vesselInCompanyId, DateTime charterOutEndDateTime, long userId);

        void RegisterNewVessel(long ownerId, string vesselCode, string name, string description, DateTime activationTime,
            int userId);

        void AssignVesselToCompany(Vessel vessel, Company company, string name, string description, int userId, long warehouseId);
    }
}