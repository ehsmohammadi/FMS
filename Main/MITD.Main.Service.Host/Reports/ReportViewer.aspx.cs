using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel.Configuration;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using MITD.Presentation;
using Thinktecture.IdentityModel.Extensions;

namespace MITD.Main.Service.Host.Reports
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        Dictionary<string, string> httpHeaders = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    httpHeaders.Add("Authorization", "SAML " + ((this.User.Identity as ClaimsIdentity).BootstrapContext as BootstrapContext).SecurityToken.ToTokenXmlString());
                }
                catch
                {
                    FederatedAuthentication.WSFederationAuthenticationModule.RedirectToIdentityProvider(System.Configuration.ConfigurationManager.AppSettings["ida:Realm"],
                        this.Request.Url.ToString(), false);

                    return;

                    MainReportViewer.Visible = false;

                    lblExpiredSessionDescription.Visible =
                        lblExpiredSessionTitle.Visible = true;

                    return;
                }
                var reportServerUrlAddress = System.Configuration.ConfigurationManager.AppSettings["ReportServerUrl"];
                var reportsRootPathAddress = System.Configuration.ConfigurationManager.AppSettings["ReportsRootPath"];
                var reportItemPath = Request.QueryString["ItemPath"];
                var reportPath = reportsRootPathAddress + "/" + reportItemPath;

                MainReportViewer.ProcessingMode = ProcessingMode.Remote;
                MainReportViewer.ServerReport.ReportServerCredentials = new CustomReportCredentials("ReportViewer", "MnHoPAXw$");
                MainReportViewer.ServerReport.ReportServerUrl = new Uri(reportServerUrlAddress, UriKind.Absolute);
                MainReportViewer.ServerReport.ReportPath = reportPath;

                setReportParameters(MainReportViewer, Request.QueryString, reportItemPath);

                MainReportViewer.ServerReport.Timeout = 10000;
                MainReportViewer.ServerReport.Refresh();
            }

            MainReportViewer.Height = new Unit(string.IsNullOrEmpty(ClientSizeHeight.Value) ? "600px" : ClientSizeHeight.Value + "px");
        }

        private void setReportParameters(Microsoft.Reporting.WebForms.ReportViewer reportViewer, NameValueCollection queryStringNameValueCollection, string reportItemPath)
        {
            long fuelUserCompanyId = GetCurrentFuelUser();

            var reportParameters = reportViewer.ServerReport.GetParameters();

            if (reportParameters.Any(rp => rp.Name == "User"))
                reportViewer.ServerReport.SetParameters(new ReportParameter("User", User.Identity.Name));

            switch (reportItemPath)
            {
                case "AccountingCardex":
                    reportViewer.ServerReport.SetParameters(new ReportParameter("CompanyId", fuelUserCompanyId.ToString()));
                    break;

                case "InventoryCardex":
                    reportViewer.ServerReport.SetParameters(new ReportParameter("CompanyId", fuelUserCompanyId.ToString()));
                    break;

                case "VoucherReport":

                    reportViewer.ServerReport.SetParameters(new ReportParameter("CompanyId", fuelUserCompanyId.ToString()));

                    var localVoucherNo = queryStringNameValueCollection["LocalVoucherNo"];
                    reportViewer.ServerReport.SetParameters(new ReportParameter("LocalVoucherNo", localVoucherNo));

                    break;

                case "InventoryQuantityReport":

                    var inventoryQuantityReportType = queryStringNameValueCollection["Type"];

                    if (inventoryQuantityReportType == "Company")
                        reportViewer.ServerReport.SetParameters(new ReportParameter("DefaultCompanyId", fuelUserCompanyId.ToString()));
                    else
                        if (isHoldingReportAccessibleByCurrentUser())
                            reportViewer.ServerReport.SetParameters(new ReportParameter("DefaultCompanyId", (string)null));
                        else
                        {
                            reportViewer.Visible = false;
                            lblUnauthorizedAccess.Visible = true;
                        }

                    break;

                case "FuelOriginalDataReport":
                    
                    var reportCode = queryStringNameValueCollection["ReportCode"];
                    reportViewer.ServerReport.SetParameters(new ReportParameter("ReportCode", reportCode));

                    break;

                case "VesselReportDataReport":

                    break;

                case "PeriodicalFuelStatisticsReport":

                    var periodicalFuelStatisticsReportType = queryStringNameValueCollection["Type"];

                    if (periodicalFuelStatisticsReportType == "Company")
                        reportViewer.ServerReport.SetParameters(new ReportParameter("CompanyId", fuelUserCompanyId.ToString()));
                    else
                        if (isHoldingReportAccessibleByCurrentUser())
                            reportViewer.ServerReport.SetParameters(new ReportParameter("CompanyId", (string)null));
                        else
                        {
                            reportViewer.Visible = false;
                            lblUnauthorizedAccess.Visible = true;
                        }

                    break;
                case "VesselRunningValuesReport":
                    reportViewer.ServerReport.SetParameters(new ReportParameter("CompanyId", fuelUserCompanyId.ToString()));
                    break;
            }

        }

        public long GetCurrentFuelUser()
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["WebApiFuel"] + "/apiArea/Fuel/Users/0/CurrentCompanyId";

            var syncEvent = new AutoResetEvent(false);

            long fuelUserCompanyId = 0;


            WebClientHelper.Get<long>(new Uri(url, UriKind.Absolute),
                (res, exception) =>
                {

                    if (exception == null)
                        fuelUserCompanyId = res;

                    syncEvent.Set();
                }, MITD.Presentation.WebClientHelper.MessageFormat.Json, httpHeaders);

            syncEvent.WaitOne();

            return fuelUserCompanyId;
        }

        private bool isHoldingReportAccessibleByCurrentUser()
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["WebApiFuel"] + "/apiArea/Fuel/Users/0/CurrentUserAccessToHolding";

            var syncEvent = new AutoResetEvent(false);

            bool hasAccess = false;


            WebClientHelper.Get<bool>(new Uri(url, UriKind.Absolute),
                (res, exception) =>
                {

                    if (exception == null)
                        hasAccess = res;
                    else
                        hasAccess = false;

                    syncEvent.Set();
                }, MITD.Presentation.WebClientHelper.MessageFormat.Json, httpHeaders);

            syncEvent.WaitOne();

            return hasAccess;
        }

    }

    public class CustomReportCredentials : IReportServerCredentials
    {
        private string _UserName;
        private string _PassWord;
        private string _DomainName;


        public CustomReportCredentials(string UserName, string PassWord)
        {
            _UserName = UserName;
            _PassWord = PassWord;
        }

        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }


        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get { return null; }
        }

        public ICredentials NetworkCredentials
        {
            get { return string.IsNullOrWhiteSpace(_DomainName) ? new NetworkCredential(_UserName, _PassWord) : new NetworkCredential(_UserName, _PassWord, _DomainName); }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string user,
         out string password, out string authority)
        {
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }
}