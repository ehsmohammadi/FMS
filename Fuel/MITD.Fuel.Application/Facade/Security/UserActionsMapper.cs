using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.FuelSecurity.Domain.Model;
//using MITD.Fuel.Presentation.Contracts;
//using MITD.PMSSecurity.Domain;

namespace MITD.Fuel.Application.Facade
{

    public class UserActionsMapper //: BaseMapper<List<ActionType>, ClaimsPrincipal>, IMapper<List<ActionType>, ClaimsPrincipal>
    {


        public  ClaimsPrincipal MapToModel(List<ActionType> entity)
        {
            throw new NotSupportedException("map List<User> to ClaimsPrincipal not supported  ");
        }

        public  List<ActionType> MapToEntity(ClaimsPrincipal user)
        {
            var res = new List<ActionType>();
            var claimUserActions = user.Claims.SingleOrDefault(c => c.Type == "CurrentUserActions");
            if (claimUserActions == null || string.IsNullOrWhiteSpace(claimUserActions.Value))
                return res;
            // var lNameClaim = user.Claims.SingleOrDefault(c => c.Type == "http://identityserver.thinktecture.com/claims/profileclaims/lastname");

            foreach (var actionCode in claimUserActions.Value.Split(','))
            {
               // var actionType =ActionType.FromValue(int.Parse(actionCode));
                var actionType = ActionType.GetActionType(actionCode);
                if (actionType != null)
                    res.Add(actionType.First());
            }
            return res;
        }
    }



}
