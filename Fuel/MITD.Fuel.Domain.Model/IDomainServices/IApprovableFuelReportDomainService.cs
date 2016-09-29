using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IApprovableFuelReportDomainService : IApprovableDomainService
    {
        void Submit(FuelReport fuelReport, FuelReportState entityNewState, long approverId);
    }
}