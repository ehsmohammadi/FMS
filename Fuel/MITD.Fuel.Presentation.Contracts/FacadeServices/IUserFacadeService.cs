using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
    public interface IUserFacadeService : IFacadeService
    {
        void ChangePassWord(string newPassWord, string oldPassWord);
        //1
        void UpdateUserAccess(long userId, Dictionary<int, bool> actionTyps);
      //2
        List<ActionTypeDto> GetGroupActionType(long groupId);
        List<ActionTypeDto> GetAllUserActionTypes(long userId);
        //1
        UserDto GetUserByUserName(string username);
        //2
        UserGroupDto GetUserGroupByName(string name);

        UserGroupDto AddUserGroup(UserGroupDto userGroupDto);
        UserGroupDto UpdateUserGroup(UserGroupDto userGroupDto);
        string DeleteUserGroup(string name);

       //1
        List<ActionTypeDto> GetAllActionTypes();

        List<UserGroupDtoWithActions> GetAllUserGroups();
       //1
        void Add(UserDto data);
      //1
        void Update(UserDto data);
        void Delete(UserDto data);
        UserDto GetById(int id);
        PageResultDto<UserDto> GetAll(int pageSize, int pageIndex);
        List<UserDto> GetAll();
        void DeleteById(int id);
        UserDto GetUserWithCompany(int id);

        FuelUserDto GetCurrentFuelUser();
        UserStateDTO GetUserState(ClaimsPrincipal u);
        long GetCurrentUserId();

        long GetCurrentFuelUserId();

        long GetCurrentFuelUserCompanyId();

        bool GetCurrentFuelUserAccessToHolding();
        PageResultDto<UserDTOWithActions> GetAllUsers(int pageSize, int pageIndex, string filter);
    }
}
