using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Presentation.Contracts.DTOs.Security;
using MITD.FuelSecurity.Domain.Model;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class LogToDtoMapper : BaseMapper<Log, LogDto>, ILogToDtoMapper
   {
       public override Log MapToEntity(LogDto model)
       {
           throw new NotImplementedException();
       }

       public override LogDto MapToModel(Log entity)
       {
          return new LogDto()
                 {
                     Id=entity.Id,
                     ClassName = entity.ClassName,
                     Code=entity.Code,
                     //UserName=
                     Title = entity.Title,
                     LogDate = entity.LogDate,
                     Messages = entity.Messages,
                     MethodName = entity.MethodName,
                     LogLevel = entity.LogLevel.ToString(),
                     

                 };
       }
   }
}
