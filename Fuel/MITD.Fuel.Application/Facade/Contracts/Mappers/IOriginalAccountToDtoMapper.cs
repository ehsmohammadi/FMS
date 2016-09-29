using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IOriginalAccountToDtoMapper : IFacadeMapper<OriginalAccount, AccountDto>
    {
        AccountDto MapToDtoModel(OriginalAccount account);
        List<AccountDto> MapToDtoModel(List<OriginalAccount> accounts);
    }
}
