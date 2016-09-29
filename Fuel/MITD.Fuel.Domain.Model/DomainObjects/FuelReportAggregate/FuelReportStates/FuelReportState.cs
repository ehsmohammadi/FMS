using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates
{
    public abstract class FuelReportState
    {
        public States State { get; private set; }

        protected readonly IFuelReportStateFactory FuelReportStateFactory;

        protected FuelReportState(IFuelReportStateFactory fuelReportStateFactory, States state)
        {
            this.FuelReportStateFactory = fuelReportStateFactory;
            this.State = state;
        }

        public virtual void Approve(FuelReport fuelReport, IApprovableFuelReportDomainService approveService, long approverId)
        {
            throw new InvalidStateException("Approve", string.Format("Cannot Approve {0} State", fuelReport.State.ToString()));
        }
        public virtual void Reject(FuelReport fuelReport, long approverId)
        {
            throw new InvalidStateException("Reject", string.Format("Cannot Reject {0} State", fuelReport.State.ToString()));
        }
        public virtual void Cancel(FuelReport fuelReport, long approverId)
        {
            throw new InvalidStateException("Cancel", string.Format("Cannot Cancel {0} State", fuelReport.State.ToString()));
        }
    }
}