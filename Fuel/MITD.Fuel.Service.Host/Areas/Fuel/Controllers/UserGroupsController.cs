using System.Collections.Generic;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    [Authorize]
    public class UserGroupsController : ApiController
    {
        private readonly IUserFacadeService userServiceFacade;

        public UserGroupsController(
            IUserFacadeService userServiceFacade
            )
        {
            this.userServiceFacade = userServiceFacade;
        }

        public List<UserGroupDtoWithActions> GetAllUserGroups()
        {
            return userServiceFacade.GetAllUserGroups();
        }
        public UserGroupDto GetUserGroup(string partyName)
        {
            return userServiceFacade.GetUserGroupByName(partyName);
        }
        public UserGroupDto PostUserGroup(UserGroupDto userGroup)
        {
            return userServiceFacade.AddUserGroup(userGroup);
        }
        public UserGroupDto PutUserGroup(UserGroupDto userGroup)
        {
            return userServiceFacade.UpdateUserGroup(userGroup);
        }
        public string DeleteUserGroup(string partyName)
        {
            return userServiceFacade.DeleteUserGroup(partyName);
        }
        
    }
}