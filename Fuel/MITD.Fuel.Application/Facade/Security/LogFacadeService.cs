using System.Collections.Generic;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Security;
using MITD.Fuel.Presentation.Contracts.DTOs.Security;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.FuelSecurity.Domain.Model;
using MITD.FuelSecurity.Domain.Model.Repository;
using MITD.FuelSecurity.Domain.Model.Service;

namespace MITD.Fuel.Application.Facade.Security
{
    public class LogFacadeService : ILogFacadeService
    {
        private readonly ILogService _logService;

        private ILogToDtoMapper logDtoMapper;
        private readonly IUserRepository _userRep;

        #region ctor

        public LogFacadeService(ILogService logService, IUserRepository userRep, ILogToDtoMapper mapper)
        {
            this._logService = logService;
            this.logDtoMapper = mapper;
            this._userRep = userRep;
            
        }

        #endregion

        #region method
        public LogDto GetLog(long logId)
        {
            Log log = _logService.GetLogById(logId);
            var LogDto = logDtoMapper.MapToModel(log);
            return LogDto;
        }

        public LogDto AddEventLog(EventLogDto LogDto)
        {
            User user = null;
            //this check must be move to domain servi
            if (!string.IsNullOrWhiteSpace(LogDto.UserName))
                user = _userRep.GetUserById(LogDto.UserName)as User;
            var log = _logService.AddEventLog(LogDto.Code, LogLevel.FromName(LogDto.LogLevel),
                user, LogDto.ClassName, LogDto.MethodName, LogDto.Title, LogDto.Messages);
            return logDtoMapper.MapToModel(log);
        }

        public LogDto AddExceptionLog(ExceptionLogDto LogDto)
        {
            User user = null;
            //this check must be move to domain servi
            if (!string.IsNullOrWhiteSpace(LogDto.UserName))
                user = _userRep.GetUserById(LogDto.UserName)as User;
            var log = _logService.AddExceptionLog(LogDto.Code, LogLevel.FromName(LogDto.LogLevel),
                user, LogDto.ClassName, LogDto.MethodName, LogDto.Title, LogDto.Messages);
            return logDtoMapper.MapToModel(log);
        }

        public string DeleteLog(List<long> logIdList)
        {
            ///_logService.Delete(logIdList.Select(id => new LogId(id)).ToList());
            return "logs deleted successfully ";
        }

        public LogDto AddEventLog(string title, string code, string logLevel, string className, string methodName, string messages,
                                string userName)
        {
            User user = null;
            //this check must be move to domain servi
            if (!string.IsNullOrWhiteSpace(userName))
                user = _userRep.GetUserById(userName)as User;
            var log = _logService.AddEventLog(code, LogLevel.FromName(logLevel),
                user, className, methodName, title, messages);
            return logDtoMapper.MapToModel(log);
        }

        #endregion

    }
}
