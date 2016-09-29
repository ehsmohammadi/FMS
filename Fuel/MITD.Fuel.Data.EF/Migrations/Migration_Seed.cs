using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Transactions;
using System.Windows.Forms;
using FluentMigrator;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.FakeDomainServices;
using MITD.FuelSecurity.Domain.Model;
using WorkflowDataConvertor;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Profile("debug")]
    public class Migration_Seed : Migration
    {
        public override void Up()
        {
            if (MessageBox.Show("Fill database with required data?", "Seed Database", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            //zakerfathi , amoozadeh ,  seyyedi
            //DbContextTransaction transaction = context.Database.BeginTransaction();

            try
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;
                transactionOptions.Timeout = TransactionManager.MaximumTimeout;

                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    using (DataContainer context = new DataContainer())
                    {

                        MessageBox.Show("Continue Seeding the Database without debugging? \n\n At this moment, you can attach visual studio to the process with the name of 'Migrate.exe', put break point at any desired point in 'Migration_Seed.cs' file and then 'OK' this message in order to continue with debugging the Database Seeding, if no just 'OK' the message.");

                        //insertBasicInfo(context);

                        //if (MessageBox.Show("Update Action Types?", "Action Types", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //    insertActionTypes(context);

                        if (MessageBox.Show("Insert Users?", "Users", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            this.insertUsers(context);


                        if (MessageBox.Show("Insert Vessels?", "Vessels", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            context.Vessels.AddOrUpdate(FakeDomainService.GetVessels().ToArray());
                            context.SaveChanges();

                            context.VesselsInCompanies.AddOrUpdate(FakeDomainService.GetVesselsInCompanies().ToArray());
                            context.SaveChanges();
                        }

                        #region Add Voyages

                        if (MessageBox.Show("Insert Voyages?", "Voyages", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Execute.Sql("INSERT INTO [Fuel].[Voyage] ([Id], [VoyageNumber], [Description], [VesselInCompanyId], [CompanyId], [StartDate], [EndDate], [IsActive]) VALUES (19031555, N'IRS0225S', N'IRS0225S', 13, 1, N'2014-05-03 23:59:00', N'2014-05-05 23:59:00', 1)");
                            Execute.Sql("INSERT INTO [Fuel].[Voyage] ([Id], [VoyageNumber], [Description], [VesselInCompanyId], [CompanyId], [StartDate], [EndDate], [IsActive]) VALUES (19032323, N'HDM0233S', N'HDM0233S', 17, 1, N'2014-05-13 00:01:00', N'2014-06-17 23:59:00', 1)");
                            Execute.Sql("INSERT INTO [Fuel].[Voyage] ([Id], [VoyageNumber], [Description], [VesselInCompanyId], [CompanyId], [StartDate], [EndDate], [IsActive]) VALUES (19033111, N'HDM0242N', N'HDM0242N', 17, 1, N'2014-06-17 00:01:00', N'2014-07-11 23:59:00', 1)");
                            Execute.Sql("INSERT INTO [Fuel].[Voyage] ([Id], [VoyageNumber], [Description], [VesselInCompanyId], [CompanyId], [StartDate], [EndDate], [IsActive]) VALUES (19033112, N'HDM0242S', N'HDM0242S', 17, 1, N'2014-07-11 00:01:00', N'2014-08-10 23:59:00', 1)");


                            //context.Voyages.AddOrUpdate(FakeDomainService.GetVoyages().ToArray());
                            //context.SaveChanges();

                            //context.VoyagesLog.AddOrUpdate(FakeDomainService.CreateVoyagesLog(context.Voyages.FirstOrDefault()).ToArray());
                            //context.SaveChanges();
                        }
                        #endregion

                        if (MessageBox.Show("Insert Effective Factors?", "Effective Factors", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            context.EffectiveFactors.AddOrUpdate(FakeDomainService.GetEffectiveFactors().ToArray());
                            context.SaveChanges();
                        }


                        //if (MessageBox.Show("Insert FuelReport Importer Users?", "FuelReport Importer Users", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{
                        //    var fuelReportImporterUsers = context.FuelUsers.Where(u => u.Name.StartsWith("frimporter")).ToList();

                        //    foreach (var fuelReportImporterUser in fuelReportImporterUsers)
                        //    {
                        //        var importerCompanyId = fuelReportImporterUser.CompanyId;

                        //        var fmsUser = context.FuelUsers.Single(u => u.Name.StartsWith("fms") && u.CompanyId == importerCompanyId);
                        //        var initialToApprovedFuelReportStep = insertFuelReportWorkflowConfig(context, fmsUser.Id, fmsUser.Id);
                        //        //insertFuelReportsFakeData(context, userId, initialToApprovedFuelReportStep.Id);

                        //    }
                        //}


                        //if (MessageBox.Show("Insert Offhire System Mapping Data?", "Offhire System Mapping", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //    insertOffhireSystemMappingData();

                        //if (MessageBox.Show("Insert Workflow Configuration Data?", "Workflow Configuration", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{
                        //    foreach (var company in context.Companies.ToList())
                        //    {
                        //        InsertWorkflowConfigs(context, company.Id);
                        //    }

                        //    WFDataConvertor.ConcvertOldData(this.ConnectionString);
                        //}

                        //if (MessageBox.Show("Modify Configuration Data for Financial FuelReport submit?", "Workflow Configuration", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{
                        //    foreach (var company in context.Companies.ToList())
                        //    {
                        //        modifyFuelReportWorkflowConfigForFinancialSubmit_9404041400(company.Id, context);
                        //    }
                        //}

                        //if (MessageBox.Show("Modify Configuration Data for Cancelling FuelReport?", "Workflow Configuration", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{
                        //    foreach (var company in context.Companies.ToList())
                        //    {
                        //        modifyFuelReportWorkflowConfigForCancel_9408071200(company.Id, context);
                        //    }
                        //}

                        //transaction.Commit();
                        transactionScope.Complete();
                    }
                }
            }
            catch
            {
                //transaction.Rollback();
                //context.Database.Connection.Close();

                //Database.Delete("DataContainer");
                MessageBox.Show("Some errors have been occured!");
                throw;
            }
        }

        private void insertUsers(DataContainer context)
        {
            const long irislCompanyId = 1;
            const long sapidCompanyId = 2;
            const long hafezCompanyId = 5;


            int id = 1;

            /* var fueluser = new User(id, "FuelUser1", "f1", "l", "e", "fueluser");
             fueluser.UpdateCompany(1);
             context.Parties.Add(fueluser);

             insertWorkflowConfigs(context, id);*/

            id++;

            /*var fueluseririsl = new User(id, "fmsirisl", "f2", "fmsirisl", "e", "fmsirisl");
            fueluseririsl.UpdateCompany(1);
            context.Parties.Add(fueluseririsl);

            insertWorkflowConfigs(context, id);

            var customAction1 = new PartyCustomAction(id, ActionType.ManageFuelReportApprovement.Id, true);
            context.PartyCustomActions.Add(customAction1);


            insertFuelReportWorkflowConfig(context, id, id);*/


            id++;

            /* var fuelusersapid = new User(id, "fmssapid", "f3", "fmssapid", "e", "fmssapid");
             fuelusersapid.UpdateCompany(2);
             context.Parties.Add(fuelusersapid);

             insertWorkflowConfigs(context, id);

            var customAction2 = new PartyCustomAction(id, ActionType.ManageFuelReportApprovement.Id, true);
            context.PartyCustomActions.Add(customAction2);

            insertFuelReportWorkflowConfig(context, id, id);*/

            id++;

            /*var fueluserhafiz = new User(id, "fmshafiz", "f4", "fmshafiz", "e", "fmshafiz");
            fueluserhafiz.UpdateCompany(3);
            context.Parties.Add(fueluserhafiz);

            insertWorkflowConfigs(context, id);

            var customAction3 = new PartyCustomAction(id, ActionType.ManageFuelReportApprovement.Id, true);
            context.PartyCustomActions.Add(customAction3);

            insertFuelReportWorkflowConfig(context, id, id);*/

            id++;




            /*var fuelReportImporter = new User(id, "frimporter", "", "frimporter", "e", "frimporter");
            fuelReportImporter.UpdateCompany(1);
            context.Parties.Add(fuelReportImporter);

            var customAction = new PartyCustomAction(id, ActionType.ImportFuelReports.Id, true);
            context.PartyCustomActions.Add(customAction);
            context.SaveChanges();*/

            id++;


            //var gmfmshafiz = new User(id, "gmfmshafiz", "", "Capt. Safara", "safara@irisl.net", "gmfmshafiz");
            //gmfmshafiz.UpdateCompany(3);
            //context.Parties.Add(gmfmshafiz);

            //insertWorkflowConfigs(context, id);

            id++;


            //var comhafiz = new User(id, "comhafiz", "", "Capt. Zareei", "zareei@irisl.net", "comhafiz");
            //comhafiz.UpdateCompany(3);
            //context.Parties.Add(comhafiz);

            //insertWorkflowConfigs(context, id);

            //var customAction3 = new PartyCustomAction(id, ActionType.ManageFuelReportApprovement.Id, true);
            //context.PartyCustomActions.Add(customAction3);


            //insertFuelReportWorkflowConfig(context, id, id);

            id++;

            //var finhafiz = new User(id, "finhafiz", "", "Mrs. Aghamohammadi", "aghamohammadi@irisl.net", "finhafiz");
            //finhafiz.UpdateCompany(3);
            //context.Parties.Add(finhafiz);

            //insertWorkflowConfigs(context, id);

            id++;

            //var sadeghpoorhafiz = new User(id, "sadeghpoor", "", "Mr. Sadeghpoor", "sadeghpoor@irisl.net", "sadeghpoor");
            //sadeghpoorhafiz.UpdateCompany(3);
            //context.Parties.Add(sadeghpoorhafiz);

            id++;

            //var yeganehhafiz = new User(id, "yeganeh", "", "Mr. Yeganeh", "yeganeh@irisl.net", "yeganeh");
            //yeganehhafiz.UpdateCompany(3);
            //context.Parties.Add(yeganehhafiz);

            id++; //11
            id++; //12

            id++; //13

            /*var z_ghavamsapid = new User(id, "z.ghavam", "", "Mr. z.ghavam", "z.ghavam@irisl.net", "z.ghavam");
            z_ghavamsapid.UpdateCompany(sapidCompanyId);
            context.Parties.Add(z_ghavamsapid);

            grantActionToUser(context, z_ghavamsapid.Id,
                            ActionType.ViewFuelReports,
                            ActionType.EditFuelReport,
                            ActionType.ImportFuelReports,
                            ActionType.ManageFuelReportApprovement,
                            ActionType.ManageFuelReportSubmittion,
                            ActionType.ViewScraps,
                            ActionType.ViewOrders,
                            ActionType.ViewInvoices,
                            ActionType.ViewOffhires,
                            //ActionType.ImportOffhire,
                            //ActionType.EditOffhire,
                            //ActionType.RemoveOffhire,
                            //ActionType.ManageOffhireApprovement,
                            //ActionType.ManageOffhireSubmittion,
                            ActionType.ViewVoyages, 
                            ActionType.QueryGoods,
                            ActionType.QueryVoyages,
                            ActionType.QueryCompanies
                );
            //denyActionFromUser();

            //var z_ghavamsapidInventoryUser = new Inventory_User()
            //                                 {
            //                                     Id = (int)z_ghavamsapid.Id,
            //                                     Code = (int)z_ghavamsapid.Id,
            //                                     CreateDate =  DateTime.Now,
            //                                     EmailAddress = z_ghavamsapid.Email,
            //                                     UserName = z_ghavamsapid.UserName,
            //                                     Name = string.Format("{0} {1}",  z_ghavamsapid.FirstName, z_ghavamsapid.LastName),
            //                                     IsActive = true,
            //                                     Password = "****",
            //                                     UserCreatorId = 100000
            //                                 };

            //context.Inventory_User.Add(z_ghavamsapidInventoryUser);

            context.SaveChanges();

            assignUserToFuelReportApprovementFlow(context, sapidCompanyId, z_ghavamsapid.Id);
            */
            id++;

            /* var m_mohammadisapid = new User(id, "m.mohammadi", "", "Ms. m.mohammadi", "m.mohammadi@irisl.net", "m.mohammadi");
             m_mohammadisapid.UpdateCompany(sapidCompanyId);
             context.Parties.Add(m_mohammadisapid);

             grantActionToUser(context, m_mohammadisapid.Id,
                             ActionType.ViewFuelReports,
                             ActionType.EditFuelReport,
                             ActionType.ImportFuelReports,
                             ActionType.ManageFuelReportApprovement,
                             ActionType.ManageFuelReportSubmittion,
                             ActionType.ViewScraps,
                             ActionType.ViewOrders,
                             ActionType.ViewInvoices,
                             ActionType.ViewOffhires,
                 //ActionType.ImportOffhire,
                 //ActionType.EditOffhire,
                 //ActionType.RemoveOffhire,
                 //ActionType.ManageOffhireApprovement,
                 //ActionType.ManageOffhireSubmittion,
                             ActionType.ViewVoyages,
                             ActionType.QueryGoods,
                             ActionType.QueryVoyages,
                             ActionType.QueryCompanies
                 );
             //denyActionFromUser();

             //var m_mohammadisapidInventoryUser = new Inventory_User()
             //{
             //    Id = (int)m_mohammadisapid.Id,
             //    Code = (int)m_mohammadisapid.Id,
             //    CreateDate = DateTime.Now,
             //    EmailAddress = m_mohammadisapid.Email,
             //    UserName = m_mohammadisapid.UserName,
             //    Name = string.Format("{0} {1}", m_mohammadisapid.FirstName, m_mohammadisapid.LastName),
             //    IsActive = true,
             //    Password = "****",
             //    UserCreatorId = 100000
             //};

             //context.Inventory_User.Add(m_mohammadisapidInventoryUser);

             context.SaveChanges();


             assignUserToFuelReportApprovementFlow(context, sapidCompanyId, m_mohammadisapid.Id);
             */
            id++;

            /*var h_purkavehsapid = new User(id, "h.purkaveh", "", "Mr. h.purkaveh", "h.purkaveh@irisl.net", "h.purkaveh");
            h_purkavehsapid.UpdateCompany(sapidCompanyId);
            context.Parties.Add(h_purkavehsapid);

            grantActionToUser(context, h_purkavehsapid.Id,
                            ActionType.ViewFuelReports,
                            ActionType.EditFuelReport,
                            ActionType.ImportFuelReports,
                            ActionType.ManageFuelReportApprovement,
                            ActionType.ManageFuelReportSubmittion,
                            ActionType.ViewScraps,
                            ActionType.ViewOrders,
                            ActionType.ViewInvoices,
                            ActionType.ViewOffhires,
                //ActionType.ImportOffhire,
                //ActionType.EditOffhire,
                //ActionType.RemoveOffhire,
                //ActionType.ManageOffhireApprovement,
                //ActionType.ManageOffhireSubmittion,
                            ActionType.ViewVoyages,
                            ActionType.QueryGoods,
                            ActionType.QueryVoyages,
                            ActionType.QueryCompanies
                );
            //denyActionFromUser();
            context.SaveChanges();
            assignUserToFuelReportApprovementFlow(context, sapidCompanyId, h_purkavehsapid.Id);
            */
            id++;

            //var zakerfathi = new User(id, "zakerfathi", "", "Capt. Zakerfathi", "zakerfathi@irisl.net", "zakerfathi");
            //zakerfathi.UpdateCompany(sapidCompanyId);
            //context.Parties.Add(zakerfathi);

            //context.SaveChanges();

            //assignUserToAllApprovementFlows(context, sapidCompanyId, zakerfathi.Id);

            id++;

            //var amoozadeh = new User(id, "amoozadeh", "", "Capt. Amoozadeh", "amoozadeh@irisl.net", "amoozadeh");
            //amoozadeh.UpdateCompany(sapidCompanyId);
            //context.Parties.Add(amoozadeh);

            //context.SaveChanges();

            ////assignUserToAllApprovementFlows(context, sapidCompanyId, amoozadeh.Id);

            id++;

            //var seyyedi = new User(id, "seyyedi", "", "Capt. Seyyedi", "seyyedi@irisl.net", "seyyedi");
            //seyyedi.UpdateCompany(sapidCompanyId);
            //context.Parties.Add(seyyedi);

            //context.SaveChanges();

            ////assignUserToAllApprovementFlows(context, sapidCompanyId, seyyedi.Id);

            id++;


            context.SaveChanges();
        }

        private void denyActionFromUser(DataContainer context, long userId, params ActionType[] actionTypes)
        {
            foreach (var actionType in actionTypes)
            {
                var customAction = new PartyCustomAction(userId, actionType.Id, false);
                context.PartyCustomActions.Add(customAction);
            }
        }

        private void grantActionToUser(DataContainer context, long userId, params ActionType[] actionTypes)
        {
            foreach (var actionType in actionTypes)
            {
                var customAction = new PartyCustomAction(userId, actionType.Id, true);
                context.PartyCustomActions.Add(customAction);
            }
        }

        /*  User assignment to Workflows is depricated.
        private void assignUserToAllApprovementFlows(DataContainer context, long companyId, long userId)
        {
            assignUserToFuelReportApprovementFlow(context, companyId, userId);
            assignUserToOrderApprovementFlow(context, companyId, userId);
            assignUserToInvoiceApprovementFlow(context, companyId, userId);
            assignUserToOffhireApprovementFlow(context, companyId, userId);
            assignUserToScrapApprovementFlow(context, companyId, userId);
            assignUserToCharterInStartApprovementFlow(context, companyId, userId);
            assignUserToCharterInEndApprovementFlow(context, companyId, userId);
            assignUserToCharterOutStartApprovementFlow(context, companyId, userId);
            assignUserToCharterOutEndApprovementFlow(context, companyId, userId);
        }

        private void assignUserToFuelReportApprovementFlow(DataContainer context, long companyId, long userId)
        {
            var fuelUser = context.FuelUsers.Single(fu => fu.IdentityId == userId);

            var v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.FuelReport && wf.State == States.Open &&
                   wf.CurrentWorkflowStage == WorkflowStages.Initial && wf.WithWorkflowAction == WorkflowActions.Approve &&
                   wf.ActorUsers.Any(u => u.CompanyId == companyId)
                   ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.FuelReport && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.FuelReport && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            // context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.FuelReport && wf.State == States.Submitted &&
                wf.CurrentWorkflowStage == WorkflowStages.Submited && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.FuelReport && wf.State == States.SubmitRejected &&
                wf.CurrentWorkflowStage == WorkflowStages.SubmitRejected && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            context.SaveChanges();
        }

        private void assignUserToOrderApprovementFlow(DataContainer context, long companyId, long userId)
        {
            var fuelUser = context.FuelUsers.Single(fu => fu.IdentityId == userId);

            var v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Order && wf.State == States.Open &&
                   wf.CurrentWorkflowStage == WorkflowStages.Initial && wf.WithWorkflowAction == WorkflowActions.Approve &&
                   wf.ActorUsers.Any(u => u.CompanyId == companyId)
                   ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Order && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Order && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            // context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Order && wf.State == States.Submitted
                && wf.CurrentWorkflowStage == WorkflowStages.Submited
                && wf.WithWorkflowAction == WorkflowActions.Close
                //&& wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Order && wf.State == States.Submitted &&
                wf.CurrentWorkflowStage == WorkflowStages.Submited && wf.WithWorkflowAction == WorkflowActions.Cancel &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Order && wf.State == States.Cancelled &&
                wf.CurrentWorkflowStage == WorkflowStages.Canceled && wf.WithWorkflowAction == WorkflowActions.Cancel &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Order && wf.State == States.Closed &&
               wf.CurrentWorkflowStage == WorkflowStages.Closed && wf.WithWorkflowAction == WorkflowActions.None &&
               wf.ActorUsers.Any(u => u.CompanyId == companyId)
               ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            context.SaveChanges();
        }

        private void assignUserToInvoiceApprovementFlow(DataContainer context, long companyId, long userId)
        {
            var fuelUser = context.FuelUsers.Single(fu => fu.IdentityId == userId);

            var v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Invoice && wf.State == States.Open &&
                   wf.CurrentWorkflowStage == WorkflowStages.Initial && wf.WithWorkflowAction == WorkflowActions.Approve &&
                   wf.ActorUsers.Any(u => u.CompanyId == companyId)
                   ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Invoice && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Invoice && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            // context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Invoice && wf.State == States.Submitted &&
                wf.CurrentWorkflowStage == WorkflowStages.Submited && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Invoice && wf.State == States.Submitted &&
                wf.CurrentWorkflowStage == WorkflowStages.Submited && wf.WithWorkflowAction == WorkflowActions.Cancel &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Invoice && wf.State == States.Cancelled &&
                wf.CurrentWorkflowStage == WorkflowStages.Canceled && wf.WithWorkflowAction == WorkflowActions.Cancel &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));

            context.SaveChanges();
        }

        private void assignUserToOffhireApprovementFlow(DataContainer context, long companyId, long userId)
        {
            var fuelUser = context.FuelUsers.Single(fu => fu.IdentityId == userId);

            var v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Offhire && wf.State == States.Open &&
                   wf.CurrentWorkflowStage == WorkflowStages.Initial && wf.WithWorkflowAction == WorkflowActions.Approve &&
                   wf.ActorUsers.Any(u => u.CompanyId == companyId)
                   ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Offhire && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Offhire && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            // context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Offhire && wf.State == States.Submitted &&
                wf.CurrentWorkflowStage == WorkflowStages.Submited && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Offhire && wf.State == States.SubmitRejected &&
                wf.CurrentWorkflowStage == WorkflowStages.SubmitRejected && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            context.SaveChanges();
        }

        private void assignUserToScrapApprovementFlow(DataContainer context, long companyId, long userId)
        {
            var fuelUser = context.FuelUsers.Single(fu => fu.IdentityId == userId);

            var v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Scrap && wf.State == States.Open &&
                   wf.CurrentWorkflowStage == WorkflowStages.Initial && wf.WithWorkflowAction == WorkflowActions.Approve &&
                   wf.ActorUsers.Any(u => u.CompanyId == companyId)
                   ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Scrap && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Scrap && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            // context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Scrap && wf.State == States.Submitted &&
                wf.CurrentWorkflowStage == WorkflowStages.Submited && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.Scrap && wf.State == States.SubmitRejected &&
                wf.CurrentWorkflowStage == WorkflowStages.SubmitRejected && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            context.SaveChanges();
        }

        private void assignUserToCharterInStartApprovementFlow(DataContainer context, long companyId, long userId)
        {
            var fuelUser = context.FuelUsers.Single(fu => fu.IdentityId == userId);

            var v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterInStart && wf.State == States.Open &&
                   wf.CurrentWorkflowStage == WorkflowStages.Initial && wf.WithWorkflowAction == WorkflowActions.Approve &&
                   wf.ActorUsers.Any(u => u.CompanyId == companyId)
                   ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterInStart && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterInStart && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            // context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterInStart && wf.State == States.Submitted &&
                wf.CurrentWorkflowStage == WorkflowStages.Submited && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterInStart && wf.State == States.SubmitRejected &&
                wf.CurrentWorkflowStage == WorkflowStages.SubmitRejected && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            context.SaveChanges();
        }

        private void assignUserToCharterInEndApprovementFlow(DataContainer context, long companyId, long userId)
        {
            var fuelUser = context.FuelUsers.Single(fu => fu.IdentityId == userId);

            var v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterInEnd && wf.State == States.Open &&
                   wf.CurrentWorkflowStage == WorkflowStages.Initial && wf.WithWorkflowAction == WorkflowActions.Approve &&
                   wf.ActorUsers.Any(u => u.CompanyId == companyId)
                   ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterInEnd && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterInEnd && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            // context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterInEnd && wf.State == States.Submitted &&
                wf.CurrentWorkflowStage == WorkflowStages.Submited && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterInEnd && wf.State == States.SubmitRejected &&
                wf.CurrentWorkflowStage == WorkflowStages.SubmitRejected && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            context.SaveChanges();
        }

        private void assignUserToCharterOutStartApprovementFlow(DataContainer context, long companyId, long userId)
        {
            var fuelUser = context.FuelUsers.Single(fu => fu.IdentityId == userId);

            var v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterOutStart && wf.State == States.Open &&
                   wf.CurrentWorkflowStage == WorkflowStages.Initial && wf.WithWorkflowAction == WorkflowActions.Approve &&
                   wf.ActorUsers.Any(u => u.CompanyId == companyId)
                   ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterOutStart && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterOutStart && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            // context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterOutStart && wf.State == States.Submitted &&
                wf.CurrentWorkflowStage == WorkflowStages.Submited && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterOutStart && wf.State == States.SubmitRejected &&
                wf.CurrentWorkflowStage == WorkflowStages.SubmitRejected && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            context.SaveChanges();
        }

        private void assignUserToCharterOutEndApprovementFlow(DataContainer context, long companyId, long userId)
        {
            var fuelUser = context.FuelUsers.Single(fu => fu.IdentityId == userId);

            var v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterOutEnd && wf.State == States.Open &&
                   wf.CurrentWorkflowStage == WorkflowStages.Initial && wf.WithWorkflowAction == WorkflowActions.Approve &&
                   wf.ActorUsers.Any(u => u.CompanyId == companyId)
                   ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterOutEnd && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterOutEnd && wf.State == States.Open &&
                wf.CurrentWorkflowStage == WorkflowStages.Approved && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            // context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterOutEnd && wf.State == States.Submitted &&
                wf.CurrentWorkflowStage == WorkflowStages.Submited && wf.WithWorkflowAction == WorkflowActions.Reject &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            v = context.ApproveFlows.Where(wf => wf.WorkflowEntity == WorkflowEntities.CharterOutEnd && wf.State == States.SubmitRejected &&
                wf.CurrentWorkflowStage == WorkflowStages.SubmitRejected && wf.WithWorkflowAction == WorkflowActions.Approve &&
                wf.ActorUsers.Any(u => u.CompanyId == companyId)
                ).ToList();

            v.ForEach(ws => ws.ActorUsers.Add(fuelUser));
            //context.SaveChanges();

            context.SaveChanges();
        }
        */

        private static T Construct<T>(Type[] paramTypes, object[] paramValues)
        {
            //A method to Construct objects via private constructors to bypass business rules checkings.
            Type t = typeof(T);

            ConstructorInfo ci = t.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, paramTypes, null);

            return (T)ci.Invoke(paramValues);
        }

        private static T Construct<T>(params object[] paramValues)
        {
            //A method to Construct objects via private constructors to bypass business rules checkings.
            var paramTypes = paramValues.Select(v => v.GetType()).ToArray();
            return Construct<T>(paramTypes, paramValues);
        }

        public override void Down()
        {
            //Nothing to do.
        }
    }
}
