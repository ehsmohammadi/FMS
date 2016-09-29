using System;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.FakeDomainServices
{
    public static class FakeDomainService
    {
        public static List<VesselInCompany> GetVesselsInCompanies()
        {
            return new List<VesselInCompany>
                       {                          
                            new VesselInCompany("TOOSKA", "TOOSKA / IRISL",    1, 1, VesselStates.Inactive, false),
                            new VesselInCompany("TOOSKA", "TOOSKA / SAPID",    2, 1, VesselStates.Inactive, false),
                            new VesselInCompany("SARVIN","SARVIN / IRISL",     1, 2, VesselStates.Inactive, false),
                            new VesselInCompany("SARVIN","SARVIN / SAPID",     2, 2, VesselStates.Inactive, false),
                            new VesselInCompany("APPOLO","APPOLO / IRISL",	   1, 3, VesselStates.Inactive, false),
                            new VesselInCompany("APPOLO","APPOLO / SAPID",	   2, 3, VesselStates.Inactive, false),
                            new VesselInCompany("AEROLITE","AEROLITE / IRISL", 1, 4, VesselStates.Inactive, false),
                            new VesselInCompany("AEROLITE","AEROLITE / SAPID", 2, 4, VesselStates.Inactive, false),
                            new VesselInCompany("PERARIN","PERARIN / IRISL",   1, 5, VesselStates.Inactive, false),
                            new VesselInCompany("PERARIN","PERARIN / HAFIZ",   3, 5, VesselStates.Inactive, false),
                            new VesselInCompany("PENDAR","PENDAR / IRISL",     1, 6, VesselStates.Inactive, false),
                            new VesselInCompany("PENDAR","PENDAR / HAFIZ",     3, 6, VesselStates.Inactive, false),
                            new VesselInCompany("SHABDIS","SHABDIS / IRISL",   1, 7, VesselStates.Inactive, false),
                            new VesselInCompany("SHABDIS","SHABDIS / HAFIZ",   3, 7, VesselStates.Inactive, false),

                            new VesselInCompany("ZARDIS","ZARDIS / IRISL",     1, 8, VesselStates.Inactive, false),
                            new VesselInCompany("ZARDIS","ZARDIS / HAFIZ",     3, 8, VesselStates.Inactive, false),
                            new VesselInCompany("SHAHRAZ","SHAHRAZ / IRISL",   1, 9, VesselStates.Inactive, false),
                            new VesselInCompany("SHAHRAZ","SHAHRAZ / HAFIZ",   3, 9, VesselStates.Inactive, false),
                       };
        }


        public static List<Vessel> GetVessels()
        {
            return new List<Vessel>
                       {
                           new Vessel("0168", 1),  //IRISL      1
                           new Vessel("3051", 1),  //IRISL      2
                           new Vessel("0110", 1),  //IRISL      3
                           new Vessel("0092", 1),  //IRISL      4
                           new Vessel("0151", 1),  //IRISL      5
                           new Vessel("3050", 1),  //IRISL      6
                           new Vessel("0165", 1),  //IRISL      7
                           new Vessel("0173", 1),  //IRISL      8
                           new Vessel("0164", 1),  //IRISL      9

                       };
        }

        public static List<Voyage> GetVoyages()
        {
            return new List<Voyage>
                       {
                           new Voyage(19031556, "S4-1089", "S4-1089", 1, 11, new DateTime(2012, 1, 1, 0, 0, 0), new DateTime(2014, 1, 2, 17, 0, 0)),
                           new Voyage(190315557, "S4-1092", "S4-1092", 1, 11, new DateTime(2014, 1, 2, 17, 0, 1), new DateTime(2014, 12, 12, 8, 0, 0)),
//                           Id	VoyageNumber	Description	VesselInCompanyId	CompanyId	StartDate	EndDate	IsActive
//19031555	IRS0225S	IRS0225S	13	1	2014-05-03 23:59:00.000	2014-05-05 23:59:00.000	1new Voyage(19031555, "S4-1092", "S4-1092", 1, 11, new DateTime(2014, 1, 2, 17, 0, 1), new DateTime(2014, 12, 12, 8, 0, 0)),
                       };
        }

        public static List<VoyageLog> CreateVoyagesLog(Voyage voyage)
        {
            return new List<VoyageLog>();
        }

        public static List<FuelReport> GetFuelReports()
        {
            return new List<FuelReport>
                       {
                           //new FuelReport("NoonReport1", "NoonReport1", new DateTime(2014, 1, 1, 12, 0, 0), new DateTime(2014, 1, 2, 17, 0, 0), 1, 1,
                           //               FuelReportTypes.NoonReport, States.Open),
                           //new FuelReport("NoonReport2", "NoonReport2", new DateTime(2014, 1, 2, 12, 0, 0), new DateTime(2014, 1, 2, 17, 0, 0), 1, null,
                           //               FuelReportTypes.NoonReport, States.Open),
                           //new FuelReport("EndOfVoyage1", "EndOfVoyage1", new DateTime(2014, 1, 2, 17, 0, 0), new DateTime(2014, 1, 2, 17, 0, 0),1, 1,
                           //               FuelReportTypes.EndOfVoyage, States.Open),
                           //new FuelReport("NoonReport1", "NoonReport1", new DateTime(2014, 1, 3, 12, 0, 0), new DateTime(2014, 1, 2, 17, 0, 0),1, 2,
                           //               FuelReportTypes.NoonReport, States.Open),
                           //new FuelReport("NoonReport2", "NoonReport2", new DateTime(2014, 1, 4, 12, 0, 0), new DateTime(2014, 1, 2, 17, 0, 0),1, 1,
                           //               FuelReportTypes.NoonReport, States.Open),                     
                           //new FuelReport("NoonReport3", "NoonReport3", new DateTime(2014, 1, 5, 12, 0, 0), new DateTime(2014, 1, 2, 17, 0, 0),1, 2,
                           //               FuelReportTypes.NoonReport, States.Open),
                           //new FuelReport("EndOfVoyage2", "EndOfVoyage2", new DateTime(2014, 1, 6, 8, 0, 0), new DateTime(2014, 1, 2, 17, 0, 0),1,2,
                           //               FuelReportTypes.EndOfVoyage, States.Open),
                           
                       };
        }

        public static List<FuelReportDetail> GetFuelReportDetails(List<FuelReport> fuelReports)
        {
            return new List<FuelReportDetail>
                       {
                           new FuelReportDetail(0, fuelReports[0].Id, 470, "Ton", 15, 0, null, 0, null, (decimal?)12.5, CorrectionTypes.Minus, CorrectionPricingTypes.Default, 700,"USD", 1, 11010, 110101, 12001),
                           new FuelReportDetail(0, fuelReports[0].Id, 120, "Ton", 2, 50, ReceiveTypes.InternalTransfer, 20,TransferTypes.TransferSale, null,null,CorrectionPricingTypes.Default, null,"USD",null, 11011, 110112, 12001),
                                                                           
                           new FuelReportDetail(0, fuelReports[1].Id, 430, "Ton", 13, 13, ReceiveTypes.TransferPurchase, 0,null,  null,null,CorrectionPricingTypes.Default, null,"USD",null, 11010, 110101, 12001),
                           new FuelReportDetail(0, fuelReports[1].Id, 100, "Ton", (decimal)2.5, 0, null, 50,TransferTypes.TransferSale, 1, CorrectionTypes.Minus,CorrectionPricingTypes.Default, 1900, "USD",1, 11011, 110112, 12001),
                                                                          
                           new FuelReportDetail(0, fuelReports[2].Id, 410, "Ton", 20, null,null,null,null,null,null,CorrectionPricingTypes.Default, null,"USD",null,11010, 110101, 12001),
                           new FuelReportDetail(0, fuelReports[2].Id, 70, "Ton", 0, 111, ReceiveTypes.TransferPurchase, 50,TransferTypes.TransferSale, 1, CorrectionTypes.Minus,CorrectionPricingTypes.Default,  1600, "USD",1, 11011, 110112, 12001),
                                                                          
                           new FuelReportDetail(0, fuelReports[3].Id, 380, "Ton", 12, 0, null, 65,TransferTypes.InternalTransfer, null,null,CorrectionPricingTypes.Default, null,"USD",null, 11010, 110101, 12001),
                           new FuelReportDetail(0, fuelReports[3].Id, 140, "Ton", 5, 50, ReceiveTypes.TransferPurchase, 10,TransferTypes.TransferSale, 30, CorrectionTypes.Plus,CorrectionPricingTypes.Default,  1800, "USD",1, 11011, 110112, 12001),
                                                                         
                           new FuelReportDetail(0, fuelReports[4].Id, 350, "Ton", 20, 13, ReceiveTypes.TransferPurchase, 54,TransferTypes.TransferSale, 1, CorrectionTypes.Minus,CorrectionPricingTypes.Default,  900,"USD", 1, 11010, 110101, 12001),
                           new FuelReportDetail(0, fuelReports[4].Id, 130, "Ton", 10, 111, ReceiveTypes.TransferPurchase, 50,TransferTypes.TransferSale, 1, CorrectionTypes.Plus,CorrectionPricingTypes.Default,  1850, "USD",1, 11011, 110112, 12001),
                                                                       
                           new FuelReportDetail(0, fuelReports[5].Id, 380, "Ton", 11, 0, null,null,null,null,null,CorrectionPricingTypes.Default, null,"USD",null,11010, 110101, 12001),
                           new FuelReportDetail(0, fuelReports[5].Id, 100, "Ton", 10, 0, null, 0,null, 0, null, CorrectionPricingTypes.Default, 200, "USD",1, 11011, 110112, 12001),
                       };
        }

        public static List<Unit> GetUnits()
        {
            return new List<Unit>
                       {
                           new Unit(1, "Metric Ton"),
                           new Unit(2, "Liter"),
                           new Unit(3, "Long Ton")
                       };
        }

        public static List<GoodUnit> GetCompanyGoodUnits()
        {
            return new List<GoodUnit>
                       {
                           new GoodUnit(1, 1, null, "Ton", 1, "Ton"),
                           new GoodUnit(2, 1, 1, "Liter", (decimal)0.990, "Liter"),
                           new GoodUnit(3, 1, 2, "Long Ton", (decimal)1.2*1000, "Long Ton"),
                           new GoodUnit(4, 2, null, "Ton", 1, "Ton"),
                           new GoodUnit(5, 2, 4, "Liter", (decimal)0.990, "Liter"),
                           new GoodUnit(6, 2, 5, "Long Ton", (decimal)1.2*1000, "Long Ton"),
                       };
        }

        public static List<Good> GetGoods()
        {
            var res1 = new Good(1, "Heavy Fuel Oil", "HFO", 1, 1);
            var res2 = new Good(2, "Marine Diesel Oil", "MDO", 2, 1);
            return new List<Good>
                   {
                      res1,res2
                   };
        }

        public static List<SharedGood> GetShareGoods()
        {
            return new List<SharedGood> { new SharedGood(1, "Shared Good 1", "SharedGood1", 1), new SharedGood(2, "Shared Good 2", "SharedGood2", 2), };
        }

        public static List<EffectiveFactor> GetEffectiveFactors()
        {
            return new List<EffectiveFactor>
                       {
                           new EffectiveFactor("مالیات ", EffectiveFactorTypes.Increase,"vv","jj"),
                           new EffectiveFactor("تخفیف", EffectiveFactorTypes.Increase,"vv","jj"),
                           new EffectiveFactor("حمل و نقل", EffectiveFactorTypes.Decrease,"vv","jj"),
                       };
        }
    }
}
