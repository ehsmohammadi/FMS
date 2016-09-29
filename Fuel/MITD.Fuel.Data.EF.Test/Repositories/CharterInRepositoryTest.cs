using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class CharterInRepositoryTest
    {
        EFUnitOfWork eFUnitOfWork;
        CharterInRepository charterInRepository;
        CharterRepository Repository;
        [TestInitialize]
        public void Init()
        {
            eFUnitOfWork = new EFUnitOfWork(new DataContainer(DbConnectionHelper.GetConnectionString()));
            charterInRepository = new CharterInRepository(eFUnitOfWork);
            //Repository = new CharterRepository(eFUnitOfWork);
        }

        private void ReInit()
        {
            eFUnitOfWork.Dispose();
            eFUnitOfWork = new EFUnitOfWork(new DataContainer(DbConnectionHelper.GetConnectionString()));
            charterInRepository = new CharterInRepository(eFUnitOfWork);
           // Repository = new CharterRepository(eFUnitOfWork);
        }

        [TestMethod]
        public void Can_be_add_CharterIn()
        {

            #region Arrange

            //var x = new CharterIn(0, 1, 1, 1, 1, new DateTime(2013, 01, 01), new List<CharterItem>(), new List<InventoryOperation>(),
            //                     CharterType.End, CharterEndType.LayUp,
            //                     null);
            //x.OffHirePricingType =OffHirePricingType.IssueBase;

           // var y = new CharterOut(0, 1, 1, 1, 1, new DateTime(2015, 01, 01), new List<CharterItem>(), new List<InventoryOperation>(),
                      //        CharterType.End, CharterEndType.LayUp,
                  //            null);
           
            
            
            //DataContainer dc = new DataContainer();

            //dc.Entry(x).State = EntityState.Added;
            //dc.SaveChanges();



            #endregion

            #region Action


            //charterInRepository.Add(x);
            //eFUnitOfWork.Commit();

            #endregion

            #region Assert

             // ReInit();
             // var r = charterInRepository.GetAll().OfType<CharterIn>();
             //Assert.IsNotNull(r);


            #endregion

        }
    }
}
