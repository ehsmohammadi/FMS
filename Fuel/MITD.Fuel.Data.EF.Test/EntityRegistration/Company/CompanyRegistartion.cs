using System;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Transactions;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Migrations;
using MITD.Fuel.Data.EF.Repositories;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.FuelSecurity.Domain.Model;
using MITD.FuelSecurity.Domain.Model.Repository;

namespace MITD.Fuel.Data.EF.Test.EntityRegistration.Company
{
    [TestClass]
    public class CompanyRegistartion
    {
        private TransactionScope scope;

        private IUnitOfWorkScope unitOfWorkScope;

        [TestInitialize]
        public void InitTest()
        {
            scope = new TransactionScope();

            this.unitOfWorkScope = new UnitOfWorkScope(new EFUnitOfWorkFactory(() => new DataContainer()));
        }

        [TestCleanup]
        public void Cleanup()
        {
            scope.Dispose();
        }

        //================================================================================

        [TestMethod]
        public void HAFEZ_CompanyRegistration()
        {
            DataContainer context = new DataContainer();

            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {

                var lastCompanyId = Enumerable.Max(context.Inventory_Company.Select(c => c.Id));
                var hafezInventoryCompany = new Inventory_Company()
                                            {
                                                Id = ++lastCompanyId,
                                                Code = "0000213",
                                                CreateDate = DateTime.Now,
                                                IsActive = true,
                                                Name = "HAFEZ",
                                                UserCreatorId = 100000,
                                            };

                context.Inventory_Company.Add(hafezInventoryCompany);
                context.SaveChanges();


                var lastUserId = context.Parties.OfType<User>().Select(u => u.Id).Max();
                var lastInventoryUserId = (int) lastUserId;

                //var lastInventoryUserId = context.Inventory_User.Where(u => u.Id != 100000).Select(u => u.Id).Max();
                var inventoryUser_gmfmshafez = new Inventory_User()
                                               {

                                                   Id = ++lastInventoryUserId,
                                                   Code = lastInventoryUserId,
                                                   CreateDate = DateTime.Now,
                                                   EmailAddress = "",
                                                   IsActive = true,
                                                   Name = "gmfmshafez",
                                                   Password = "******",
                                                   SessionId = "",
                                                   UserCreatorId = 100000,
                                                   UserName = "gmfmshafez"
                                               };

                context.Inventory_User.Add(inventoryUser_gmfmshafez);
                context.SaveChanges();


                var gmfmshafez = new User(lastInventoryUserId, "gmfmshafez", "", "Capt. Safara", "safara@irisl.net", "gmfmshafez");

                gmfmshafez.UpdateCompany(lastCompanyId);
                context.Parties.Add(gmfmshafez);
                context.SaveChanges();


                //lastInventoryUserId = context.Inventory_User.Where(u => u.Id != 100000).Select(u => u.Id).Max();
                lastUserId = context.Parties.OfType<User>().Select(u => u.Id).Max();
                lastInventoryUserId = (int) lastUserId;

                var inventoryUser_fmshafez = new Inventory_User()
                                             {
                                                 Id = ++lastInventoryUserId,
                                                 Code = lastInventoryUserId,
                                                 CreateDate = DateTime.Now,
                                                 EmailAddress = "",
                                                 IsActive = true,
                                                 Name = "fmshafez",
                                                 Password = "******",
                                                 SessionId = "",
                                                 UserCreatorId = 100000,
                                                 UserName = "fmshafez",
                                             };

                context.Inventory_User.Add(inventoryUser_fmshafez);
                context.SaveChanges();

                var fmshafez = new User(lastInventoryUserId, "fmshafez", "", "fmshafez", "safara@irisl.net", "fmshafez");

                fmshafez.UpdateCompany(lastCompanyId);
                context.Parties.Add(fmshafez);
                context.SaveChanges();


                var fuel_gmfmshafez = context.FuelUsers.Single(fu => fu.IdentityId == gmfmshafez.Id);
                var fuel_fmshafez = context.FuelUsers.Single(fu => fu.IdentityId == fmshafez.Id);

                //Insert WorkflowSteps for new Company

                //foreach (var workflowStep in context.WorkflowSteps.ToList())
                //{
                //    //workflowStep.ActorUsers.Add(fuel_gmfmshafez);
                //    //workflowStep.ActorUsers.Add(fuel_fmshafez);
                //}

                context.SaveChanges();

                var hafizCompany = context.Companies.Single(c => c.Name == "HAFIZ");
                var hafizVessels = context.VesselsInCompanies.Where(vic => vic.Company.Name == "HAFIZ").ToList();

                var lastWarehouseId = context.Inventory_Warehouse.Select(w => w.Id).Max();

                foreach (var hafizVessel in hafizVessels)
                {
                    var hafezVessel = new VesselInCompany(hafizVessel.Name, hafizVessel.Name + " / HAFEZ", hafezInventoryCompany.Id, hafizVessel.VesselId, VesselStates.Inactive, true);

                    context.VesselsInCompanies.Add(hafezVessel);


                    var hafezWarehouse = new Inventory_Warehouse()
                                         {
                                             Code = hafizVessel.Vessel.Code,
                                             CompanyId = hafezInventoryCompany.Id,
                                             CreateDate = DateTime.Now,
                                             Id = ++lastWarehouseId,
                                             IsActive = false,
                                             Name = hafizVessel.Name + " / HAFEZ",
                                             UserCreatorId = 100000
                                         };

                    context.Inventory_Warehouse.Add(hafezWarehouse);
                }
                context.SaveChanges();

                transaction.Commit();
            }

            //Added to persist changes.
            scope.Complete();
        }

        //================================================================================



    }
}
