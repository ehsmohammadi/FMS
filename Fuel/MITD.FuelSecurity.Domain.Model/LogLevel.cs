using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.FuelSecurity.Domain.Model
{
    public class LogLevel
    {
        #region Prop

        public static readonly LogLevel None = new LogLevel("None", "0");
        public static readonly LogLevel Information = new LogLevel("Information", "1");
        public static readonly LogLevel Warning = new LogLevel("Warning", "2");
        public static readonly LogLevel Error = new LogLevel("Error", "3");
        public static readonly LogLevel AccessControl = new LogLevel("AccessControl", "4");

        public string Value { get; private set; }
        public string Name { get; private set; }

        #endregion

        public LogLevel()
        {

        }
        public LogLevel(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }


        public static LogLevel FromName(string name)
        {
            string nam = name.ToLower();
            var res =new LogLevel();
            switch (nam)
            {
                case "none":
                {
                    res = None;

                }
                    break;
                case "information":
                {
                    res = Information;

                }
                    break;
                case "warning":
                {
                    res = Warning;

                }
                    break;
                case "error":
                {
                    res = Error;

                }
                    break;
                case "accesscontrol":
                {
                    res = AccessControl;

                }
                    break;

            }
            return res;
        }

        public static implicit operator LogLevel(int i)
        {
            return GetLogLevel(i);
        }

        public static explicit operator int(LogLevel logLevel)
        {
            return Int32.Parse(logLevel.Value);
        }

        public static LogLevel GetLogLevel(int id)
        {
            var res = None;
            switch (id)
            {
                case 1:
                    res = Information;
                    break;
                case 2:
                    res = Warning;
                    break;
                case 3:
                    res = Error;
                    break;
                case 4:
                    res = AccessControl;
                    break;

            } return res;
        }
    }
}
