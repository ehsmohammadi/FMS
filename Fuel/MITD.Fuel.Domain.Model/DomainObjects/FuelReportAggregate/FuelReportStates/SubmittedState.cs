using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates
{
    public class SubmittedState : FuelReportState
    {
        public SubmittedState(IFuelReportStateFactory fuelReportStateFactory)
            : base(fuelReportStateFactory, States.Submitted)
        {
        }


        public override void Approve(FuelReport fuelReport, IApprovableFuelReportDomainService approvableFuelReportDomainService, long approverId)
        {
            fuelReport.Close(this.FuelReportStateFactory.CreateClosedState(), approverId);
        }

        public override void Reject(FuelReport fuelReport, long approverId)
        {
            fuelReport.RejectSubmitted(this.FuelReportStateFactory.CreateSubmitRejectedState(), approverId);
        }
    }
}