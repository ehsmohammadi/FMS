#region

using System;
using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IVoyageDomainService : IDomainService
    {
        List<Voyage> GetAll();

        bool IsVoyageAvailable(long voyageId);

        List<Voyage> GetVoyagesEndedBefore(long vesselInCompanyId, DateTime dateTime);

        Voyage Get(long id);

        List<Voyage> GetByFilter(long companyId, long vesselInCompanyId);

        PageResult<Voyage> GetPagedData(int pageSize, int pageIndex);

        PageResult<Voyage> GetPagedDataByFilter(long companyId, long vesselInCompanyId, int pageSize, int pageIndex);

        Voyage GetVoyage(Company company, VesselInCompany vesselInCompany, DateTime date);
        List<Voyage> FindVoyages(long companyId, long vesselInCompanyId, DateTime date);
        Voyage GetVoyageContainingDuration(Company company, VesselInCompany vesselInCompany, DateTime startDateTime, DateTime endDateTime);

        FuelReport GetVoyageValidEndOfVoyageFuelReport(long voyageId);
        
        void UpdateVoyageFromRotationData(long voyageId);

        void UpdateVoyageFromRotationData(Voyage voyage);
    }
}