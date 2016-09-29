using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.DataAccess.EF;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Repositories;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MITD.Fuel.Data.EF.Test.Repositories
{
  

    [TestClass]
    public class CharterOutRepositoryTest
    {
        EFUnitOfWork eFUnitOfWork;
        CharterOutRepository charterRepository;

        [TestInitialize]
        public void Init()
        {
            eFUnitOfWork = new EFUnitOfWork(new DataContainer(DbConnectionHelper.GetConnectionString()));
            charterRepository = new CharterOutRepository(eFUnitOfWork);
        }

        private void ReInit()
        {
            eFUnitOfWork.Dispose();
            eFUnitOfWork = new EFUnitOfWork(new DataContainer(DbConnectionHelper.GetConnectionString()));
            charterRepository = new CharterOutRepository(eFUnitOfWork);
        }

        [TestMethod]
        public void Can_be_add_CharterOut()
        {

            #region Arrange

            //var x = new CharterOut(0, 1, 1, 1, 1, new DateTime(2013, 01, 01), new List<CharterItem>(), new List<InventoryOperation>(),
            //                      CharterType.End, CharterEndType.LayUp,
            //                      null);


            #endregion

            #region Action


            //charterRepository.Add(x);
            //eFUnitOfWork.Commit();

            #endregion

            #region Assert
            ReInit();
            var r = charterRepository.GetAll();
            Assert.IsNotNull(r);


            #endregion

        }
    }
}
