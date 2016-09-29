using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IAccountServiceWrapper:IServiceWrapper
    {
        void GetByFilter(Action<PageResultDto<AccountDto>, Exception> action, string name, string code, int pageIndex, int pageSize);
        void Add(Action<AccountDto, Exception> action, AccountDto charterDto);
    }
}
