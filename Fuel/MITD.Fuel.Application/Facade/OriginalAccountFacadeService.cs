using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class OriginalAccountFacadeService : IOriginalAccountFacadeService
    {

        private IOriginalAccountRepository _accountRepository;
        private IOriginalAccountToDtoMapper _accountToDtoMapper;
        public OriginalAccountFacadeService(IOriginalAccountRepository accountRepository, IOriginalAccountToDtoMapper accountToDtoMapper)
        {
            _accountRepository = accountRepository;
            _accountToDtoMapper = accountToDtoMapper;
        }

        public PageResultDto<AccountDto> GetAllByFilter(string name, string code, int pageIndex, int pageSize)
        {

            IQueryable<OriginalAccount> query = _accountRepository.GetQuery()
                .Where(c =>
                    (c.Name.Contains(name) || string.IsNullOrEmpty(name)) &&
                    (c.Code.Contains(code) || string.IsNullOrEmpty(code)));
            var totalCount = query.Count();

            pageIndex = Math.Min(totalCount / pageSize, pageIndex);

            var account = query.OrderBy(c => c.Code).Skip(pageSize * pageIndex)
            //var account = query.OrderBy(c => c.Code).Skip(pageSize * ((pageIndex==0)?0:(pageIndex - 1)))                                
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
    }
}
