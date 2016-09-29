using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IEffectiveFactorDomainService : IDomainService
    {
        IEnumerable<EffectiveFactor> GetEffectiveFactors();
    }

 
}