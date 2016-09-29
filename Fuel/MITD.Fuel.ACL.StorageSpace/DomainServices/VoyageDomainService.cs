using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.Specifications;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices
{
    public class VoyageDomainService : IVoyageDomainService
    {
        private readonly IVoyageRepository voyageRepository;
        private readonly IFuelReportRepository fuelReportRepository;
        private IRepository<RotationVoyage> rotationVoyageRepository;

        public VoyageDomainService(IVoyageRepository voyageRepository, IFuelReportRepository fuelReportRepository, IRepository<RotationVoyage> rotationVoyageRepository)
        {
            this.voyageRepository = voyageRepository;
            this.fuelReportRepository = fuelReportRepository;
            this.rotationVoyageRepository = rotationVoyageRepository;
        }

        public List<Voyage> Get(List<long> ids)
        {
            return this.voyageRepository.Find(v => v.IsActive).Where(v => ids.Contains(v.Id)).ToList();
        }

        public Voyage Get(long id)
        {
            var fetchStrategy = new SingleResultFetchStrategy<Voyage>();
            fetchStrategy.Include(v => v.VesselInCompany).Include(v => v.Company);

            return this.voyageRepository.First(v => v.Id == id && v.IsActive, fetchStrategy);
        }

        public List<Voyage> GetAll()
        {
            var fetchStrategy = new ListFetchStrategy<Voyage>();
            fetchStrategy.Include(v => v.VesselInCompany).Include(v => v.Company);

            return this.voyageRepository.GetAll(fetchStrategy)
                .OrderBy(v => v.VesselInCompany.Vessel.Code)
                .ThenBy(v => v.EndDate.HasValue ? v.EndDate : DateTime.MaxValue).ToList();
        }

        public bool IsVoyageAvailable(long voyageId)
        {
            return this.voyageRepository.Find(v => v.Id == voyageId && v.IsActive).Count == 1;
        }


        public List<Voyage> GetVoyagesEndedBefore(long vesselInCompanyId, DateTime dateTime)
        {
            var fetchStrategy = new ListFetchStrategy<Voyage>();
            fetchStrategy.Include(v => v.VesselInCompany).Include(v => v.Company);

            return this.voyageRepository.Find(
                v =>
                    v.VesselInCompanyId == vesselInCompanyId &&
                    (v.EndDate.HasValue && v.EndDate < dateTime) &&
                    v.IsActive, fetchStrategy)
                .OrderBy(v => v.EndDate.HasValue ? v.EndDate : DateTime.MaxValue).ToList();
        }

        public List<Voyage> GetByFilter(long companyId, long vesselInCompanyId)
        {
            var fetchStrategy = new ListFetchStrategy<Voyage>();
            fetchStrategy.Include(v => v.VesselInCompany).Include(v => v.Company);

            var result = this.voyageRepository.Find(
                v => v.IsActive && v.CompanyId == companyId && v.VesselInCompanyId == vesselInCompanyId, fetchStrategy)
                .OrderByDescending(v => v.EndDate.HasValue ? v.EndDate : DateTime.MaxValue).ToList();

            return result;
        }

        public PageResult<Voyage> GetPagedData(int pageSize, int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<Voyage>();
            fetchStrategy.Include(v => v.VesselInCompany).Include(v => v.Company).OrderByDescending(v => v.EndDate.HasValue ? v.EndDate : DateTime.MaxValue);

            fetchStrategy.WithPaging(pageSize, pageNumber);

            this.voyageRepository.Find(v => v.IsActive, fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }

        public PageResult<Voyage> GetPagedDataByFilter(long companyId, long vesselInCompanyId, int pageSize, int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<Voyage>();
            fetchStrategy.Include(v => v.VesselInCompany).Include(v => v.Company).OrderByDescending(v => v.EndDate.HasValue ? v.EndDate : DateTime.MaxValue);

            fetchStrategy.WithPaging(pageSize, pageNumber);

            this.voyageRepository.Find(v => v.IsActive && v.CompanyId == companyId && v.VesselInCompanyId == vesselInCompanyId, fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }

        public Voyage GetVoyage(Company company, VesselInCompany vesselInCompany, DateTime date)
        {
            var voyage = this.voyageRepository.Single(v => v.VesselInCompany.CompanyId == company.Id && v.VesselInCompanyId == vesselInCompany.Id && v.StartDate <= date && (v.EndDate.HasValue ? v.EndDate : DateTime.MaxValue) >= date);

            return voyage;
        }

        public List<Voyage> FindVoyages(long companyId, long vesselInCompanyId, DateTime date)
        {
            var voyages = this.voyageRepository.Find(v => v.IsActive && v.VesselInCompany.CompanyId == companyId && v.VesselInCompanyId == vesselInCompanyId && v.StartDate <= date && (v.EndDate.HasValue ? v.EndDate : DateTime.MaxValue) >= date).ToList();

            return voyages;
        }

        public Voyage GetVoyageContainingDuration(Company company, VesselInCompany vesselInCompany, DateTime startDateTime, DateTime endDateTime)
        {
            if (startDateTime > endDateTime)
                throw new InvalidArgument("Invlaid duration.", "EndDateTime");

            var voyage = this.voyageRepository.Single(v => v.VesselInCompany.CompanyId == company.Id && v.VesselInCompanyId == vesselInCompany.Id &&
                v.StartDate <= startDateTime && v.EndDate >= endDateTime);

            return voyage;
        }

        public FuelReport GetVoyageValidEndOfVoyageFuelReport(long voyageId)
        {
            var isFuelReportSubmitted = new IsFuelReportSubmitted();

            var endOfVoyageFuelReports = this.fuelReportRepository.Find(
                Extensions.And(isFuelReportSubmitted.Predicate, fr => fr.FuelReportType == FuelReportTypes.EndOfVoyage && voyageId == fr.VoyageId));

            if(endOfVoyageFuelReports.Count > 1)
            {
                throw new BusinessRuleException("", "There are more than one submitted EOV report for voyage " + endOfVoyageFuelReports.First().Voyage.VoyageNumber);
            }

            return endOfVoyageFuelReports.SingleOrDefault();
        }

        public void UpdateVoyageFromRotationData(long voyageId)
        {
            var rotationVoyage = rotationVoyageRepository.Single(rv => rv.Id == voyageId);

            var fuelVoyage = voyageRepository.Single(v=>v.Id == voyageId);

            if (rotationVoyage == null) return;

            if(fuelVoyage == null) throw new ObjectNotFound("FuelVoyage", voyageId);

            fuelVoyage.Update(rotationVoyage);
        }

        public void UpdateVoyageFromRotationData(Voyage voyage)
        {
            var rotationVoyage = rotationVoyageRepository.Single(rv => rv.Id == voyage.Id);

            if (rotationVoyage == null) return;

            voyage.Update(rotationVoyage);
        }

        //================================================================================
    }
}