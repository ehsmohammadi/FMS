using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using Thinktecture.IdentityModel.WSTrust;
using System.ServiceModel;
using System.IdentityModel.Tokens;
using System.Net.Http;
using System.Text;

namespace MITD.PMS.Service.Host.Controllers
{
 
    public class ActionTypesController : ApiController
    {
        private readonly IUserFacadeService userServiceFacade;
        private ISecurityFacadeService _securityFacadeService;

        public ActionTypesController(IUserFacadeService userServiceFacade,ISecurityFacadeService securityFacadeService)
        {
            this.userServiceFacade = userServiceFacade;
            this._securityFacadeService = securityFacadeService;
        }

        public List<ActionTypeDto> GetAllActionTypes()
        {
            return userServiceFacade.GetAllActionTypes();
        }
        public List<ActionTypeDto> GetAllUserActionTypes(string  userName,bool isGroup,long groupId)
        {

            if (isGroup)
            {
                return userServiceFacade.GetGroupActionType(groupId);
            }
            else
            {
                return _securityFacadeService.GetUserAuthorizedActions(userName);
            }
           
            
        }

        public void Put([FromBody] Dictionary<int,bool> entity,long userId)
        {
            this.userServiceFacade.UpdateUserAccess(userId,entity);
        }
    }

}