using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IAccountToDtoMapper :IFacadeMapper<Account,AccountDto>
    {
        AccountDto MapToDtoModel(Account account);
        List<AccountDto> MapToDtoModel(List<Account> accounts);
    }
}
