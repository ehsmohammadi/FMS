using System;
using System.Windows;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface IFuelController:IApplicationController
    {
        void getLogonUser();
        UserStateDTO CurrentUserState { get; set; }
        void HandleException(Exception exp);
        void GetRemoteInstance<T>(Action<T, Exception> action) where T : class;
        FuelUserDto GetCurrentUser();
        void Login(Action action);

        void CheckToken();

        void LogOut();

    }
}
