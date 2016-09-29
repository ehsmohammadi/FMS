using System;
using System.Transactions;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.Factories
{
    public class TransactionScopeFactory : ITransactionScopeFactory
    {
        public TransactionScope Create()
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            transactionOptions.Timeout = TransactionManager.MaximumTimeout;

            return new TransactionScope(TransactionScopeOption.Required, transactionOptions);
        }
    }
}
