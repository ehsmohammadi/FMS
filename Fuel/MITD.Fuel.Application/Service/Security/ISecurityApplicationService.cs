using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.FuelSecurity.Domain.Model;
using MITD.Services.Application;

namespace MITD.Fuel.Application.Service.Security
{
    public interface ISecurityApplicationService : IApplicationService
    {
        bool IsAuthorize(List<ActionType> userActionTypes, List<ActionType> methodRequiredActionTypes);
        List<ActionType> GetAllAuthorizedActions(List<User> users);

        User GetUser(long id);
        User GetLogonUser();

        User AddUser(string firstName, string lastName, bool isActive, string email,
            Dictionary<int, bool> customActions, List<long> groups,string userName);

        User UpdateUser(long companyId,long id, string firstName, string lastName, bool isActive, string email,
           Dictionary<int, bool> customActions, List<long> groups);

        Group UpdateGroup(long id, string description, Dictionary<int, bool> customActions);
        Group AddGroup(long id, string partyName, string description, Dictionary<int, bool> customActions);
        User AddUpdate(string partyName, string firstName, string lastName, string email);
        User UpdateUserAccess(long id, Dictionary<int, bool> customActions);
    }
}
