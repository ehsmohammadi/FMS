using System.Collections.Generic;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;
namespace MITD.Fuel.ACL.StorageSpace.DomainServices
{
    public class TankDomainService : ITankDomainService
    {

        //TODO: (A.H) Added for Fake data fetch;
        private readonly IRepository<Tank> tankRepository;

        public TankDomainService(IRepository<Tank> tankRepository)
        {
            this.tankRepository = tankRepository;
        }

        public Tank Get(long id)
        {
            return tankRepository.Single(t => t.Id == id);
        }
    }
}