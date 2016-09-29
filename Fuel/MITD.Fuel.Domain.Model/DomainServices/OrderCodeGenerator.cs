using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.Factories
{
    public class OrderCodeGenerator : IOrderCodeGenerator
    {
        private readonly IOrderRepository repository;

        #region IOrderCodeGenerator Members

        public OrderCodeGenerator( IOrderRepository repository)
        {
            this.repository = repository;
        }

        public string GenerateNewCode()
        {
            var last = repository.First(c => c.Id > 0, new SingleResultFetchStrategy<Order>().OrderByDescending(c => c.Id));
            if (last == null)
                return "1";
            return (last.Id + 1).ToString();
        }

        #endregion
    }
}