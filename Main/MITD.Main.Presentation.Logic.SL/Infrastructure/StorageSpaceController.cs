using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Browser;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Main.Presentation.Logic.SL.ServiceWrapper;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using Newtonsoft.Json.Linq;
using MITD.Fuel.Presentation.Contracts.SL.Views;

namespace MITD.Main.Presentation.Logic.SL.Infrastructure
{

    public partial class FuelController : ApplicationController, IFuelController, IApplicationController
    {
        #region ctor


        public FuelController(
            IViewManager viewManager,
            IEventPublisher eventPublisher,
            IDeploymentManagement deploymentManagement, IUserSecurityServiceWrapper userService, IUserProvider userProvider)
            : base(viewManager, eventPublisher, deploymentManagement)
        {

            this.userService = userService;
            this.userProvider = userProvider;

            //viewManager
            if (viewManager == null)
                throw new Exception("ViewManager can not be null");

            this.ViewManager = viewManager;
            //eventPublisher
            if (eventPublisher == null)
                throw new Exception("eventPublisher can not be null");

            this.EventPublisher = eventPublisher;

            //deploymentManagement
            if (deploymentManagement == null)
                throw new Exception("deploymentManagement can not be null");

            this.DeploymentManagement = deploymentManagement;



        }

        #endregion

        #region props

        public FuelUserDto CurrentUser { get; set; }

        private IUserSecurityServiceWrapper userService;

        private IUserProvider userProvider;
        public UserStateDTO LoggedInUserState { get; set; }

        public UserStateDTO CurrentUserState { get; set; }
        public IViewManager ViewManager { get; set; }

        public IEventPublisher EventPublisher { get; set; }

        public IDeploymentManagement DeploymentManagement { get; set; }

        #endregion

        public void getLogonUser()
        { 
            
            userService.GetLogonUser((res, exp) => BeginInvokeOnDispatcher(() =>
            {
                if (exp == null)
                {

                    CurrentUserState = res;
                    LoggedInUserState = res;
                    Publish(new MainWindowUpdateArgs());

                }
                else
                {
                    HideBusyIndicator();
                    HandleException(exp);
                }
            }));
        }


        public FuelUserDto GetCurrentUser()
        {
            return CurrentUser;
        }



        public void HandleException(Exception exp)
        {
            if (exp.Data.Contains("error"))
            {
                try
                {
                    var exceptionMessageDto = Newtonsoft.Json.JsonConvert.DeserializeObject<ExceptionMessageDto>(exp.Data["error"] as string);
                    viewManager.ShowMessage("خطا : \n" + exceptionMessageDto.Message, this);
                }
                catch
                {
                    var stack = (exp.StackTrace != null) ? exp.StackTrace.ToString() : "";
                    var inner = (exp.InnerException != null) ? exp.InnerException.Message.ToString() : "";
                    //var inner1 = (exp.InnerException.InnerException.Message != null) ? exp.InnerException.InnerException.Message.ToString() : "";
                    viewManager.ShowMessage("خطا : \n" + exp.Message + "\n" +
                                            "Stacktrace: " + stack + "\n" + "Inner: " + inner, this);
                }
            }
            else
            {
                // viewManager.ShowMessage(exp.Message, this);
                viewManager.ShowMessage("خطای سیستمی : \n" + exp.Message, this);
            }

            //viewManager.ShowMessage(exp.Data["error"].ToString(), this);
        }


        public void GetRemoteInstance<T>(Action<T, Exception> action) where T : class
        {
            deploymentManagement.AddModule(typeof(T),
                res =>
                {
                    action(ServiceLocator.Current.GetInstance(typeof(T)) as T, null);
                },
                exp => { action(null, exp); });
        }

        public void Login(Action action)
        {
            ShowBusyIndicator("در حال ورود به سامانه...");
            this.userService.GetToken((res, exp) => BeginInvokeOnDispatcher(() =>
            {
                if (exp == null)
                {
                    //userProvider.SamlToken = res;
                    //var json = JObject.Parse(res);
                    //var expiresIn = int.Parse(json["expires_in"].ToString());
                    //var expiration = DateTime.UtcNow.AddSeconds(expiresIn);
                    getSessionToken(res,
                                    () => this.getCurrentFuelUser(action, DateTime.Now));
                }
                else
                {
                    HandleException(exp);
                }
            }));
        }

        public void CheckToken()
        {
            this.userService.GetToken((res, exp) => BeginInvokeOnDispatcher(() =>
            {
                if (exp == null)
                {
                    //userProvider.SamlToken = res;
                    //var json = JObject.Parse(res);
                    //var expiresIn = int.Parse(json["expires_in"].ToString());
                    //var expiration = DateTime.UtcNow.AddSeconds(expiresIn);
                    getSessionToken(res, () => this.getCurrentFuelUser(() =>
                    {
                    }, DateTime.Now));
                }
                else
                {
                    HandleException(exp);
                }
            }));
        }

        private void getSessionToken(string token, Action action, string newCurrentWorkListUser = "")
        {
            userService.GetSessionToken((res, exp) => BeginInvokeOnDispatcher(() =>
            {
                HideBusyIndicator();
                if (exp == null)
                {
                    var json = JObject.Parse(res);
                    var sessionToken = json["access_token"].ToString();
                    var expiresIn = int.Parse(json["expires_in"].ToString());
                    var expiration = DateTime.UtcNow.AddSeconds(expiresIn);
                    userProvider.Token = sessionToken;
                    ApiConfig.Headers = ApiConfig.CreateHeaderDic(userProvider.Token);
                    getCurrentFuelUser(() => { }, expiration);
                    action();
                }

                else
                {

                    HandleException(exp);
                }
            }), token, newCurrentWorkListUser);
        }


        private void getCurrentFuelUser(Action action, DateTime dateTime)
        {
            userService.GetCurrentFuelUser((res, exp) => BeginInvokeOnDispatcher(() =>
            {
                HideBusyIndicator();
                if (exp == null)
                {
                    CurrentUser = res;
                    this.Publish<MainWindowArg>(new MainWindowArg() { UserDto = res.User, TimeOut = dateTime });
                    action();
                }

                else
                {

                    HandleException(exp);
                }
            }));
        }

        [ScriptableMember]
        void ForceLogin()
        {

            Login(() => { });
            viewManager.CloseAllTabs();
        }

        public void LogOut()
        {
            userProvider.Token = null;
            userProvider.SamlToken = null;
            CurrentUser = null;
            ApiConfig.Headers = new Dictionary<string, string>();
            HtmlPage.Window.Navigate(new Uri("Security/LogOut", UriKind.Relative));
        }
        }
    }
