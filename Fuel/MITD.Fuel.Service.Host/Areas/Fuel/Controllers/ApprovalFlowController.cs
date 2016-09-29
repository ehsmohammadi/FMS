using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class WorkflowController : ApiController
    {
        #region props
        private IWorkflowFacadeService FacadeService { get; set; }
        #endregion

        #region ctor
        public WorkflowController()
        {
            var scope = ServiceLocator.Current.GetInstance<IUnitOfWorkScope>();

            this.FacadeService = ServiceLocator.Current.GetInstance<IWorkflowFacadeService>();
        }

        public WorkflowController(IWorkflowFacadeService facadeService)
            : base()
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public ApprovmentDto Put(long id, [FromBody] ApprovmentDto entity)
        {
            var result = this.FacadeService.MoveToNextStep(entity);
            return result;
        }

        #endregion
    }
}
