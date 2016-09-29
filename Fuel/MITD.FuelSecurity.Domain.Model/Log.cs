using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.FuelSecurity.Domain.Model
{
    public class Log
    {
        #region Properties


        private  long id;
        public virtual long Id { get { return id; }
            private set { id = value; }
        }

        private  string code;
        public virtual string Code { get { return code; } private set { code = value; } }

        private  LogLevel logLevel;
        public virtual LogLevel LogLevel { get { return logLevel; } private set { logLevel = value; } }

        private  int logLevelId;
        public int LogLevelId { get { return logLevelId; } private set { logLevelId = value; } }


        private  long partyId;
        public virtual long PartyId { get { return partyId; } private set { partyId = value; } }

        private  string className;

        public virtual string ClassName                     
        {
            get { return className; }
            private set { className = value; }

        }

        private  string methodName;
        public virtual string MethodName { get { return methodName; } private set { methodName = value; } }

        private  DateTime logDate;
        public virtual DateTime LogDate { get { return logDate; } private set { logDate = value; } }

        private  string title;
        public virtual string Title { get { return title; } private set { title = value; } }

        private  string messages;
        public virtual string Messages { get { return messages; } private set { messages = value; } }


        #endregion

        #region Ctor

        public Log()
        {
            //For OR mapper
        }
        public Log(long id, string code, LogLevel logLevel, User user, string className,
           string methodName, string title, string messages)
        {

            this.id = id;
            this.code = code;
            this.logLevel = logLevel;
            if (user != null)
                this.partyId = user.Id;
            this.className = className;
            this.methodName = methodName;
            this.logDate = DateTime.Now;
            this.logLevelId =(int)logLevel;

            if (!string.IsNullOrEmpty(title) && title.Length > 200)
                this.title = title.Substring(0, 199);
            else
                this.title = title;

            if (!string.IsNullOrEmpty(messages) && messages.Length > 4000)
                this.messages = messages.Substring(0, 3999);
            else
                this.messages = messages;
        }
        #endregion

       
    }
}