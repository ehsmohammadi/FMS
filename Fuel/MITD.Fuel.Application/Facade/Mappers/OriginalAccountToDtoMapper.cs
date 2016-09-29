using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class OriginalAccountToDtoMapper : IOriginalAccountToDtoMapper
    {
        public AccountDto MapToDtoModel(OriginalAccount account)
        {
            return new AccountDto()
                   {
                     
                       Code=account.Code,
                       Name = account.Name
                   };
        }

        public List<AccountDto> MapToDtoModel(List<OriginalAccount> accounts)
        {
            var res = new List<AccountDto>();

            accounts.ForEach(c => res.Add(MapToDtoModel(c)));

            return res;
        }

        public IEnumerable<OriginalAccount> MapToEntity(IEnumerable<AccountDto> models)
        {
            throw new NotImplementedException();
        }

        public OriginalAccount MapToEntity(AccountDto model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AccountDto> MapToModel(IEnumerable<OriginalAccount> entities)
        {
            throw new NotImplementedException();
        }

        public AccountDto MapToModel(OriginalAccount entity)
        {
            throw new NotImplementedException();
        }

        public AccountDto RemapModel(AccountDto model)
        {
            throw new NotImplementedException();
        }
    }
}
