using System;
using System.Net.Http;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.StorageSpace.Adapter;
using MITD.Fuel.ACL.StorageSpace.DomainServices;
using MITD.Fuel.ACL.StorageSpace.Mapper;
using MITD.Fuel.ACL.StorageSpace.ServiceWrappers;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Repositories;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainServices;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Infrastructure.Service;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.AntiCorruption;
using MITD.Services.Connectivity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MITD.Fuel.Application.Test
{
    [TestClass]
    public class GoodUnitConvertorDomainServiceTest1
    {
        private UnitOfWorkScope _scope;

        private BasicInfoDomainServiceObjectsContainer basicInfoDomainServiceObjects;

        private IGoodUnitConvertorDomainService _target;
        [TestInitialize]
        public void TestInitialize()
        {
            _scope = new UnitOfWorkScope(new EFUnitOfWorkFactory(() => new DataContainer()));

            basicInfoDomainServiceObjects = new BasicInfoDomainServiceObjectsContainer(_scope);

            _target = new GoodUnitConvertorDomainService(
                new GoodDomainService(
                    new GoodAntiCorruptionAdapter(
                        new GoodAntiCorruptionServiceWrapper(new WebClientHelper(new HttpClient()),
                                                             new ExternalHostAddressHelper()),
                        new GoodAntiCorruptionMapper()), new EFRepository<Good>(_scope),
                        basicInfoDomainServiceObjects.CompanyDomainService, new EFRepository<GoodUnit>(_scope)),
                        new GoodUnitDomainService(new BaseAntiCorruptionAdapter<GoodUnit, GoodUnitDto>(new BaseAntiCorruptionServiceWrapper<GoodUnitDto>
                            (new WebClientHelper(new HttpClient())), new BaseAntiCorruptionMapper<GoodUnit, GoodUnitDto>()),
                            new EFRepository<GoodUnit>(_scope)

                            ));



        }

        [TestMethod]
        public void GetUnitValueInMainUnit()
        {
            var result = _target.GetUnitValueInMainUnit(1, 1, 120);
            Assert.IsTrue(result.Name == "Unit1");
            Assert.IsTrue(result.Id == 1);
            Assert.IsTrue(result.Value == 24000);

        }
    }
}
