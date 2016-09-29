using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Application.Service
{
    public class ReceiveTypeApplicationService : IReceiveTypeApplicationService
    {
        #region Prop

        public IRepository<ReceiveType> Repository { get; set; }
        public IUnitOfWorkScope UnitOfWorkScope { get; set; }

        #endregion



        #region Ctor

        public ReceiveTypeApplicationService(IRepository<ReceiveType> repository,IUnitOfWorkScope unitOfWorkScope)
        {
            this.Repository = repository;
            this.UnitOfWorkScope = unitOfWorkScope;
        }

        #endregion

        #region Method


        public List<ReceiveType> GetAll()
        {
            var lstFetchStartegy = new ListFetchStrategy<ReceiveType>(Enums.FetchInUnitOfWorkOption.NoTracking);

            return Repository.GetAll(lstFetchStartegy).ToList();
        }

        public ReceiveType GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ReceiveType> GetById(List<int> idList)
        {
            throw new NotImplementedException();
        }

        public ReceiveType Add(ReceiveType entity)
        {
            throw new NotImplementedException();
        }

        public ReceiveType Update(ReceiveType entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(ReceiveType entity)
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