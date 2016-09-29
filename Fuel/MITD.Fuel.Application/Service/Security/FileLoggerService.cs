using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.FuelSecurity.Domain.Model;
using MITD.FuelSecurity.Domain.Model.Service;

namespace MITD.Fuel.Application.Service.Security
{
    public class FileLoggerService : ILoggerService
    {
        public FileLoggerService()
        {
                
        }
        public void AddLog(Log log)
        {
            string filename = getFileLogPath();
            string logText = logToString(log);
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            lock (this)
            {
                try
                {
                    var logFileInfo = new FileInfo(filename);
                    fileStream = !logFileInfo.Exists ? logFileInfo.Create() : new FileStream(filename, FileMode.Append);
                    streamWriter = new StreamWriter(fileStream);
                    streamWriter.WriteLine(logText);
                }
                finally
                {
                    if (streamWriter != null) streamWriter.Close();
                    if (fileStream != null) fileStream.Close();
                }
            }
        }

        private string getFileLogPath()
        {
            string logPath = ConfigurationManager.AppSettings["LogFilesPath"];
            string fileCreationType = ConfigurationManager.AppSettings["LogFileCreationType"];
            string filePath = AppDomain.CurrentDomain.BaseDirectory + logPath + "\\Log" +
                "-" + DateTime.Today.ToString(fileCreationType) + ".log";

            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo = new FileInfo(filePath);

            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists)
                logDirInfo.Create();

            return filePath;
        }

        private string logToString(Log log)
        {
            if (log == null)
                return "";
            string lineFeedChar = "\r\n";
            string msg = "";
            msg += (log is EventLog) ? "<LogType>:Event" : "<LogType>:Exception\n";
            msg += lineFeedChar;
            msg += log.LogDate.ToString() + lineFeedChar;
            msg += log.LogLevel + lineFeedChar;
            msg += "Id:" + log.Id.ToString() + lineFeedChar;
            if (!string.IsNullOrEmpty(log.Code)) msg += "code:" + log.Code + lineFeedChar;
            if (!string.IsNullOrWhiteSpace(log.ClassName)) msg += "className:" + log.ClassName + lineFeedChar;
            if (!string.IsNullOrWhiteSpace(log.MethodName)) msg += "methodName:" + log.MethodName + lineFeedChar;
            if (!string.IsNullOrEmpty(log.Title)) msg += log.Title + lineFeedChar;
            if (log.PartyId != null) msg += "user:" + log.PartyId.ToString() + lineFeedChar;
            if (!string.IsNullOrEmpty(log.Messages)) msg += "message:" + lineFeedChar + log.Messages + lineFeedChar;
            msg += lineFeedChar + "=========================================" + lineFeedChar;
            return msg;
        }

        public IList<Log> GetAll()
        {
            throw new NotImplementedException();
        }

        public Log GetLogById(long logId)
        {
            throw new NotImplementedException();
        }

        public void DeleteLog(Log log)
        {
            throw new NotImplementedException();
        }

    }
}
