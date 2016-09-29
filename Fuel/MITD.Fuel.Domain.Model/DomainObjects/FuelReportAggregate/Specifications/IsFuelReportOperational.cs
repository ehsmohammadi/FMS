using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using System.Linq;

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class IsFuelReportOperational : SpecificationBase<FuelReport>
    {
        public IsFuelReportOperational()
            : base(
                fr =>
                    //(fr.Voyage == null || fr.Voyage.IsActive) &&  //The validity of voyage will be checked during system operations such as submit.
                    (fr.State == States.SubmitRejected || 
                        fr.State == States.Open || 
                        (fr.State == States.Submitted && fr.ApproveWorkFlows.Any(w => w.Active && w.CurrentWorkflowStep.CurrentWorkflowStage == WorkflowStages.Submited))
                    )
            )
        {

        }
    }
}
