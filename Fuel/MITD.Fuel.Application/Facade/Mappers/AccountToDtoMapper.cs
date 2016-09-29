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
    public class AccountToDtoMapper : IAccountToDtoMapper
    {
        public AccountDto MapToDtoModel(Account account)
        {
            return new AccountDto()
                   {
                       Id=account.Id,
                       Code=account.Code,
                       Name = account.Name
                   };
        }

        public List<AccountDto> MapToDtoModel(List<Account> accounts)
        {
            var res = new List<AccountDto>();

            accounts.ForEach(c => res.Add(MapToDtoModel(c)));

            return res;
        }

        public IEnumerable<Account> MapToEntity(IEnumerable<AccountDto> models)
        {
            throw new NotImplementedException();
        }

        public Account MapToEntity(AccountDto model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AccountDto> MapToModel(IEnumerable<Account> entities)
        {
            throw new NotImplementedException();
        }

        public AccountDto MapToModel(Account entity)
        {
            throw new NotImplementedException();
        }

        public AccountDto RemapModel(AccountDto model)
        {
            throw new NotImplementedException();
        }
    }
}
