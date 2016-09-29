using System;
using System.Collections.Generic;
using System.Windows;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation.Contracts;
using System.IO;

namespace MITD.Fuel.Presentation.FuelApp.Logic.SL.ServiceWrapper
{
    public class UserServiceWrapper : IUserServiceWrapper
    {
        #region fields

        private string userAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/Users/{0}");

        #endregion

        #region methods

        public void GetUser(Action<UserDto, Exception> action, string username)
        {
            var url = string.Format(userAddressFormatString + "?partyName=" + username);
            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action,
                                                    WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void GetAll(Action<List<UserDto>, Exception> action, string methodName)
        {
            var url = string.Format(userAddressFormatString, string.Empty);

            WebClientHelper.Get<List<UserDto>>(new Uri(url, UriKind.Absolute),
                                                                    (res, exp) => action(res, exp),
                                                                    WebClientHelper.MessageFormat.Json,ApiConfig.Headers
            );
        }

        public void GetAll(Action<PageResultDto<UserDto>, Exception> action, string methodName, int pageSize,
                           int pageIndex)
        {
            var url = string.Format(userAddressFormatString, string.Empty) +
                "?PageSize=" + pageSize + "&PageIndex=" + pageIndex;

            WebClientHelper.Get<PageResultDto<UserDto>>(new Uri(url, UriKind.Absolute),
                                                                    (res, exp) => action(res, exp),
                                                                    WebClientHelper.MessageFormat.Json,ApiConfig.Headers
                );
        }

        public void GetById(Action<UserDto, Exception> action, int id)
        {
            var url = string.Format(userAddressFormatString, id);

            WebClientHelper.Get<UserDto>(new Uri(url, UriKind.Absolute),
                                                     (res, exp) => action(res, exp),
                                                     WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void Add(Action<UserDto, Exception> action, UserDto ent)
        {
            var url = string.Format(userAddressFormatString, string.Empty);

            WebClientHelper.Post<UserDto, UserDto>(new Uri(url, UriKind.Absolute),
                                                                           (res, exp) => action(res, exp), ent,
                                                                           WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void Update(Action<UserDto, Exception> action, UserDto ent)
        {
            var url = string.Format(userAddressFormatString, ent.Id);

            WebClientHelper.Put<UserDto, UserDto>(new Uri(url, UriKind.Absolute),
                                                                          (res, exp) => action(res, exp), ent,
                                                                          WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

       


        public void Delete(Action<string, Exception> action, int id)
        {
            var url = string.Format(userAddressFormatString, id);

            WebClientHelper.Delete(new Uri(url, UriKind.Absolute), (res, exp) => action(res, exp));
        }

        public void GetAllUserGroupsDescriptions(Action<List<UserGroupDescriptionDto>, Exception> action)
        {
            var url = string.Format(userAddressFormatString, string.Empty);
            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action,
                                                    WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }


        #endregion

    }
}
