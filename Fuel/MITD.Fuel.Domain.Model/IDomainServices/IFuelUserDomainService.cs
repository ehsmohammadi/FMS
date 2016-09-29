using MITD.Domain.Model;
namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IFuelUserDomainService : IDomainService
    {
        long GetCurrentFuelUserId();
    }
}