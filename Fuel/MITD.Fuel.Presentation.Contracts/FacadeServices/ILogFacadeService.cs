using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.DTOs.Security;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
  public  interface ILogFacadeService:IFacadeService
    {
        LogDto GetLog(long logId);
        LogDto AddEventLog(EventLogDto logDto);
        LogDto AddExceptionLog(ExceptionLogDto log);

        LogDto AddEventLog(string title, string code, string logLevel, string className, string methodName, string messages, string userName);
    }
}
