using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Application.Service
{
    public class TransferTypeApplicationService : ITransferTypeApplicationService
    {

        #region Prop

        public IRepository<TransferType> Repository { get; set; }
        public IUnitOfWorkScope UnitOfWorkScope { get; set; }

        #endregion


        #region Ctor

        public TransferTypeApplicationService(IRepository<TransferType> repository ,IUnitOfWorkScope unitOfWorkScope)
        {
            this.Repository = repository;
            this.UnitOfWorkScope = unitOfWorkScope;
        }

        #endregion


        #region Method

        public List<TransferType> GetAll()
        {
            var lstFetchStrategy = new ListFetchStrategy<TransferType>(Enums.FetchInUnitOfWorkOption.NoTracking);
            return Repository.GetAll(lstFetchStrategy).ToList();
        }

        public TransferType GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<TransferType> GetById(List<int> idList)
        {
            throw new NotImplementedException();
        }

        public TransferType Add(TransferType entity)
        {
            throw new NotImplementedException();
        }

        public TransferType Update(TransferType entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TransferType entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
       
    }
}