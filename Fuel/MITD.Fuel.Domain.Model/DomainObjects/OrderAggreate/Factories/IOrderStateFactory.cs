using MITD.Fuel.Domain.Model.Factories;

namespace MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate
{
    public interface IOrderStateFactory : IFactory
    {
        OrderState CreateSubmitState();
        OrderState CreateOpenState();
        OrderState CreateCloseState();
        OrderState CreateCancelState();
        OrderState CreateSubmitRejectedState();
    }
}