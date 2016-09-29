using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.FuelSecurity.Domain.Model;
using MITD.Services.Application;

namespace MITD.Fuel.Application.Service.Security
{
    public interface ILogService : IApplicationService
    {
        Log AddEventLog(string code, LogLevel logLevel, User user, string className,
            string methodName, string title, string messages);
        Log AddExceptionLog(string code, LogLevel logLevel, User user, string className,
            string methodName, string title, string messages);
        void Delete(List<long> toList);
        IList<Log> GetAllLogs();
        Log GetLogById(long logId);

        void AddExceptionLog(Exception ex);
    }
}
