using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.FuelSecurity.Domain.Model;
using MITD.FuelSecurity.Domain.Model.Service;

namespace MITD.Fuel.Application.Service.Security
{
    public class LogService : ILogService
    {
        private ILogManagerService logManagerService;

        public LogService(ILogManagerService logManagerService)
        {
            this.logManagerService = logManagerService;
        }

        public Log AddEventLog(string code, LogLevel logLevel, User user, string className,
                          string methodName, string title, string messages)
        {
            var log = new EventLog(0, code, logLevel, user, className, methodName, title, messages);
            return logManagerService.AddLog(log);
        }

        public Log AddExceptionLog(string code, LogLevel logLevel, User user, string className,
                          string methodName, string title, string messages)
        {
            var log = new EventLog(0, code, logLevel, user, className, methodName, title, messages);
            return logManagerService.AddLog(log);
        }

        public void Delete(List<long> toList)
        {
            foreach (long logId in toList)
            {
                var log = logManagerService.GetLog(logId);
                logManagerService.DeleteLog(log);
            }
        }

        public IList<Log> GetAllLogs()
        {
            return logManagerService.GetAllLog();
        }

        public Log GetLogById(long logId)
        {
            return logManagerService.GetLog(logId);
        }

        public void AddExceptionLog(Exception ex)
        {
        }
    }
}
