using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Main.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;

namespace MITD.Main.Presentation.Logic.SL
{
    public class MainWindowVM : WorkspaceViewModel, IEventHandler<MainWindowArg>
    {
        #region Fields

        ReadOnlyObservableCollection<CommandViewModel> _fuelcommands;
        ReadOnlyObservableCollection<CommandViewModel> _usercommands;
        ReadOnlyObservableCollection<CommandViewModel> reportCommands;
        ReadOnlyObservableCollection<CommandViewModel> _basicInfoCommands;
        private ReadOnlyObservableCollection<CommandViewModel> _inventoryCommands;
        private ReadOnlyObservableCollection<CommandViewModel> _logestic;
        private ReadOnlyObservableCollection<CommandViewModel> _financial;

        private UserDto _userState;
        public UserDto UserState
        {
            get { return _userState; }
            set { this.SetField(p => p.UserState, ref _userState, value); }
        }


        private DateTime _timeToLogOut;
        public DateTime TimeToLogOut
        {
            get { return _timeToLogOut; }
            set { this.SetField(p => p.TimeToLogOut, ref _timeToLogOut, value); }
        }

        IFuelController controller;

        private CommandViewModel _signOutCommand;
        public CommandViewModel SignOutCommand
        {
            get
            {
                _signOutCommand = new CommandViewModel("خروج ...", new DelegateCommand(() =>
                                                                                       {

                                                                                           LogOut();

                                                                                       }));
                return _signOutCommand;
            }

        }




        #endregion // Fields

        #region Constructor

        public MainWindowVM()
        {
            DisplayName = "سامانه";

        }


        public MainWindowVM(IFuelController controller)
        {
            DisplayName = "سامانه";
            this.controller = controller;

        }
        #endregion

        #region Commands

        public ReadOnlyObservableCollection<CommandViewModel> UserCommands
        {
            get
            {
                if (_usercommands == null)
                {

                    _usercommands = UserCreateMenu();
                }
                return _usercommands;
            }
        }
        
        
        public ReadOnlyObservableCollection<CommandViewModel> BasicInfoCommands
        {
            get
            {
                if (_basicInfoCommands == null)
                {

                    _basicInfoCommands = BasicInfoCreateMenu();
                }
                return _basicInfoCommands;
            }
        }
        public ReadOnlyObservableCollection<CommandViewModel> FuelCommands
        {
            get
            {
                if (_fuelcommands == null)
                {

                    _fuelcommands = FuelCreateMenu();
                }
                return _fuelcommands;
            }
        }

        public ReadOnlyObservableCollection<CommandViewModel> InventoryCommands
        {
            get
            {
                if (_inventoryCommands == null)
                {

                    _inventoryCommands = InventoryCreateMenu();
                }
                return _inventoryCommands;
            }
        }

        public ReadOnlyObservableCollection<CommandViewModel> Logestic
        {
            get
            {
                if (_logestic == null)
                {

                    _logestic = LogesticCreateMenu();
                }
                return _logestic;
            }
        }

        public ReadOnlyObservableCollection<CommandViewModel> Financial
        {
            get
            {
                if (_financial == null)
                {

                    _financial = FinancialCreateMenu();
                }
                return _financial;
            }
        }
        public ReadOnlyObservableCollection<CommandViewModel> ReportCommands
        {
            get
            {
                if (reportCommands == null)
                {

                    reportCommands = CreateReportMenu();
                }
                return reportCommands;
            }
        }

        public ReadOnlyObservableCollection<CommandViewModel> BasicInfoCreateMenu()
        {

            var menuHelper = new MenuHelper(controller);

            menuHelper.AddMenuItem(new MenuItem<IFinancialAccountController>("حساب های معین", "ShowList"));
            menuHelper.AddMenuItem(new MenuItem<IVesselController>("شناورها (Vessels)", "ShowList"));
            menuHelper.AddMenuItem(new MenuItem<ICurrencyController>("ارزها و نرخ تبدیل", "ShowExchangesList"));

            return menuHelper.ExecuteMenu();
        }
        public ReadOnlyObservableCollection<CommandViewModel> LogesticCreateMenu()
        {

            var menuHelper = new MenuHelper(controller);

            menuHelper.AddMenuItem(new MenuItem<IOrderController>("سفارشات (Order)", "ShowList"));
            menuHelper.AddMenuItem(new MenuItem<IInvoiceController>("صورت حساب (Invoice)", "ShowList"));
            return menuHelper.ExecuteMenu();
        }

        public ReadOnlyObservableCollection<CommandViewModel> InventoryCreateMenu()
        {

            var menuHelper = new MenuHelper(controller);

            menuHelper.AddMenuItem(new MenuItem<IInventoryTransactionController>("عملیات انبارداری (Inventory Transaction)", "ShowList"));

            return menuHelper.ExecuteMenu();
        }

        public ReadOnlyObservableCollection<CommandViewModel> FinancialCreateMenu()
        {

            var menuHelper = new MenuHelper(controller);

            menuHelper.AddMenuItem(new MenuItem<IVoucherController>("اسناد مالی", "ShowVoucherList"));
            menuHelper.AddMenuItem(new MenuItem<IVoucherSetingController>("تنظیمات اسناد مالی", "ShowVoucherSeting"));
            return menuHelper.ExecuteMenu();
        }

