using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates
{
    public class OpenState : FuelReportState
    {
        public OpenState(IFuelReportStateFactory fuelReportStateFactory)
            : base(fuelReportStateFactory, States.Open)
        {
        }

        public override void Approve(FuelReport fuelReport, IApprovableFuelReportDomainService approveService, long approverId)
        {
            approveService.Submit(
                fuelReport,
                this.FuelReportStateFactory.CreateSubmitState(), approverId);
        }

        public override void Cancel(FuelReport fuelReport, long approverId)
        {
            fuelReport.Invalidate(this.FuelReportStateFactory.CreateInvalidState(), approverId);

        }
    }
}