using System;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public class AccountServiceWrapper : IAccountServiceWrapper
    {

        private string hostAccountAddressController;
        public AccountServiceWrapper()
        {
            hostAccountAddressController = ApiConfig.HostAddress + "apiarea/Fuel/Account/{0}";
        }
        public void GetByFilter(Action<PageResultDto<AccountDto>, Exception> action, string name, string code, int pageIndex, int pageSize)
        {

            var uri = String.Format(hostAccountAddressController, string.Empty);
            StringBuilder stringBuilder = new StringBuilder(uri);
            stringBuilder.Append("?name=" + name);
            stringBuilder.Append("&code=" + code);
            stringBuilder.Append("&pageIndex=" + pageIndex);
            stringBuilder.Append("&pageSize=" + pageSize);
          

            WebClientHelper.Get<PageResultDto<AccountDto>>(new Uri(stringBuilder.ToString(), UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);


        }


        public void Add(Action<AccountDto, Exception> action, AccountDto accountDto)
        {
            var uri = String.Format(hostAccountAddressController, string.Empty);
            WebClientHelper.Post<AccountDto, AccountDto>(new Uri(uri, UriKind.Absolute), action, accountDto, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);

        }
    }
}
