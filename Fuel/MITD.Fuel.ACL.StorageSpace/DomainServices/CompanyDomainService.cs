using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.Contracts.Adapters;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Services.AntiCorruption.Contracts;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices
{
    public class CompanyDomainService : ICompanyDomainService
    {
        private readonly IRepository<Company> companyRepository;

        public CompanyDomainService(
            ICompanyAntiCorruptionAdapter companyAntiCorruptionAdapter,
            IRepository<Company> companyRepository
            )
        {
            CompanyAntiCorruptionAdapter = companyAntiCorruptionAdapter;
            this.companyRepository = companyRepository;
        }


        private IAntiCorruptionAdapter<Company, EnterprisePartyDto> Adapter { get; set; }
        private ICompanyAntiCorruptionAdapter CompanyAntiCorruptionAdapter { get; set; }

        #region ICompanyDomainService Members





        public List<Company> Get(List<long> IDs)
        {
            return this.GetAll().Where(c => IDs.Contains(c.Id)).ToList();
        }

        public List<VesselInCompany> GetCompanyVessels(long enterpriseId)
        {
            var company = this.Get(enterpriseId);

            if (company == null)
                throw new ObjectNotFound("Company", enterpriseId);

            return company.VesselsOperationInCompany;
        }


        public List<Company> GetAll()
        {
            return companyRepository.GetAll().ToList();
        }

        public bool CanBePoGood(long goodId, long companyId)
        {
            return true;
            //return this.CompanyAntiCorruptionAdapter.CanBePoGood(goodId, companyId);
        }

        public void IsValid(long ownerId)
        {
        }

        public bool GoodHaveValidSuplier(long companyId, long goodId,
                                                        long supplierId)
        {
            return true;
        }

        public bool CompanyHaveAnySupliersForGood(long companyId, long goodId)
        {
            return true;
        }

        public bool CompanyHaveAnyTransporterForGood(long companyId, long goodId,
                                                             long transporterId)
        {
            return true;
        }

        public bool GoodHaveValidTransporter(long goodId, long partyGoodId)
        {
            return true;
        }

        public Company Get(long id)
        {
            var fetchStrategy = new SingleResultFetchStrategy<Company>()
                .Include(c => c.VesselsOperationInCompany).Include(c => c.Users).Include(c => c.Goods);

            return companyRepository.First(c => c.Id == id, fetchStrategy);
        }
        
        public List<Company> GetUserCompanies(long userId)
        {
            var fecthStrategy = new ListFetchStrategy<Company>()
                .Include(p => p.Fleet)
                .Include(p => p.Goods)
                .Include(p => p.Users)
                .Include(p => p.VesselsOperationInCompany);

            return companyRepository.Find(c => c.Users.Any(u => u.Id == userId), fecthStrategy).ToList();
        }

        #endregion
    }
}