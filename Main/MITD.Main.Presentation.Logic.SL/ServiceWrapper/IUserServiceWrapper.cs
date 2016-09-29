using System;
using System.Collections.Generic;
using MITD.Presentation;
using MITD.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts;

namespace MITD.Main.Presentation.Logic.SL.ServiceWrapper
{
    public interface IUserSecurityServiceWrapper : IServiceWrapper
    {
        void ChangePassWord(Action<object, Exception> action, string newPassWord, string oldPassWord);
        void GetAllUsers(Action<PageResultDto<UserDTOWithActions>, Exception> action, int pageSize,
                                 int pageIndex, Dictionary<string, string> sortBy, UserCriteria criteria);
        void GetAllUsers(Action<List<UserDto>, Exception> action);

        void GetAllUserActionTypes(Action<List<ActionTypeDto>, Exception> action, string userName, bool isGroup,
            long groupId);

        void GetUser(Action<UserDto, Exception> action, string username);
        void AddUser(Action<UserDto, Exception> action, UserDto userDto);
        void UpdateUser(Action<UserDto, Exception> action, UserDto userDto);
        void DeleteUser(Action<string, Exception> action, string username);

        void GetAllUserGroups(Action<List<UserGroupDtoWithActions>, Exception> action);
        void GetUserGroup(Action<UserGroupDto, Exception> action, string groupName);
        void AddUserGroup(Action<UserGroupDto, Exception> action, UserGroupDto userGroupDto);
        void UpdateUserGroup(Action<UserGroupDto, Exception> action, UserGroupDto userGroupDto);
        void DeleteUserGroup(Action<string, Exception> action, string groupName);

        void GetToken(Action<string, Exception> action, string userName, string passWord);
        void GetToken(Action<string, Exception> action);
        void GetSessionToken(Action<string, Exception> action, string token, string newCurrentWorkListUser);
        void GetLogonUser(Action<UserStateDTO, Exception> action);
        void LogoutUser(Action<string, Exception> action);
        void GetAllUserGroupsDescriptions(Action<List<UserGroupDescriptionDto>, Exception> action);
        void GetAllActionTypes(Action<List<ActionTypeDto>, Exception> action);

        void ChangeCurrentWorkListUserName(Action<string, Exception> action, string currentUsername,
            string newUserName);

        void GetAllUserDescriptions(Action<PageResultDto<UserDescriptionDTO>, Exception> action, int pageSize,
                                 int pageIndex, Dictionary<string, string> sortBy, UserCriteria criteria);

        void GetCurrentFuelUser(Action<FuelUserDto, Exception> action);
        void UpdateUserAccess(Action<UserGroupDto, Exception> action, long userId, Dictionary<int, bool> actionList);
    }
}
