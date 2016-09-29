using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.FuelSecurity.Domain.Model.Service
{
    public interface ISecurityServiceChecker
    {
        List<ActionType> GetAllAuthorizedActionTypes(List<User> users);
        List<ActionType> GetAllAuthorizedActionTypes(long userId);
        bool IsActionPossible(long userId, ActionType actionType);

        bool IsAuthorize(List<ActionType> authorizeActions, List<ActionType> actions);

        List<ActionType> GetAllAuthorizedActionTypesForRole(List<string> rols);
    }
}
