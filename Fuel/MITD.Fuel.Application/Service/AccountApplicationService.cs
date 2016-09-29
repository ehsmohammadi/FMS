using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Application.Service
{
    public class AccountApplicationService : IAccountApplicationService
    {
        #region Prop
        private IAccountRepository _accountRepository;

        private IUnitOfWorkScope _unitOfWorkScope;
        #endregion

        public AccountApplicationService(IAccountRepository accountRepository,IUnitOfWorkScope unitOfWorkScope)
        {
            _accountRepository = accountRepository;
            _unitOfWorkScope = unitOfWorkScope;
        }

        public void Add(Account account)
        {
            _accountRepository.Add(account);

            var res= _accountRepository.GetAll().Where(c => c.Code == account.Code).FirstOrDefault();
            if (res == null)
            {
                _unitOfWorkScope.Commit();  
            }
            else
            {
                throw new BusinessRuleException("010","Duplicate Account");
            }

           


        }
    }
}
