using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices
{
    public class ActivityLocationDomainService : IActivityLocationDomainService
    {
        //TODO: (A.H) Added for Fake data fetch;
        private readonly IRepository<ActivityLocation> activityLocationFakeRepository;

        public ActivityLocationDomainService(
            //TODO: (A.H) Added for Fake data fetch;
            IRepository<ActivityLocation> activityLocationFakeRepository
            )
        {
            //CompanyAntiCorruptionAdapter = companyAntiCorruptionAdapter;
            this.activityLocationFakeRepository = activityLocationFakeRepository;
        }

        ActivityLocation IActivityLocationDomainService.Get(long id)
        {
            return activityLocationFakeRepository.Single(al => al.Id == id);
        }
    }
}