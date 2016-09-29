using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;

namespace MITD.FuelSecurity.Domain.Model.Service
{
  public  interface ILogManagerService :IDomainService
    {
        Log AddLog(Log log);
        void DeleteLog(Log log);
        Log GetLog(long logId);
        List<Log> GetAllLog();
    }
}
