using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.DTOs.Security;
using MITD.FuelSecurity.Domain.Model;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface ILogToDtoMapper : IFacadeMapper<Log, LogDto>
    {
    }
}
