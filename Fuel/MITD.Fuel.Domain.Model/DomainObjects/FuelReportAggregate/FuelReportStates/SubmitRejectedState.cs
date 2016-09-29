using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates
{
    public class SubmitRejectedState : FuelReportState
    {
        public SubmitRejectedState(IFuelReportStateFactory fuelReportStateFactory)
            : base(fuelReportStateFactory, States.SubmitRejected)
        {

        }

        public override void Approve(FuelReport fuelReport, IApprovableFuelReportDomainService approveService, long approverId)
        {
            approveService.Submit(fuelReport, this.FuelReportStateFactory.CreateSubmitState(), approverId);
        }

        public override void Cancel(FuelReport fuelReport, long approverId)
        {
            fuelReport.Invalidate(this.FuelReportStateFactory.CreateInvalidState(), approverId);
        }
    }
}