        public ReadOnlyObservableCollection<CommandViewModel> FuelCreateMenu()
        {

            var menuHelper = new MenuHelper(controller);
            menuHelper.AddMenuItem(new MenuItem<ICharterController>("Charter In", "ShowCharterInList"));
            menuHelper.AddMenuItem(new MenuItem<IFuelReportController>("گزارش سوخت (Fuel Report)", "ShowList"));
            menuHelper.AddMenuItem(new MenuItem<IVoyageController>("گزارش سفرها (Voyages Report)", "Show"));
            menuHelper.AddMenuItem(new MenuItem<IOffhireController>("Offhire", "ShowList"));
            menuHelper.AddMenuItem(new MenuItem<IScrapController>("Scrap", "ShowList"));
            menuHelper.AddMenuItem(new MenuItem<ICharterController>("Charter Out", "ShowCharterOutList"));

            return menuHelper.ExecuteMenu();
        }
        public ReadOnlyObservableCollection<CommandViewModel> CreateReportMenu()
        {
            var menuHelper = new MenuHelper(controller);
            menuHelper.AddMenuItem(new MenuItem<ICardexReportController>("کاردکس حسابداری", "ShowAccountingCardexReport"));
            menuHelper.AddMenuItem(new MenuItem<ICardexReportController>("کاردکس انبارداری", "ShowInventoryCardexReport"));
            menuHelper.AddMenuItem(new MenuItem<IVoucherReportController>("سند", "ShowReport"));
            menuHelper.AddMenuItem(new MenuItem<IInventoryQuantityReportController>("موجودی هولدینگ", "ShowHoldingReport"));
            menuHelper.AddMenuItem(new MenuItem<IInventoryQuantityReportController>("موجودی شرکت/کشتی", "ShowCompanyReport"));
            menuHelper.AddMenuItem(new MenuItem<IPeriodicalFuelStatisticsReportController>("آمار دوره ای هولدینگ", "ShowHoldingReport"));
            menuHelper.AddMenuItem(new MenuItem<IPeriodicalFuelStatisticsReportController>("آمار دوره ای شرکت/کشتی", "ShowCompanyReport"));

            menuHelper.AddMenuItem(new MenuItem<IVesselDataReportController>("گزارش شناورها", "ShowReport"));
            menuHelper.AddMenuItem(new MenuItem<IVesselDataReportController>("Speed/Consumption Report", "ShowVesselRunningValuesReport"));
            return menuHelper.ExecuteMenu();
        }
        public ReadOnlyObservableCollection<CommandViewModel> UserCreateMenu()
        {
            var menuHelper = new MenuHelper(controller);
            menuHelper.AddMenuItem(new MenuItem<IUserController>("تغییر رمزعبور", "ShowChangePassWord"));
            menuHelper.AddMenuItem(new MenuItem<IUserController>("کاربران", "ShowUserList"));
            menuHelper.AddMenuItem(new MenuItem<IUserController>("گروه های کاربران", "ShowGroupList"));
            return menuHelper.ExecuteMenu();
        }

        #endregion // Commands

        private void LogOut()
        {
            UserState = null;
            controller.LogOut();
        }

        public void Handle(MainWindowArg eventData)
        {
            UserState = eventData.UserDto;
            //startTokenCheckerTimer();
        }

        private DispatcherTimer tokenCheckerDispatcherTimer;
        
        private void startTokenCheckerTimer()
        {
            if (tokenCheckerDispatcherTimer == null)
            {
                tokenCheckerDispatcherTimer = new DispatcherTimer();
                tokenCheckerDispatcherTimer.Interval = new TimeSpan(0, 3, 0); // Every 4 minitues
                tokenCheckerDispatcherTimer.Tick += this.tokenChecker_Tick;
                tokenCheckerDispatcherTimer.Start();
            }
        }

        private void tokenChecker_Tick(object o, EventArgs sender)
        {
            controller.CheckToken();

            //This is inorder to refresh the
            //var hostingSiteBaseAddress = Application.Current.Host.Source.AbsoluteUri.Replace(Application.Current.Host.Source.AbsolutePath, string.Empty);

            //var reportViewerPageUri = new Uri(hostingSiteBaseAddress + "/Reports/ReportViewer.aspx?ItemPath=VesselReportDataReport", UriKind.Absolute);

            //var request = (HttpWebRequest)System.Net.Browser.WebRequestCreator.ClientHttp.Create(reportViewerPageUri);
            //request.Method = "GET";
            //request.Accept = "application/html";

            //if (ApiConfig.Headers != null)
            //    foreach (var header in ApiConfig.Headers)
            //        request.Headers[header.Key] = header.Value;

            //request.BeginGetResponse(iar2 =>
            //{
            //    WebResponse response = null;
            //    try
            //    {
            //        response = request.EndGetResponse(iar2);
            //    }
            //    catch (Exception exp)
            //    {
            //        return;
            //    }
            //}, null);

            //System.Windows.Browser.HtmlElement element = System.Windows.Browser.HtmlPage.Document.CreateElement("iframe");
            //element.SetAttribute("src", reportViewerPageUri.ToString());
        }
    }
}
