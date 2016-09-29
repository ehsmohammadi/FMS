using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.FuelSecurity.Domain.Model
{
   public class EventLog:Log
    {

       protected EventLog()
       {
           
       }

       public EventLog(long id, string code, LogLevel logLevel, User party, string className,
           string methodName, string title, string messages)
           : base(id, code, logLevel, party, className, methodName, title, messages)
       {
       }
    }
}
