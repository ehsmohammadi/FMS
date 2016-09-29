using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;

namespace MITD.FuelSecurity.Domain.Model.Service
{
   public interface ILoggerService 
   {
       void AddLog(Log log);
       IList<Log> GetAll();
       Log GetLogById(long logId);
       void DeleteLog(Log log);
    }
}
