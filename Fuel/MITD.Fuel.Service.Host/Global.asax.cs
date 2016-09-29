using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
//sing HibernatingRhinos.Profiler.Appender.EntityFramework;
using MITD.Fuel.Service.Host.App_Start;
using MITD.Fuel.Service.Host.Infrastructure;

namespace MITD.Fuel.Services.Host
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           // EFlogger.EntityFramework6.EFloggerFor6.Initialize();
            (new HostBootstrapper()).Execute();
        
  //          HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize(EntityFrameworkVersion.EntityFramework6);
       }
    }
}