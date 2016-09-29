using System;
using System.Linq;
using FuelReportImporter.Database.VesselReportsDataSetTableAdapters;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Repositories;
using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.VesselInCompanyAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Domain.Repository;
using MITD.DataAccess.EF;
using System.Collections.Generic;

namespace FuelReportImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var fuelContext = new DataContainer();
            //UnitOfMeasuresAndCurrenciesRegsitrar.ReloadData();

            var reportTableAdapter = new VesselReportTableAdapter();
            var reportDetailsTableAdapter = new VesselReportDetailsTableAdapter();

            //CASE ISNULL(VRFR.FuelRepotType , 1)
            //WHEN 1 THEN 1
            //WHEN 2 THEN 2
            //WHEN 3 THEN 8
            //WHEN 4 THEN 7 
            //WHEN 5 THEN 5 
            //WHEN 6 THEN 6 
            //WHEN 7 THEN 3 
            //WHEN 8 THEN 4 


            var reportDataTable = reportTableAdapter.GetData(null);

            var usersCompanies = new Dictionary<long, long>();

            usersCompanies.Add(4, 3);//HAFIZ User
            usersCompanies.Add(3, 2);//SAPID User


            //var initialToApprovedFuelReportStep = fuelContext.ApproveFlows.Where(
            //    fl =>
            //        fl.WorkflowEntity == WorkflowEntities.FuelReport &&
            //        fl.ActorUserId == userId &&
            //        fl.CurrentWorkflowStage == WorkflowStages.Initial &&
            //        fl.WithWorkflowAction == WorkflowActions.Approve);

            var efUnitOfWork = new UnitOfWorkScope(new EFUnitOfWorkFactory(() => fuelContext));

            var vesselInInventoryRepository = new VesselInInventoryRepository(efUnitOfWork);
            var vesselInCompanyStateFactory = new VesselInCompanyStateFactory();

            var fuelReportFactory = new FuelReportFactory(
                new FuelReportConfigurator(
                    new FuelReportStateFactory(),
                    vesselInInventoryRepository,
                    vesselInCompanyStateFactory),
                new WorkflowRepository(efUnitOfWork),
                new VesselInCompanyRepository(efUnitOfWork, new VesselInCompanyConfigurator(vesselInCompanyStateFactory, vesselInInventoryRepository)));

            foreach (var userId in usersCompanies.Keys)
            {
                var companyId = usersCompanies[userId];

                foreach (var reportRow in reportDataTable)
                {
                    if (reportRow.CompanyId != companyId) continue;

                    var reportDetailsDataTable = reportDetailsTableAdapter.GetData(reportRow.Code, reportRow.CompanyId);

                    var fuelReport = fuelReportFactory.CreateFuelReport(reportRow.Code.ToString(), reportRow.Description, reportRow.EventDate, reportRow.ReportDate, reportRow.VesselInCompanyId,
                        reportRow.IsVoyageIdNull() ? null : (long?)reportRow.VoyageId, (FuelReportTypes)reportRow.FuelReportType);


                    var vesselInInventory = fuelReport.VesselInCompany.VesselInInventory;

                    foreach (var reportDetailRow in reportDetailsDataTable)
                    {
                        var fuelReportDetail = fuelReportFactory.CreateFuelReportDetail(
                            0,
                            (decimal)Math.Round(reportDetailRow.ROB, 3),
                            "TON",
                            (decimal)Math.Round(reportDetailRow.Consumption, 3),
                            reportDetailRow.IsRecieveNull() ? null : (decimal?)Math.Round(reportDetailRow.Recieve, 3),
                            null,
                            reportDetailRow.IsTransferNull() ? null : (decimal?)Math.Round((double)reportDetailRow.Transfer, 3),
                            null,
                            reportDetailRow.IsCorrectionNull() ? null : (decimal?)Math.Round((double)reportDetailRow.Correction, 3),
                            reportDetailRow.IsCorrectionTypeNull() ? null : (CorrectionTypes?)(reportDetailRow.CorrectionType ? CorrectionTypes.Plus : CorrectionTypes.Minus),
                            null, 
                            null,
                            "USD", 
                            null,
                            reportDetailRow.GoodId,
                            reportDetailRow.GoodUnitId,
                            fuelContext.Tanks.First(t => t.VesselInInventoryId == vesselInInventory.Id).Id);

                        fuelReport.FuelReportDetails.Add(fuelReportDetail);
                    }

                    fuelContext.FuelReports.Add(fuelReport);
                }
            }

            fuelContext.SaveChanges();

            efUnitOfWork.Commit();
        }
    }
}
