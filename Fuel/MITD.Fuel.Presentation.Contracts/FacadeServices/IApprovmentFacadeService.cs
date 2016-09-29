
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
    public interface IWorkflowFacadeService : IFacadeService
    {
        ApprovmentDto MoveToNextStep(ApprovmentDto entity);

        void ApplyBatchAction(ApprovmentDto entity);
    }
}
