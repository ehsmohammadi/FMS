using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.ModelBuilder.Descriptors;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Fuel.Presentation.Contracts.FacadeServices.Fuel;
using MITD.FuelSecurity.Domain;
using MITD.FuelSecurity.Domain.Model;

namespace MITD.Fuel.Application.Facade
{
    public class MethodMapper
    {
        public class MethodAction
        {
            public string ClassName { get; private set; }
            public string MethodName { get; private set; }
            public List<ActionType> Actions { get; private set; }
            public MethodAction(string className, string methodName, List<ActionType> actions)
            {
                ClassName = className;
                MethodName = methodName;
                Actions = actions;
            }
        }

        private readonly List<MethodAction> mapTable = new List<MethodAction>
        {
            #region chrter in
            new MethodAction(typeof(ICharterInFacadeService).Name,typeof(ICharterInFacadeService).GetMethods().First(m=>m.Name == "GetAll").Name,new List<ActionType>(){ActionType.QueryCharterIn}),
            new MethodAction(typeof(ICharterInFacadeService).Name,typeof(ICharterInFacadeService).GetMethods().First(m=>m.Name == "GetById").Name,new List<ActionType>(){ActionType.QueryCharterIn}),
            new MethodAction(typeof(ICharterInFacadeService).Name,typeof(ICharterInFacadeService).GetMethods().First(m=>m.Name == "GetCharterEnd").Name,new List<ActionType>(){ActionType.QueryCharterIn}),
            new MethodAction(typeof(ICharterInFacadeService).Name,typeof(ICharterInFacadeService).GetMethods().First(m=>m.Name == "GetAllItem").Name,new List<ActionType>(){ActionType.QueryCharterIn}),
            new MethodAction(typeof(ICharterInFacadeService).Name,typeof(ICharterInFacadeService).GetMethods().First(m=>m.Name == "GetItemById").Name,new List<ActionType>(){ActionType.QueryCharterIn}),
            new MethodAction(typeof(ICharterInFacadeService).Name,typeof(ICharterInFacadeService).GetMethods().First(m=>m.Name == "Add").Name,new List<ActionType>(){ActionType.AddCharterIn}),
            new MethodAction(typeof(ICharterInFacadeService).Name,typeof(ICharterInFacadeService).GetMethods().First(m=>m.Name == "Update").Name,new List<ActionType>(){ActionType.EditCharterIn}),
            new MethodAction(typeof(ICharterInFacadeService).Name,typeof(ICharterInFacadeService).GetMethods().First(m=>m.Name == "Delete").Name,new List<ActionType>(){ActionType.DeleteCharterIn}),
            new MethodAction(typeof(ICharterInFacadeService).Name,typeof(ICharterInFacadeService).GetMethods().First(m=>m.Name == "AddItem").Name,new List<ActionType>(){ActionType.AddCharterInItem}),
            new MethodAction(typeof(ICharterInFacadeService).Name,typeof(ICharterInFacadeService).GetMethods().First(m=>m.Name == "UpdateItem").Name,new List<ActionType>(){ActionType.EditCharterInItem}),
            new MethodAction(typeof(ICharterInFacadeService).Name,typeof(ICharterInFacadeService).GetMethods().First(m=>m.Name == "DeleteItem").Name,new List<ActionType>(){ActionType.DeleteCharterInItem}),

            #endregion
      
            #region chater out
            new MethodAction(typeof(ICharterOutFacadeService).Name,typeof(ICharterOutFacadeService).GetMethods().First(m=>m.Name == "GetAll").Name,new List<ActionType>(){ActionType.QueryCharterOut}),
            new MethodAction(typeof(ICharterOutFacadeService).Name,typeof(ICharterOutFacadeService).GetMethods().First(m=>m.Name == "GetById").Name,new List<ActionType>(){ActionType.QueryCharterOut}),
            new MethodAction(typeof(ICharterOutFacadeService).Name,typeof(ICharterOutFacadeService).GetMethods().First(m=>m.Name == "GetCharterEnd").Name,new List<ActionType>(){ActionType.QueryCharterOut}),
            new MethodAction(typeof(ICharterOutFacadeService).Name,typeof(ICharterOutFacadeService).GetMethods().First(m=>m.Name == "GetAllItem").Name,new List<ActionType>(){ActionType.QueryCharterOut}),
            new MethodAction(typeof(ICharterOutFacadeService).Name,typeof(ICharterOutFacadeService).GetMethods().First(m=>m.Name == "GetItemById").Name,new List<ActionType>(){ActionType.QueryCharterOut}),
            new MethodAction(typeof(ICharterOutFacadeService).Name,typeof(ICharterOutFacadeService).GetMethods().First(m=>m.Name == "Add").Name,new List<ActionType>(){ActionType.AddCharterOut}),
            new MethodAction(typeof(ICharterOutFacadeService).Name,typeof(ICharterOutFacadeService).GetMethods().First(m=>m.Name == "Update").Name,new List<ActionType>(){ActionType.EditCharterOut}),
            new MethodAction(typeof(ICharterOutFacadeService).Name,typeof(ICharterOutFacadeService).GetMethods().First(m=>m.Name == "Delete").Name,new List<ActionType>(){ActionType.DeleteCharterOut}),
            new MethodAction(typeof(ICharterOutFacadeService).Name,typeof(ICharterOutFacadeService).GetMethods().First(m=>m.Name == "AddItem").Name,new List<ActionType>(){ActionType.AddCharterOutItem}),
            new MethodAction(typeof(ICharterOutFacadeService).Name,typeof(ICharterOutFacadeService).GetMethods().First(m=>m.Name == "UpdateItem").Name,new List<ActionType>(){ActionType.EditCharterOutItem}),
            new MethodAction(typeof(ICharterOutFacadeService).Name,typeof(ICharterOutFacadeService).GetMethods().First(m=>m.Name == "DeleteItem").Name,new List<ActionType>(){ActionType.DeleteCharterOutItem}),
            #endregion
      
            #region Voucher
             new MethodAction(typeof(IVoucherFacadeService).Name,typeof(IVoucherFacadeService).GetMethods().First(m=>m.Name == "GetAll").Name,new List<ActionType>(){ActionType.QueryVoucher}),
              new MethodAction(typeof(IVoucherFacadeService).Name,typeof(IVoucherFacadeService).GetMethods().First(m=>m.Name == "GetById").Name,new List<ActionType>(){ActionType.QueryVoucher}),


            #endregion


            #region VoucherSeting
               new MethodAction(typeof(IVoucherSetingFacadeService).Name,typeof(IVoucherSetingFacadeService).GetMethods().First(m=>m.Name == "GetByFilter").Name,new List<ActionType>(){ActionType.QueryVoucherSeting}),
               new MethodAction(typeof(IVoucherSetingFacadeService).Name,typeof(IVoucherSetingFacadeService).GetMethods().First(m=>m.Name == "GetById").Name,new List<ActionType>(){ActionType.QueryVoucherSeting}),
               new MethodAction(typeof(IVoucherSetingFacadeService).Name,typeof(IVoucherSetingFacadeService).GetMethods().First(m=>m.Name == "GetDetailById").Name,new List<ActionType>(){ActionType.QueryVoucherSeting}),
             
               new MethodAction(typeof(IVoucherSetingFacadeService).Name,typeof(IVoucherSetingFacadeService).GetMethods().First(m=>m.Name == "AddVoucherSeting").Name,new List<ActionType>(){ActionType.AddVoucherSeting}),

               new MethodAction(typeof(IVoucherSetingFacadeService).Name,typeof(IVoucherSetingFacadeService).GetMethods().First(m=>m.Name == "UpdateVoucherSeting").Name,new List<ActionType>(){ActionType.UpdateVoucherSeting}),

               new MethodAction(typeof(IVoucherSetingFacadeService).Name,typeof(IVoucherSetingFacadeService).GetMethods().First(m=>m.Name == "AddVoucherSetingDetail").Name,new List<ActionType>(){ActionType.AddVoucherSetingDetail}),
               
               new MethodAction(typeof(IVoucherSetingFacadeService).Name,typeof(IVoucherSetingFacadeService).GetMethods().First(m=>m.Name == "UpdateVoucherSetingDetail").Name,new List<ActionType>(){ActionType.UpdateVoucherSetingDetail}),


            #endregion


            new MethodAction(typeof(IFuelReportFacadeService).Name,typeof(IFuelReportFacadeService).GetMethods().First(m=>m.Name == "GetById").Name,new List<ActionType>{ActionType.ViewFuelReports}),
            new MethodAction(typeof(IFuelReportFacadeService).Name,typeof(IFuelReportFacadeService).GetMethods().First(m=>m.Name == "GetFuelReportDetailById").Name,new List<ActionType>{ActionType.ViewFuelReports}),
            new MethodAction(typeof(IFuelReportFacadeService).Name,typeof(IFuelReportFacadeService).GetMethods().First(m=>m.Name == "GetByFilter").Name,new List<ActionType>{ActionType.ViewFuelReports}),
            new MethodAction(typeof(IFuelReportFacadeService).Name,typeof(IFuelReportFacadeService).GetMethods().First(m=>m.Name == "GetInventoryOperations").Name,new List<ActionType>{ActionType.ViewFuelReports}),
            new MethodAction(typeof(IFuelReportFacadeService).Name,typeof(IFuelReportFacadeService).GetMethods().First(m=>m.Name == "GetDetailInventoryOperations").Name,new List<ActionType>{ActionType.ViewFuelReports}),
            new MethodAction(typeof(IFuelReportFacadeService).Name,typeof(IFuelReportFacadeService).GetMethods().First(m=>m.Name == "Update").Name,new List<ActionType>{ActionType.EditFuelReport}),
            new MethodAction(typeof(IFuelReportFacadeService).Name,typeof(IFuelReportFacadeService).GetMethods().First(m=>m.Name == "UpdateFuelReportDetail").Name,new List<ActionType>{ActionType.EditFuelReport}),
            new MethodAction(typeof(IFuelReportFacadeService).Name,typeof(IFuelReportFacadeService).GetMethods().First(m=>m.Name == "UpdateFuelReportDetailByFinance").Name,new List<ActionType>{ActionType.EditFinancialFuelReport}),
            new MethodAction(typeof(IFuelReportFacadeService).Name,typeof(IFuelReportFacadeService).GetMethods().First(m=>m.Name == "Add").Name,new List<ActionType>{ActionType.ImportFuelReports}),
            new MethodAction(typeof(IFuelReportFacadeService).Name,typeof(IFuelReportFacadeService).GetMethods().First(m=>m.Name == "Delete").Name,new List<ActionType>{ActionType.RemoveFuelReport}),

            new MethodAction(typeof(IScrapFacadeService).Name,typeof(IScrapFacadeService).GetMethods().First(m=>m.Name == "GetById").Name,new List<ActionType>{ActionType.ViewScraps}),
            new MethodAction(typeof(IScrapFacadeService).Name,typeof(IScrapFacadeService).GetMethods().First(m=>m.Name == "GetPagedData").Name,new List<ActionType>{ActionType.ViewScraps}),
            new MethodAction(typeof(IScrapFacadeService).Name,typeof(IScrapFacadeService).GetMethods().First(m=>m.Name == "GetPagedDataByFilter").Name,new List<ActionType>{ActionType.ViewScraps}),
            new MethodAction(typeof(IScrapFacadeService).Name,typeof(IScrapFacadeService).GetMethods().First(m=>m.Name == "GetPagedScrapDetailData").Name,new List<ActionType>{ActionType.ViewScraps}),
            new MethodAction(typeof(IScrapFacadeService).Name,typeof(IScrapFacadeService).GetMethods().First(m=>m.Name == "GetScrapDetail").Name,new List<ActionType>{ActionType.ViewScraps}),
            new MethodAction(typeof(IScrapFacadeService).Name,typeof(IScrapFacadeService).GetMethods().First(m=>m.Name == "GetInventoryOperations").Name,new List<ActionType>{ActionType.ViewScraps}),
            new MethodAction(typeof(IScrapFacadeService).Name,typeof(IScrapFacadeService).GetMethods().First(m=>m.Name == "ScrapVessel").Name,new List<ActionType>{ActionType.CreateScrap}),
            new MethodAction(typeof(IScrapFacadeService).Name,typeof(IScrapFacadeService).GetMethods().First(m=>m.Name == "AddScrapDetail").Name,new List<ActionType>{ActionType.CreateScrap}),
            new MethodAction(typeof(IScrapFacadeService).Name,typeof(IScrapFacadeService).GetMethods().First(m=>m.Name == "Update").Name,new List<ActionType>{ActionType.EditScrap}),
            new MethodAction(typeof(IScrapFacadeService).Name,typeof(IScrapFacadeService).GetMethods().First(m=>m.Name == "UpdateScrapDetail").Name,new List<ActionType>{ActionType.EditScrap}),
            new MethodAction(typeof(IScrapFacadeService).Name,typeof(IScrapFacadeService).GetMethods().First(m=>m.Name == "Delete").Name,new List<ActionType>{ActionType.RemoveScrap}),
            new MethodAction(typeof(IScrapFacadeService).Name,typeof(IScrapFacadeService).GetMethods().First(m=>m.Name == "DeleteScrapDetail").Name,new List<ActionType>{ActionType.RemoveScrap}),

            new MethodAction(typeof(IOrderFacadeService).Name,typeof(IOrderFacadeService).GetMethods().First(m=>m.Name == "GetById").Name,new List<ActionType>{ActionType.ViewOrders}),
            new MethodAction(typeof(IOrderFacadeService).Name,typeof(IOrderFacadeService).GetMethods().First(m=>m.Name == "GetByFilter").Name,new List<ActionType>{ActionType.ViewOrders}),
            new MethodAction(typeof(IOrderFacadeService).Name,typeof(IOrderFacadeService).GetMethods().First(m=>m.Name == "GetAll").Name,new List<ActionType>{ActionType.ViewOrders}),
            new MethodAction(typeof(IOrderFacadeService).Name,typeof(IOrderFacadeService).GetMethods().First(m=>m.Name == "GetOrderItemById").Name,new List<ActionType>{ActionType.ViewOrders}),
            new MethodAction(typeof(IOrderFacadeService).Name,typeof(IOrderFacadeService).GetMethods().First(m=>m.Name == "GetGoodMainUnit").Name,new List<ActionType>{ActionType.ViewOrders}),
            new MethodAction(typeof(IOrderFacadeService).Name,typeof(IOrderFacadeService).GetMethods().First(m=>m.Name == "Add").Name,new List<ActionType>{ActionType.CreateOrder}),
            new MethodAction(typeof(IOrderFacadeService).Name,typeof(IOrderFacadeService).GetMethods().First(m=>m.Name == "AddItem").Name,new List<ActionType>{ActionType.CreateOrder}),
            new MethodAction(typeof(IOrderFacadeService).Name,typeof(IOrderFacadeService).GetMethods().First(m=>m.Name == "Update").Name,new List<ActionType>{ActionType.EditOrder}),
            new MethodAction(typeof(IOrderFacadeService).Name,typeof(IOrderFacadeService).GetMethods().First(m=>m.Name == "UpdateItem").Name,new List<ActionType>{ActionType.EditOrder}),
            new MethodAction(typeof(IOrderFacadeService).Name,typeof(IOrderFacadeService).GetMethods().First(m=>m.Name == "Delete").Name,new List<ActionType>{ActionType.RemoveOrder}),
            new MethodAction(typeof(IOrderFacadeService).Name,typeof(IOrderFacadeService).GetMethods().First(m=>m.Name == "DeleteById").Name,new List<ActionType>{ActionType.RemoveOrder}),
            new MethodAction(typeof(IOrderFacadeService).Name,typeof(IOrderFacadeService).GetMethods().First(m=>m.Name == "DeleteItem").Name,new List<ActionType>{ActionType.RemoveOrder}),

            new MethodAction(typeof(IGoodFacadeService).Name,typeof(IGoodFacadeService).GetMethods().First(m=>m.Name == "GetAll").Name,new List<ActionType>{ActionType.QueryGoods}),

            
            
            new MethodAction(typeof(IInvoiceFacadeService).Name,typeof(IInvoiceFacadeService).GetMethods().First(m=>m.Name == "GetByFilter").Name,new List<ActionType>{ActionType.ViewInvoices}),
            new MethodAction(typeof(IInvoiceFacadeService).Name,typeof(IInvoiceFacadeService).GetMethods().First(m=>m.Name == "GetById").Name,new List<ActionType>{ActionType.ViewInvoices}),
            new MethodAction(typeof(IInvoiceFacadeService).Name,typeof(IInvoiceFacadeService).GetMethods().First(m=>m.Name == "GetAll").Name,new List<ActionType>{ActionType.ViewInvoices}),
            new MethodAction(typeof(IInvoiceFacadeService).Name,typeof(IInvoiceFacadeService).GetMethods().First(m=>m.Name == "GetAllEffectiveFactors").Name,new List<ActionType>{ActionType.ViewInvoices}),
            
            new MethodAction(typeof(IInvoiceFacadeService).Name,typeof(IInvoiceFacadeService).GetMethods().First(m=>m.Name == "CalculateAdditionalPrice").Name,new List<ActionType>{ActionType.RegisterInvoice}),
            new MethodAction(typeof(IInvoiceFacadeService).Name,typeof(IInvoiceFacadeService).GetMethods().First(m=>m.Name == "GenerateInvoiceItemForOrders").Name,new List<ActionType>{ActionType.RegisterInvoice}),
            new MethodAction(typeof(IInvoiceFacadeService).Name,typeof(IInvoiceFacadeService).GetMethods().First(m=>m.Name == "Add").Name,new List<ActionType>{ActionType.RegisterInvoice}),
            new MethodAction(typeof(IInvoiceFacadeService).Name,typeof(IInvoiceFacadeService).GetMethods().First(m=>m.Name == "Update").Name,new List<ActionType>{ActionType.EditInvoice}),
            new MethodAction(typeof(IInvoiceFacadeService).Name,typeof(IInvoiceFacadeService).GetMethods().First(m=>m.Name == "Delete").Name,new List<ActionType>{ActionType.RemoveInvoice}),
            new MethodAction(typeof(IInvoiceFacadeService).Name,typeof(IInvoiceFacadeService).GetMethods().First(m=>m.Name == "DeleteById").Name,new List<ActionType>{ActionType.RemoveInvoice}),

            new MethodAction(typeof(IGoodFacadeService).Name,typeof(IGoodFacadeService).GetMethod("GetAll").Name,new List<ActionType>{ActionType.QueryGoods}),


            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethods().First(m=>m.Name == "GetById").Name,new List<ActionType>{ActionType.ViewOffhires}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethods().First(m=>m.Name == "GetPagedData").Name,new List<ActionType>{ActionType.ViewOffhires}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethods().First(m=>m.Name == "GetPagedDataByFilter").Name,new List<ActionType>{ActionType.ViewOffhires}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethods().First(m=>m.Name == "GetPagedOffhireDetailData").Name,new List<ActionType>{ActionType.ViewOffhires}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethods().First(m=>m.Name == "GetOffhireDetail").Name,new List<ActionType>{ActionType.ViewOffhires}),
            
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethods().First(m=>m.Name == "GetOffhireManagementSystemPagedDataByFilter").Name,new List<ActionType>{ActionType.ImportOffhire}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethods().First(m=>m.Name == "PrepareOffhireData").Name,new List<ActionType>{ActionType.ImportOffhire}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethods().First(m=>m.Name == "GetOffhirePricingValueInVoucherCurrency").Name,new List<ActionType>{ActionType.ImportOffhire}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethods().First(m=>m.Name == "GetOffhirePricingValueInMainCurrency").Name,new List<ActionType>{ActionType.ImportOffhire}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethods().First(m=>m.Name == "GetOffhirePricingValuesInVoucherCurrency").Name,new List<ActionType>{ActionType.ImportOffhire}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethods().First(m=>m.Name == "GetOffhirePricingValuesInMainCurrency").Name,new List<ActionType>{ActionType.ImportOffhire}),
            
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethods().First(m=>m.Name == "Add").Name,new List<ActionType>{ActionType.ImportOffhire}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethods().First(m=>m.Name == "Update").Name,new List<ActionType>{ActionType.EditOffhire}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethods().First(m=>m.Name == "Delete").Name,new List<ActionType>{ActionType.RemoveOffhire}),


            new MethodAction(typeof(IVoyageFacadeService).Name,typeof(IVoyageFacadeService).GetMethods().First(m=>m.Name == "GetAll").Name,new List<ActionType>{ActionType.QueryVoyages}),
            new MethodAction(typeof(IVoyageFacadeService).Name,typeof(IVoyageFacadeService).GetMethods().First(m=>m.Name == "GetByFilter").Name,new List<ActionType>{ActionType.QueryVoyages}),
            new MethodAction(typeof(IVoyageFacadeService).Name,typeof(IVoyageFacadeService).GetMethods().First(m=>m.Name == "GetById").Name,new List<ActionType>{ActionType.QueryVoyages}),
            new MethodAction(typeof(IVoyageFacadeService).Name,typeof(IVoyageFacadeService).GetMethods().First(m=>m.Name == "GetPagedData").Name,new List<ActionType>{ActionType.QueryVoyages}),
            new MethodAction(typeof(IVoyageFacadeService).Name,typeof(IVoyageFacadeService).GetMethods().First(m=>m.Name == "GetPagedDataByFilter").Name,new List<ActionType>{ActionType.QueryVoyages}),
            new MethodAction(typeof(IVoyageFacadeService).Name,typeof(IVoyageFacadeService).GetMethods().First(m=>m.Name == "GetChenageHistory").Name,new List<ActionType>{ActionType.QueryVoyages}),
            new MethodAction(typeof(IVoyageFacadeService).Name,typeof(IVoyageFacadeService).GetMethods().First(m=>m.Name == "UpdateVoyagesFromRotationData").Name,new List<ActionType>{ActionType.UpdateVoyagesFromRotationData}),

            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethod("GetById").Name,new List<ActionType>{ActionType.ViewOffhires}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethod("GetPagedData").Name,new List<ActionType>{ActionType.ViewOffhires}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethod("GetPagedDataByFilter").Name,new List<ActionType>{ActionType.ViewOffhires}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethod("GetPagedOffhireDetailData").Name,new List<ActionType>{ActionType.ViewOffhires}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethod("GetOffhireDetail").Name,new List<ActionType>{ActionType.ViewOffhires}),

            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethod("GetOffhireManagementSystemPagedDataByFilter").Name,new List<ActionType>{ActionType.ImportOffhire}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethod("PrepareOffhireData").Name,new List<ActionType>{ActionType.ImportOffhire}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethod("GetOffhirePricingValueInVoucherCurrency").Name,new List<ActionType>{ActionType.ImportOffhire}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethod("GetOffhirePricingValueInMainCurrency").Name,new List<ActionType>{ActionType.ImportOffhire}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethod("GetOffhirePricingValuesInVoucherCurrency").Name,new List<ActionType>{ActionType.ImportOffhire}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethod("GetOffhirePricingValuesInMainCurrency").Name,new List<ActionType>{ActionType.ImportOffhire}),

            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethod("Add").Name,new List<ActionType>{ActionType.ImportOffhire}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethod("Update").Name,new List<ActionType>{ActionType.EditOffhire}),
            new MethodAction(typeof(IOffhireFacadeService).Name,typeof(IOffhireFacadeService).GetMethod("Delete").Name,new List<ActionType>{ActionType.RemoveOffhire}),

            new MethodAction(typeof(ICompanyFacadeService).Name,typeof(ICompanyFacadeService).GetMethods().First(m=>m.Name == "GetAll").Name,new List<ActionType>{ActionType.QueryCompanies}),
            new MethodAction(typeof(ICompanyFacadeService).Name,typeof(ICompanyFacadeService).GetMethods().First(m=>m.Name == "GetOwnedVessels").Name,new List<ActionType>{ActionType.QueryCompanies}),
            new MethodAction(typeof(ICompanyFacadeService).Name,typeof(ICompanyFacadeService).GetMethods().First(m=>m.Name == "GetByUserId").Name,new List<ActionType>{ActionType.QueryCompanies}),
            new MethodAction(typeof(ICompanyFacadeService).Name,typeof(ICompanyFacadeService).GetMethods().First(m=>m.Name == "GetByCurrentUserId").Name,new List<ActionType>{ActionType.QueryCompanies}),

            new MethodAction(typeof(IVoyageFacadeService).Name,typeof(IVoyageFacadeService).GetMethod("GetAll").Name,new List<ActionType>{ActionType.QueryVoyages}),
            new MethodAction(typeof(IVoyageFacadeService).Name,typeof(IVoyageFacadeService).GetMethod("GetByFilter").Name,new List<ActionType>{ActionType.QueryVoyages}),
            new MethodAction(typeof(IVoyageFacadeService).Name,typeof(IVoyageFacadeService).GetMethod("GetById").Name,new List<ActionType>{ActionType.QueryVoyages}),
            new MethodAction(typeof(IVoyageFacadeService).Name,typeof(IVoyageFacadeService).GetMethod("GetPagedData").Name,new List<ActionType>{ActionType.QueryVoyages}),
            new MethodAction(typeof(IVoyageFacadeService).Name,typeof(IVoyageFacadeService).GetMethod("GetPagedDataByFilter").Name,new List<ActionType>{ActionType.QueryVoyages}),
            new MethodAction(typeof(IVoyageFacadeService).Name,typeof(IVoyageFacadeService).GetMethod("GetChenageHistory").Name,new List<ActionType>{ActionType.QueryVoyages}),
            //new MethodAction(typeof(IVoyageFacadeService).Name,typeof(IVoyageFacadeService).GetMethod("FindVoyage").Name,new List<ActionType>{ActionType.QueryVoyages}),

            new MethodAction(typeof(IAccountFacadeService).Name,typeof(IAccountFacadeService).GetMethods().First(m=>m.Name == "GetAllByFilter").Name, new List<ActionType>(){ActionType.QueryFinancialAccounts}),
            new MethodAction(typeof(IAccountFacadeService).Name,typeof(IAccountFacadeService).GetMethods().First(m=>m.Name == "Add").Name, new List<ActionType>(){ActionType.AddFinancialAccount}),
            new MethodAction(typeof(IOriginalAccountFacadeService).Name,typeof(IOriginalAccountFacadeService).GetMethods().First(m=>m.Name == "GetAllByFilter").Name, new List<ActionType>(){ActionType.QueryOriginalFinancialAccounts}),

            new MethodAction(typeof(IVesselInCompanyFacadeService).Name,typeof(IVesselInCompanyFacadeService).GetMethods().First(m=>m.Name == "GetAll").Name,new List<ActionType>{ActionType.QueryVesselsInCompany}),
            new MethodAction(typeof(IVesselInCompanyFacadeService).Name,typeof(IVesselInCompanyFacadeService).GetMethods().First(m=>m.Name == "GetCompanyVessels").Name,new List<ActionType>{ActionType.QueryVesselsInCompany}),
            new MethodAction(typeof(IVesselInCompanyFacadeService).Name,typeof(IVesselInCompanyFacadeService).GetMethods().First(m=>m.Name == "GetVesselInCompanies").Name,new List<ActionType>{ActionType.QueryVesselsInCompany}),
            new MethodAction(typeof(IVesselInCompanyFacadeService).Name,typeof(IVesselInCompanyFacadeService).GetMethods().First(m=>m.Name == "GetVesselActivationInfo").Name,new List<ActionType>{ActionType.QueryVesselsInCompany}),
            new MethodAction(typeof(IVesselInCompanyFacadeService).Name,typeof(IVesselInCompanyFacadeService).GetMethods().First(m=>m.Name == "GetOwnedVessels").Name,new List<ActionType>{ActionType.QueryVesselsInCompany}),
            new MethodAction(typeof(IVesselInCompanyFacadeService).Name,typeof(IVesselInCompanyFacadeService).GetMethods().First(m=>m.Name == "GetById").Name,new List<ActionType>{ActionType.QueryVesselsInCompany}),
            new MethodAction(typeof(IVesselInCompanyFacadeService).Name,typeof(IVesselInCompanyFacadeService).GetMethods().First(m=>m.Name == "GetOwnedOrCharterInVessels").Name,new List<ActionType>{ActionType.QueryVesselsInCompany}),
            new MethodAction(typeof(IVesselInCompanyFacadeService).Name,typeof(IVesselInCompanyFacadeService).GetMethods().First(m=>m.Name == "ActivateWarehouseIncludingRecieptsOperation").Name,new List<ActionType>{ActionType.ActivateVesselInCompany}),
            
            
            new MethodAction(typeof(IVesselFacadeService).Name,typeof(IVesselFacadeService).GetMethods().First(m=>m.Name == "Get").Name,new List<ActionType>{ActionType.QueryVessels}),
            new MethodAction(typeof(IVesselFacadeService).Name,typeof(IVesselFacadeService).GetMethods().First(m=>m.Name == "GetPagedData").Name,new List<ActionType>{ActionType.QueryVessels}),
            new MethodAction(typeof(IVesselFacadeService).Name,typeof(IVesselFacadeService).GetMethods().First(m=>m.Name == "GetPagedDataByFilter").Name,new List<ActionType>{ActionType.QueryVessels}),
            new MethodAction(typeof(IVesselFacadeService).Name,typeof(IVesselFacadeService).GetMethods().First(m=>m.Name == "Add").Name,new List<ActionType>{ActionType.CreateVessel}),

            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "UpdateUserAccess").Name,new List<ActionType>{ActionType.SecurityUser}),
            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "GetUserByUserName").Name,new List<ActionType>{ActionType.SecurityUser}),
            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "GetAllActionTypes").Name,new List<ActionType>{ActionType.SecurityUser}),
            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "Add").Name,new List<ActionType>{ActionType.SecurityUser}),
            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "Update").Name,new List<ActionType>{ActionType.SecurityUser}),
            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "DeleteById").Name,new List<ActionType>{ActionType.SecurityUser}),
            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "GetUserWithCompany").Name,new List<ActionType>{ActionType.SecurityUser}),
            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "GetAllUsers").Name,new List<ActionType>{ActionType.SecurityUser}),
            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "ChangePassWord").Name,new List<ActionType>{ActionType.ChangePassWord}),
            
            
            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "GetGroupActionType").Name,new List<ActionType>{ActionType.SecurityGroup}),
            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "GetUserGroupByName").Name,new List<ActionType>{ActionType.SecurityGroup}),
            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "AddUserGroup").Name,new List<ActionType>{ActionType.SecurityGroup}),
            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "UpdateUserGroup").Name,new List<ActionType>{ActionType.SecurityGroup}),
            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "DeleteUserGroup").Name,new List<ActionType>{ActionType.SecurityGroup}),
            new MethodAction(typeof(IUserFacadeService).Name,typeof(IUserFacadeService).GetMethods().First(m=>m.Name == "GetAllUserGroups").Name,new List<ActionType>{ActionType.SecurityGroup}),




        };

        private readonly List<MethodAction> MapTableWorkFlow = new List<MethodAction>
        {
           //  #region chrter in
           //  new MethodAction(ActionEntityTypeEnum.CharterIn.ToString(),DecisionTypeEnum.Approved.ToString(),new List<ActionType>(){ActionType.ManageCharterInSubmition}),
           //  new MethodAction(ActionEntityTypeEnum.CharterIn.ToString(),DecisionTypeEnum.Canceled.ToString(),new List<ActionType>(){ActionType.Manage}),
           //  new MethodAction(ActionEntityTypeEnum.CharterIn.ToString(),DecisionTypeEnum.Rejected.ToString(),new List<ActionType>(){ActionType.RejectCharterIn}),
             
           // #endregion
      
           //#region chrter Out
           //  new MethodAction(ActionEntityTypeEnum.CharterOut.ToString(),DecisionTypeEnum.Approved.ToString(),new List<ActionType>(){ActionType.ApproveCharterOut}),
           //  new MethodAction(ActionEntityTypeEnum.CharterOut.ToString(),DecisionTypeEnum.Canceled.ToString(),new List<ActionType>(){ActionType.FinalApproveCharterOut}),
           //  new MethodAction(ActionEntityTypeEnum.CharterOut.ToString(),DecisionTypeEnum.Rejected.ToString(),new List<ActionType>(){ActionType.RejectCharterOut}),
             
           // #endregion
      
       

        };



        public List<ActionType> MapWF(ActionEntityTypeEnum workflowEntitie, DecisionTypeEnum decisionTypeEnum)
        {

            var mapRow = MapTableWorkFlow.Where(m => m.ClassName == workflowEntitie.ToString() && m.MethodName == decisionTypeEnum.ToString());
            if (!mapRow.Any())
                return new List<ActionType>();
            return mapRow.FirstOrDefault().Actions;

        }

        public List<ActionType> Map(string className, string methodName)
        {

            var mapRow = mapTable.Where(m => m.ClassName == className && m.MethodName == methodName);
            if (!mapRow.Any())
                return new List<ActionType>();
            return mapRow.FirstOrDefault().Actions;

        }


    }

}
