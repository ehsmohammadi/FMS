using System;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs.Security
{
    public partial class LogDto
    {
        public string DTOTypeName
        {
            get { return GetType().Name; }
        }

        private long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        private string code;
        public string Code
        {
            get { return code; }
            set { this.SetField(p => p.Code, ref code, value); }
        }

        private string logLevel;
        public string LogLevel
        {
            get { return logLevel; }
            set { this.SetField(p => p.LogLevel, ref logLevel, value); }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { this.SetField(p => p.UserName, ref userName, value); }
        }


        private string className;
        public string ClassName
        {
            get { return className; }
            set { this.SetField(p => p.ClassName, ref className, value); }
        }

        private string methodName;
        public string MethodName
        {
            get { return methodName; }
            set { this.SetField(p => p.MethodName, ref methodName, value); }
        }

        private DateTime logDate;
        public DateTime LogDate
        {
            get { return logDate; }
            set { this.SetField(p => p.LogDate, ref logDate, value); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { this.SetField(p => p.Title, ref title, value); }
        }

        private string messages;
        public string Messages
        {
            get { return messages; }
            set { this.SetField(p => p.Messages, ref messages, value); }
        }


    }
}
