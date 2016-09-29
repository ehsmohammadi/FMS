using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.DataAccess.EF;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MITD.Fuel.Data.EF.Test.Repositories
{
    [TestClass]
    public class ChaterRepositoryTest
    {
        EFUnitOfWork eFUnitOfWork;
        CharterRepository charterRepository;

        [TestInitialize]
        public void Init() 
        {
             eFUnitOfWork = new EFUnitOfWork(new DataContainer());
             charterRepository = new CharterRepository(eFUnitOfWork);
        }

       

        [TestMethod]
        public void Can_get_Charter()
        {
            #region Arrange

            #endregion

            #region Action

          

            #endregion

            #region Assert

            var x = charterRepository.GetAll();
            Assert.IsNotNull(x);

  
            #endregion

        }

      
        

    }
}
