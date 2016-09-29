using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class WorkflowStepRepository : EFRepository<WorkflowStep>, IWorkflowStepRepository
    {
         public WorkflowStepRepository(EFUnitOfWork efUnitOfWork)
            : base(efUnitOfWork)
        {
              
        }

         public WorkflowStepRepository(IUnitOfWorkScope iUnitOfWorkScope)
            : base(iUnitOfWorkScope)
        {

        }
    }
}
