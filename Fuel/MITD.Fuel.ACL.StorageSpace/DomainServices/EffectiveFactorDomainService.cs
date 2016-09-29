using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices
{
    public class EffectiveFactorDomainService : IEffectiveFactorDomainService
    {

        private readonly IRepository<EffectiveFactor> effectiveFactorFakeRepository;

        public EffectiveFactorDomainService(IRepository<EffectiveFactor> effectiveFactorFakeRepository)
        {
            this.effectiveFactorFakeRepository = effectiveFactorFakeRepository;
        }

        public IEnumerable<EffectiveFactor> GetEffectiveFactors()
        {
            return effectiveFactorFakeRepository.GetAll().ToList();
        }
    }
}
