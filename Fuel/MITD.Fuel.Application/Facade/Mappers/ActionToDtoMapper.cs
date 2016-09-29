using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.FuelSecurity.Domain.Model;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class ActionToDtoMapper : IActionToDtoMapper

    {

        public ActionTypeDto MapToDtoModel(ActionType account)
        {
            return new ActionTypeDto()
            {
                ActionName=account.Name,
                Id=account.Id,
                Description=account.Description
            };
        }

        public List<ActionTypeDto> MapToDtoModel(List<ActionType> accounts)
        {
            var res =new List<ActionTypeDto>();
            accounts.ForEach(c => res.Add(MapToDtoModel(c)));
            return res;
        }

        public IEnumerable<ActionType> MapToEntity(IEnumerable<ActionTypeDto> models)
        {
            throw new NotImplementedException();
        }

        public ActionType MapToEntity(ActionTypeDto model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ActionTypeDto> MapToModel(IEnumerable<ActionType> entities)
        {
            throw new NotImplementedException();
        }

        public ActionTypeDto MapToModel(ActionType entity)
        {
            throw new NotImplementedException();
        }

        public ActionTypeDto RemapModel(ActionTypeDto model)
        {
            throw new NotImplementedException();
        }
    }
}
