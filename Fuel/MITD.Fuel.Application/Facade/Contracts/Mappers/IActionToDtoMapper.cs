using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.FuelSecurity.Domain.Model;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
  public  interface IActionToDtoMapper : IFacadeMapper<ActionType, ActionTypeDto>
    {
      ActionTypeDto MapToDtoModel(ActionType account);
      List<ActionTypeDto> MapToDtoModel(List<ActionType> accounts);
    }
}
