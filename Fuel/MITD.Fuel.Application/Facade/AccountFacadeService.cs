using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class AccountFacadeService : IAccountFacadeService
    {

        private IAccountRepository _accountRepository;
        private IAccountToDtoMapper _accountToDtoMapper;
        private IAccountApplicationService _accountApplicationService;
        public AccountFacadeService(IAccountRepository accountRepository, IAccountToDtoMapper accountToDtoMapper, IAccountApplicationService accountApplicationService)
        {
            _accountRepository = accountRepository;
            _accountToDtoMapper = accountToDtoMapper;
            _accountApplicationService = accountApplicationService;
        }

        public PageResultDto<AccountDto> GetAllByFilter(string name, string code, int pageIndex, int pageSize)
        {
            IQueryable<Account> query = _accountRepository.GetQuery()
                .Where(c =>
                    (c.Name.Contains(name) || string.IsNullOrEmpty(name)) &&
                    (c.Code.Contains(code) || string.IsNullOrEmpty(code)));
            var totalCount = query.Count();

            pageIndex = Math.Min(totalCount / pageSize, pageIndex);

            var account = query.OrderByDescending(c => c.Id).Skip(pageSize * pageIndex)
                //var account =query.OrderByDescending(c=>c.Id).Skip(pageSize *  ((pageIndex==0)?0:(pageIndex - 1)))
                   .Take(pageSize).ToList();

            return new PageResultDto<AccountDto>()
            {
                Result = _accountToDtoMapper.MapToDtoModel(account),
                TotalCount = totalCount,
                CurrentPage = pageIndex,
                PageSize = pageSize,
                TotalPages = Convert.ToInt32(Math.Ceiling(decimal.Divide(totalCount, pageSize)))
            };


        }


        public void Add(AccountDto data)
        {
            var ent = new Account(0, data.Code, data.Name);
            _accountApplicationService.Add(ent);
        }
    }
}
