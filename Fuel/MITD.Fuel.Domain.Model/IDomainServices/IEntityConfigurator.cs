namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IEntityConfigurator<T>
    {
        T Configure(T invoice);
        
    }
}