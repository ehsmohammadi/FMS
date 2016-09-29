using System;
using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IUserServiceWrapper : IServiceWrapper
    {
     
         void GetUser(Action<UserDto, Exception> action, string username);
        void GetAll(Action<List<UserDto>, Exception> action, string methodName);

        void GetAll(Action<PageResultDto<UserDto>, Exception> action, string methodName, int pageSize,
                         int pageIndex);

        void GetById(Action<UserDto, Exception> action, int id);

        void Add(Action<UserDto, Exception> action, UserDto ent);

        void Update(Action<UserDto, Exception> action, UserDto ent);

        void Delete(Action<string, Exception> action, int id);

        void GetAllUserGroupsDescriptions(Action<List<UserGroupDescriptionDto>, Exception> action);
    }
}
