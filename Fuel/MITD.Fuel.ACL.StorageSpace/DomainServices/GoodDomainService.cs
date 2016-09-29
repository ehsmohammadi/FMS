using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.Contracts.Adapters;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.FakeDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices
{
    public class GoodDomainService : IGoodDomainService
    {
        private readonly IRepository<Good> goodRepository;
        private readonly IRepository<GoodUnit> companyGoodUnitRepository;
        private readonly ICompanyDomainService companyDomainService;
        public IGoodAntiCorruptionAdapter Adapter { get; set; }

        public GoodDomainService(IGoodAntiCorruptionAdapter antiCorruptionAdapter, IRepository<Good> goodRepository, ICompanyDomainService companyDomainService, IRepository<GoodUnit> companyGoodUnitRepository)
        {
            this.goodRepository = goodRepository;
            this.companyDomainService = companyDomainService;
            this.companyGoodUnitRepository = companyGoodUnitRepository;
            this.Adapter = antiCorruptionAdapter;
        }

        public List<Good> Get(List<long> IDs)
        {
            //            var data = this.Adapter.Get(IDs);
            //            return data;
            return goodRepository.Find(c => IDs.Contains(c.Id)).ToList();
        }

        public Good Get(long id)
        {
            return goodRepository.Single(c => c.Id == id);

            //            var data = this.Adapter.Get(id);
            //            if (data == null)
            //                throw new ObjectNotFound("Good");
            //            return data;
        }


        public List<Good> GetCompanyGoods(long companyId)
        {
            return goodRepository.Find(c => c.CompanyId == companyId).ToList();
        }
        public IEnumerable<Good> GetCompanyGoodsWithUnits(long companyId)
        {
            return goodRepository.Find(c => c.CompanyId == companyId);
        }

        public void GetGoodSuppliersAndTransporters(long goodId, List<Company> supplierCompanies, List<Company> transporterCompanies)
        {
            supplierCompanies = companyDomainService.GetAll();
            transporterCompanies = companyDomainService.GetAll();
        }

        public List<Company> GetGoodSuppliers(long goodId)
        {
            return companyDomainService.GetAll();
        }

        public List<Company> GetGoodTransporters(long goodId)
        {
            return companyDomainService.GetAll().ToList();
        }

        public bool CanBeOrderAmountOfGood(long goodId, double quantity, List<string> exceptionList)
        {
            return true;
        }

        public bool CanBeOrderThisGood(long goodId)
        {
            return true;
        }

        public GoodFullInfo GetGoodInfoes(long companyId, long goodId)
        {
            //TODO: Must be implemented properly.
            return new GoodFullInfo
        {
            Good = goodRepository.Single(
                c => c.Id == goodId && c.CompanyId == companyId),
            CompanyGoodUnits = companyGoodUnitRepository.Find(
                c => c.GoodId == goodId).ToList(),
            GoodSuppliers = companyDomainService.GetAll(),
            GoodTransporters = companyDomainService.GetAll(),
            CanBeOrderedThisGood = true

        };
        }

        public List<Good> GetAll()
        {
            return goodRepository.GetAll().ToList();
        }

        public List<Good> GetMandatoryVesselGoods(long vesselInCompanyId, System.DateTime date)
        {
            //TODO: Must be implemented properly.
            return goodRepository.GetAll().ToList();
        }

        public Good GetGoodWithUnitAndMainUnit(long goodId, long goodUnitId)
        {
            var good = goodRepository.FindByKey(goodId);
            return good;
        }

        public Good FindGood(long companyId, long sharedGoodId)
        {
            var good = goodRepository.Single(g => g.SharedGoodId == sharedGoodId && g.CompanyId == companyId);
            return good;
        }


        public Good GetGoodWithUnit(long companyId, long goodId)
        {
            var fetchStrategy = new SingleResultFetchStrategy<Good>().Include(p => p.GoodUnits);
            return goodRepository.Single(
                c => c.Id == goodId && c.CompanyId == companyId, fetchStrategy);
        }
    }
}
