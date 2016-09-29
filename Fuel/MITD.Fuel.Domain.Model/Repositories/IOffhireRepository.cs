using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;
namespace MITD.Fuel.Domain.Model.Repositories
{
    public interface IOffhireRepository : IRepository<Offhire>
    {
        void SetConfigurator(IEntityConfigurator<Offhire> offhireConfigurator);
    }
}