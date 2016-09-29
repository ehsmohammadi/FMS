using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using System.Linq;

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class IsFuelReportSubmittedByFinance : SpecificationBase<FuelReport>
    {
        public IsFuelReportSubmittedByFinance()
            : base(
                fr => fr.State == States.Submitted && fr.ApproveWorkFlows.Any(w => w.Active && w.CurrentWorkflowStep.CurrentWorkflowStage == WorkflowStages.FinancialSubmitted)
            )
        {
        }
    }
}