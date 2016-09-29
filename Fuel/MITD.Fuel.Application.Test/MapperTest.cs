#region

using System;
using System.Linq;
using System.Net.Http;
using System.Transactions;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.StorageSpace.Adapter;
using MITD.Fuel.ACL.StorageSpace.DomainServices;
using MITD.Fuel.ACL.StorageSpace.Mapper;
using MITD.Fuel.ACL.StorageSpace.ServiceWrappers;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Facade.Mappers;
using MITD.Fuel.Application.Service;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Repositories;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainServices;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Factories;
using MITD.Fuel.Domain.Model.FakeDomainServices;
using MITD.Fuel.Infrastructure.Service;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.AntiCorruption;
using MITD.Services.Connectivity;
using MITD.Services.Facade;
using MITD.StorageSpace.Presentation.Contracts.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion


namespace MITD.Fuel.Application.Test
{
    [TestClass]
    public class MapperTest
    {
        private OrderItemToDtoMapper orderItemToDtoMapper;
        private GoodToGoodDtoMapper goodToGoodDtoMapper;
        private ICompanyGoodUnitToGoodUnitDtoMapper goodUnitMapper;
        #region Initial

        [TestInitialize]
        public void TestInitialize()
        {

            orderItemToDtoMapper = new OrderItemToDtoMapper(new GoodToGoodDtoMapper(new CompanyGoodUnitToGoodUnitDtoMapper()));
            goodUnitMapper = new CompanyGoodUnitToGoodUnitDtoMapper();
            goodToGoodDtoMapper = new GoodToGoodDtoMapper(goodUnitMapper);

        }


        [TestMethod]
        public void InitiateGoodToGoodDtoMapper()
        {
            Assert.IsNotNull(goodToGoodDtoMapper);
        }



        #endregion



        #region Approve Senarion


        #endregion

    }
}
