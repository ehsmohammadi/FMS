using System;
using System.Transactions;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Application.Service
{
    public class CurrencyApplicationService : ICurrencyApplicationService
    {
        private readonly IUnitOfWorkScope unitOfWorkScope;
        private readonly ICurrencyDomainService currencyDomainService;

        public CurrencyApplicationService(IUnitOfWorkScope unitOfWorkScope, ICurrencyDomainService currencyDomainService)
        {
            this.unitOfWorkScope = unitOfWorkScope;
            this.currencyDomainService = currencyDomainService;
        }


        //================================================================================

        //public void UpdateCurrenciesFromFinance()
        //{
        //    using (var transactionScope = new TransactionScope())
        //    {
        //        currencyDomainService.UpdateCurrenciesFromFinance();

        //        transactionScope.Complete();
        //    }
        //}

        public void UpdateCurrencies()
        {
            currencyDomainService.UpdateCurrenciesFromFinance();

            //using (var transactionScope = new TransactionScope())
            //{
                currencyDomainService.UpdateCurrencyRatesFromFinance();

                unitOfWorkScope.Commit();

            //    transactionScope.Complete();
            //}
        }
    }
}