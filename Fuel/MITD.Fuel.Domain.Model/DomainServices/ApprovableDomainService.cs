//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MITD.Fuel.Domain.Model.DomainObjects;
//using MITD.Fuel.Domain.Model.Enums;
//using MITD.Fuel.Domain.Model.IDomainServices;
//using MITD.Fuel.Domain.Model.Repositories;
//
//namespace MITD.Fuel.Domain.Model.DomainServices
//{
//
//  public   class ApprovableDomainService : IApprovableDomainService
//    {
//      private readonly IWorkflowLogRepository _workflowLogRepository;
//
//      public ApprovableDomainService(IWorkflowLogRepository workflowLogRepository)
//      {
//          _workflowLogRepository = workflowLogRepository;
//      }
//
//      public bool  IsOnFinalApprovedStates(WorkflowEntities actionEntities, long entityId)
//      {
//          return _workflowLogRepository.Count(c => c.WorkflowEntity == actionEntities &&
//              c.EntityId == entityId && c.WorkflowAction==WorkflowActions.FinalApprove) > 0;
//      }
//      
//      public bool IsOnMiddleApprovedStates(WorkflowEntities actionEntities, long entityId)
//      {
//
//          return _workflowLogRepository.Count(c => c.WorkflowEntity == actionEntities &&
//                                                       c.EntityId == entityId &&
//                                                       c.WorkflowAction != WorkflowActions.FinalApprove) > 0;
//      }
//
//      public List<WorkflowLog> Get(List<long> IDs)
//      {
//          throw new NotImplementedException();
//      }
//
//      public WorkflowLog Get(long id)
//      {
//          throw new NotImplementedException();
//      }
//
//      public List<WorkflowLog> GetAll()
//      {
//          throw new NotImplementedException();
//      }
//    }
//}
