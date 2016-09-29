using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.FuelSecurity.Domain.Model;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
   public interface ISecurityFacadeService : IFacadeService
   {
       bool IsAuthorize(string className, string methodName, ClaimsPrincipal userClaimsPrincipal);

       bool IsAuthorize(ActionEntityTypeEnum workflowEntities, DecisionTypeEnum decisionTypeEnum,
           ClaimsPrincipal userClaimsPrincipal);
       List<ActionType> GetUserAuthorizedActions(ClaimsPrincipal userClaimsPrincipal);
       void AddUpdateUser(ClaimsPrincipal userClaimsPrincipal);
       
       User GetLogonUser();

       UserDto GetUser(string userName);
       List<ActionTypeDto> GetUserAuthorizedActions(string userName);
       List<ActionTypeDto> GetUserAuthorizedActionsById(long userId);
   }
}
