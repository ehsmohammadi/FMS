using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.FuelSecurity.Domain.Model.Service
{
    public class LogManagerService : ILogManagerService
    {
        private static string loggerServiceKeys = ConfigurationManager.AppSettings["LogServicesPriority"];
        private static string logLevelTypeNames = ConfigurationManager.AppSettings["LogLevelTypes"];

        private readonly string primaryLoggerKey = "";
        private readonly string secondaryLoggerKey = "";
        private readonly string thirdLoggerKey = "";
        private readonly List<LogLevel> logLevels = new List<LogLevel>();

        private readonly ILoggerServiceFactory _loggerFactory;


        public LogManagerService()//(ILoggerServiceFactory loggerFactory)
        {
            this._loggerFactory =new LoggerServiceFactory(); //loggerFactory;
            var loggerKeys = (loggerServiceKeys != null) ? loggerServiceKeys.Split(',') : new string[] { };
            if (loggerKeys.Any())
                primaryLoggerKey = loggerKeys[0];

            if (loggerKeys.Length > 1)
                secondaryLoggerKey = loggerKeys[1];
            if (!string.IsNullOrWhiteSpace(logLevelTypeNames))
            {
                var levelList = logLevelTypeNames.Split(',');
               levelList.ToList().ForEach(c =>
                                                       { if (logLevels != null) logLevels.Add(LogLevel.FromName(c)); }) ;
            }
        }

        public Log AddLog(Log log)
        {
            var logger = _loggerFactory.Create(primaryLoggerKey);
            try
            {
                if (logLevels.Contains(log.LogLevel))
                    logger.AddLog(log);
            }
            catch
            {
                log = AddLogWithSecondaryLogger(log);
            }
            finally
            {
                _loggerFactory.Release(logger);
            }
            return log;
        }

        public void DeleteLog(Log log)
        {
            throw new NotImplementedException();
        }

        public Log GetLog(long logId)
        {
            throw new NotImplementedException();
        }

        public List<Log> GetAllLog()
        {
            throw new NotImplementedException();
        }

        private Log AddLogWithSecondaryLogger(Log log)
        {
            if (String.IsNullOrWhiteSpace(secondaryLoggerKey))
                return log;

            var logger = _loggerFactory.Create(secondaryLoggerKey);
            try
            {
                if (logLevels.Contains(log.LogLevel))
                    logger.AddLog(log);
            }
            catch
            { log = AddLogWithThridLogger(log); }
            finally
            {
                _loggerFactory.Release(logger);
            }
            return log;
        }

        private Log AddLogWithThridLogger(Log log)
        {
            if (String.IsNullOrWhiteSpace(thirdLoggerKey))
                return log;

            var logger = _loggerFactory.Create(thirdLoggerKey);
            try
            {
                if (logLevels.Contains(log.LogLevel))
                    logger.AddLog(log);
            }
            catch { }
            finally
            {
                _loggerFactory.Release(logger);
            }
            return log;
        }
    }
}
