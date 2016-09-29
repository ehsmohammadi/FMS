using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IActivityLocationDomainService : IDomainService
    {
        ActivityLocation Get(long id);
    }
}