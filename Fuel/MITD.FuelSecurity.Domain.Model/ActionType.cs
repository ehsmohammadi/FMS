using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MITD.FuelSecurity.Domain.Model
{


    public class ActionType
    {
        private enum ActionTypes
        {
            QueryCharterIn = 1,
            AddCharterIn,
            EditCharterIn,
            DeleteCharterIn,
            AddCharterInItem,
            EditCharterInItem,
            DeleteCharterInItem,
            ManageCharterInApprovement,
            ManageCharterInSubmition,
            RejectCharterIn_OBSOLETE,
            CancelCharterIn,
            QueryCharterOut,
            AddCharterOut,
            EditCharterOut,
            DeleteCharterOut,
            AddCharterOutItem,
            EditCharterOutItem,
            DeleteCharterOutItem,
            ManageCharterOutApprovement,
            ManageCharterOutSubmition,
            RejectCharterOut_OBSOLETE,
            CancelCharterOut,
            ViewFuelReports,
            EditFuelReport,
            ImportFuelReports,
            ManageFuelReportApprovement,
            ManageFuelReportSubmittion,
            ViewScraps,
            CreateScrap,
            EditScrap,
            RemoveScrap,
            ManageScrapApprovement,
            ManageScrapSubmittion,
            ViewOrders,
            CreateOrder,
            EditOrder,
            RemoveOrder,
            ManageOrderApprovement,
            ManageOrderSubmittion,
            CancelOrder,
            ViewInvoices,
            RegisterInvoice,
            EditInvoice,
            RemoveInvoice,
            ManageInvoiceApprovement,
            ManageInvoiceSubmittion,
            ManageEffectiveFactors,
            ViewOffhires,
            ImportOffhire,
            EditOffhire,
            RemoveOffhire,
            ManageOffhireApprovement,
            ManageOffhireSubmittion,
            AddUser,
            ModifyUser,
            DeleteUser,
            QueryGoods,
            QueryVoyages,
            ViewVoyages,
            QueryCompanies,
            QueryVoucher,
            QueryVoucherSeting,
            AddVoucherSeting,
            UpdateVoucherSeting,
            AddVoucherSetingDetail,
            UpdateVoucherSetingDetail,
            QueryFinancialAccounts,
            AddFinancialAccount,
            QueryOriginalFinancialAccounts,
            QueryVessels,
            CreateVessel,
            QueryVesselsInCompany,
            ActivateVesselInCompany,
            CancelInvoice,
            CloseOrder,
            SecurityGroup,
            SecurityUser,
            ManageFuelReportFinancialSubmition,
            ChangePassWord,
            EditFinancialFuelReport,
            ManageFuelReportFinancialReject,
            RemoveFuelReport,
            UpdateVoyagesFromRotationData,
            ManageFuelReportCancel,
            RejectSubmittedOrder,
            ResubmitRejectedOrder,
            RejectSubmittedInvoice,
            ResubmitRejectedInvoice,
        }


        #region Voucher
        public static readonly ActionType QueryVoucher = new ActionType((int)ActionTypes.QueryVoucher, "QueryVoucher", "QueryVoucher");

        #endregion

        #region VoucherSeting

        public static readonly ActionType QueryVoucherSeting = new ActionType((int)ActionTypes.QueryVoucherSeting, "QueryVoucherSeting", "QueryVoucherSeting");
        public static readonly ActionType AddVoucherSeting = new ActionType((int)ActionTypes.AddVoucherSeting, "AddVoucherSeting", "AddVoucherSeting");
        public static readonly ActionType UpdateVoucherSeting = new ActionType((int)ActionTypes.UpdateVoucherSeting, "UpdateVoucherSeting", "UpdateVoucherSeting");
        public static readonly ActionType AddVoucherSetingDetail = new ActionType((int)ActionTypes.AddVoucherSetingDetail, "AddVoucherSetingDetail", "AddVoucherSetingDetail");
        public static readonly ActionType UpdateVoucherSetingDetail = new ActionType((int)ActionTypes.UpdateVoucherSetingDetail, "UpdateVoucherSetingDetail", "UpdateVoucherSetingDetail");

        #endregion


        #region Charter In Actions
        public static readonly ActionType QueryCharterIn = new ActionType((int)ActionTypes.QueryCharterIn, "QueryCharterIn", "QueryCharterIn");

        public static readonly ActionType AddCharterIn = new ActionType((int)ActionTypes.AddCharterIn, "AddCharterIn", "افزودن چارتر In");
        public static readonly ActionType EditCharterIn = new ActionType((int)ActionTypes.EditCharterIn, "EditCharterIn", "ویرایش چارتر In");
        public static readonly ActionType DeleteCharterIn = new ActionType((int)ActionTypes.DeleteCharterIn, "DeleteCharterIn", "حذف چارتر In");
        public static readonly ActionType AddCharterInItem = new ActionType((int)ActionTypes.AddCharterInItem, "AddCharterInItem", "افزودن ردیف چارتر In");
        public static readonly ActionType EditCharterInItem = new ActionType((int)ActionTypes.EditCharterInItem, "EditCharterInItem", "ویرایش ردیف چارتر In");
        public static readonly ActionType DeleteCharterInItem = new ActionType((int)ActionTypes.DeleteCharterInItem, "DeleteCharterInItem", "حذف ردیف چارتر In");

        public static readonly ActionType ManageCharterInApprovement = new ActionType((int)ActionTypes.ManageCharterInApprovement, "ApproveCharterIn", "انجام تأییدات میانی چارتر In");
        public static readonly ActionType ManageCharterInSubmition = new ActionType((int)ActionTypes.ManageCharterInSubmition, "FinalApproveCharterIn", "انجام تأیید نهایی چارتر In");
        public static readonly ActionType RejectCharterIn_OBSOLETE = new ActionType((int)ActionTypes.RejectCharterIn_OBSOLETE, "RejectCharterIn", "RejectCharterIn");
        public static readonly ActionType CancelCharterIn = new ActionType((int)ActionTypes.CancelCharterIn, "CancelCharterIn", "لغو چارتر In");



        public static readonly ActionType QueryCharterOut = new ActionType((int)ActionTypes.QueryCharterOut, "QueryCharterOut", "QueryCharterOut");
        public static readonly ActionType AddCharterOut = new ActionType((int)ActionTypes.AddCharterOut, "AddCharterOut", "افزودن چارتر Out");
        public static readonly ActionType EditCharterOut = new ActionType((int)ActionTypes.EditCharterOut, "EditCharterOut", "ویرایش چارتر Out");
        public static readonly ActionType DeleteCharterOut = new ActionType((int)ActionTypes.DeleteCharterOut, "DeleteCharterOut", "حذف چارتر Out");
        public static readonly ActionType AddCharterOutItem = new ActionType((int)ActionTypes.AddCharterOutItem, "AddCharterOutItem", "افزودن ردیف چارتر Out");
        public static readonly ActionType EditCharterOutItem = new ActionType((int)ActionTypes.EditCharterOutItem, "EditCharterOutItem", "ویرایش ردیف چارتر Out");
        public static readonly ActionType DeleteCharterOutItem = new ActionType((int)ActionTypes.DeleteCharterOutItem, "DeleteCharterOutItem", "حذف ردیف چارتر Out");


        public static readonly ActionType ManageCharterOutApprovement = new ActionType((int)ActionTypes.ManageCharterOutApprovement, "ManageCharterOutApprovement", "انجام تأییدات میانی چارتر Out");
        public static readonly ActionType ManageCharterOutSubmition = new ActionType((int)ActionTypes.ManageCharterOutSubmition, "ManageCharterOutSubmition", "انجام تأیید نهایی چارتر Out");
        public static readonly ActionType RejectCharterOut_OBSOLETE = new ActionType((int)ActionTypes.RejectCharterOut_OBSOLETE, "RejectCharterOut", "RejectCharterOut");
        public static readonly ActionType CancelCharterOut = new ActionType((int)ActionTypes.CancelCharterOut, "CancelCharterOut", "لغو چارتر Out");
        #endregion

        #region FuelReport Actions

        public static readonly ActionType ViewFuelReports = new ActionType((int)ActionTypes.ViewFuelReports, "ViewFuelReports", "مشاهده لیست گزارشات سوخت");
        public static readonly ActionType EditFuelReport = new ActionType((int)ActionTypes.EditFuelReport, "EditFuelReport", "ویرایش گزارش سوخت");
        public static readonly ActionType ImportFuelReports = new ActionType((int)ActionTypes.ImportFuelReports, "ImportFuelReports", "ثبت دستی گزارشات سوخت");
        public static readonly ActionType ManageFuelReportApprovement = new ActionType((int)ActionTypes.ManageFuelReportApprovement, "ManageFuelReportApprovement", "انجام تأییدات میانی گزارش سوخت");
        public static readonly ActionType ManageFuelReportSubmittion = new ActionType((int)ActionTypes.ManageFuelReportSubmittion, "ManageFuelReportSubmittion", "انجام تأیید نهایی گزارش سوخت");
        public static readonly ActionType ManageFuelReportFinancialSubmition = new ActionType((int)ActionTypes.ManageFuelReportFinancialSubmition, "ManageFuelReportFinancialSubmition", "انجام تأیید نهایی مالی گزارش سوخت");
        public static readonly ActionType EditFinancialFuelReport = new ActionType((int)ActionTypes.EditFinancialFuelReport, "EditFinancialFuelReport", "ویرایش مالی گزارش سوخت");
        public static readonly ActionType ManageFuelReportFinancialReject = new ActionType((int)ActionTypes.ManageFuelReportFinancialReject, "ManageFuelReportFinancialReject", "انجام برگشت از تأیید نهایی مالی گزارش سوخت");
        public static readonly ActionType RemoveFuelReport = new ActionType((int)ActionTypes.RemoveFuelReport, "RemoveFuelReport", "حذف گزارش سوخت");
        public static readonly ActionType ManageFuelReportCancel = new ActionType((int)ActionTypes.ManageFuelReportCancel, "ManageFuelReportCancel", "کنسل نمودن گزارش سوخت");

        #endregion

        #region Scrap Actions

        public static readonly ActionType ViewScraps = new ActionType((int)ActionTypes.ViewScraps, "ViewScraps", "مشاهده لیست Scrap");
        public static readonly ActionType CreateScrap = new ActionType((int)ActionTypes.CreateScrap, "CreateScrap", "ایجاد Scrap");
        public static readonly ActionType EditScrap = new ActionType((int)ActionTypes.EditScrap, "EditScrap", "ویرایش Scrap");
        public static readonly ActionType RemoveScrap = new ActionType((int)ActionTypes.RemoveScrap, "RemoveScrap", "حذف Scrap");
        public static readonly ActionType ManageScrapApprovement = new ActionType((int)ActionTypes.ManageScrapApprovement, "ManageScrapApprovement", "انجام تأییدات میانی Scrap");
        public static readonly ActionType ManageScrapSubmittion = new ActionType((int)ActionTypes.ManageScrapSubmittion, "ManageScrapSubmittion", "انجام تأیید نهایی Scrap");

        #endregion

        #region Order Actions

        public static readonly ActionType ViewOrders = new ActionType((int)ActionTypes.ViewOrders, "ViewOrders", "مشاهده لیست سفارشات");
        public static readonly ActionType CreateOrder = new ActionType((int)ActionTypes.CreateOrder, "CreateOrder", "ایجاد سفارش");
        public static readonly ActionType EditOrder = new ActionType((int)ActionTypes.EditOrder, "EditOrder", "ویرایش سفارش");
        public static readonly ActionType RemoveOrder = new ActionType((int)ActionTypes.RemoveOrder, "RemoveOrder", "حذف سفارش");
        public static readonly ActionType ManageOrderApprovement = new ActionType((int)ActionTypes.ManageOrderApprovement, "ManageOrderApprovement", "انجام تأیید میانی سفارش");
        public static readonly ActionType ManageOrderSubmittion = new ActionType((int)ActionTypes.ManageOrderSubmittion, "ManageOrderSubmittion", "انجام تأیید نهایی سفارش");
        public static readonly ActionType CancelOrder = new ActionType((int)ActionTypes.CancelOrder, "CancelOrder", "ابطال سفارش");
        public static readonly ActionType CloseOrder = new ActionType((int)ActionTypes.CloseOrder, "CloseOrder", "بستن سفارش");
        public static readonly ActionType RejectSubmittedOrder = new ActionType((int)ActionTypes.RejectSubmittedOrder, ActionTypes.RejectSubmittedOrder.ToString(), "برگشت سفارش تأیید نهایی شده");
        public static readonly ActionType ResubmitRejectedOrder = new ActionType((int)ActionTypes.ResubmitRejectedOrder, ActionTypes.ResubmitRejectedOrder.ToString(), "تأیید نهایی مجـدد سفارش برگشت خورده");


        #endregion

        #region Offhire Actions

        public static readonly ActionType ViewOffhires = new ActionType((int)ActionTypes.ViewOffhires, "ViewOffhires", "نمایش آف هایر");
        public static readonly ActionType ImportOffhire= new ActionType((int)ActionTypes.ImportOffhire, "ImportOffhire", "ثبت آف هایر");
        public static readonly ActionType EditOffhire= new ActionType((int)ActionTypes.EditOffhire, "EditOffhire", "اصلاح آف هایر");
        public static readonly ActionType RemoveOffhire= new ActionType((int)ActionTypes.RemoveOffhire, "RemoveOffhire", "حذف آف هایر");
        public static readonly ActionType ManageOffhireApprovement = new ActionType((int)ActionTypes.ManageOffhireApprovement, "ManageOffhireApprovement", "مدیریت تأیید آف هایر");
        public static readonly ActionType ManageOffhireSubmittion = new ActionType((int)ActionTypes.ManageOffhireSubmittion, "ManageOffhireSubmittion", "مدیریت ارسال آف هایر");

        #endregion

        #region Invoice & Effective Factors Actions

        public static readonly ActionType ViewInvoices = new ActionType((int)ActionTypes.ViewInvoices, "ViewInvoices", "نمایش صورتحساب");
        public static readonly ActionType RegisterInvoice = new ActionType((int)ActionTypes.RegisterInvoice, "RegisterInvoice", "ثبت صورتحساب");
        public static readonly ActionType EditInvoice = new ActionType((int)ActionTypes.EditInvoice, "EditInvoice", "اصلاح صورتحساب");
        public static readonly ActionType RemoveInvoice = new ActionType((int)ActionTypes.RemoveInvoice, "RemoveInvoice", "حذف صورتحساب");
        public static readonly ActionType ManageInvoiceApprovement = new ActionType((int)ActionTypes.ManageInvoiceApprovement, "ManageInvoiceApprovement", "مدیریت تأیید صورتحساب");
        public static readonly ActionType ManageInvoiceSubmittion = new ActionType((int)ActionTypes.ManageInvoiceSubmittion, "ManageInvoiceSubmittion", "مدیریت ارسال صورتحساب");
        public static readonly ActionType ManageEffectiveFactors = new ActionType((int)ActionTypes.ManageEffectiveFactors, "ManageEffectiveFactors", "مدیریت عوامل تاثیر گزار");
        public static readonly ActionType CancelInvoice = new ActionType((int)ActionTypes.CancelInvoice, "CancelInvoice", "لغو صورت حساب");

        public static readonly ActionType RejectSubmittedInvoice = new ActionType((int)ActionTypes.RejectSubmittedInvoice, ActionTypes.RejectSubmittedInvoice.ToString(), "برگشت صورتحساب تأیید نهایی شده");
        public static readonly ActionType ResubmitRejectedInvoice = new ActionType((int)ActionTypes.ResubmitRejectedInvoice, ActionTypes.ResubmitRejectedInvoice.ToString(), "تأیید نهایی مجـدد صورتحساب برگشت خورده");

        #endregion

        #region Security
        public static readonly ActionType SecurityGroup = new ActionType((int)ActionTypes.SecurityGroup, "SecurityGroup", "گروه کاربران");
        public static readonly ActionType SecurityUser = new ActionType((int)ActionTypes.SecurityUser, "SecurityUser", "تنظیمات کاربران");
        public static readonly ActionType ChangePassWord = new ActionType((int)ActionTypes.ChangePassWord, "ChangePassWord", "تغییر رمز عبور");
        #endregion

        public static readonly ActionType ViewVoyages = new ActionType((int)ActionTypes.ViewVoyages, "ViewVoyages", "نمایش سفرها");

        public static readonly ActionType QueryGoods = new ActionType((int)ActionTypes.QueryGoods, "QueryGoods", "لیست کالاها");
        public static readonly ActionType QueryVoyages = new ActionType((int)ActionTypes.QueryVoyages, "QueryVoyages", "لیست سفرها");
        public static readonly ActionType UpdateVoyagesFromRotationData = new ActionType((int)ActionTypes.UpdateVoyagesFromRotationData, "UpdateVoyagesFromRotationData", "به روز رسانی سفرها");
        public static readonly ActionType QueryCompanies = new ActionType((int)ActionTypes.QueryCompanies, "QueryCompanies", "لیست شرکتها");

        public static readonly ActionType QueryFinancialAccounts = new ActionType((int)ActionTypes.QueryFinancialAccounts, "QueryFinancialAccounts", "لیست حسابهای معین");
        public static readonly ActionType AddFinancialAccount = new ActionType((int)ActionTypes.AddFinancialAccount, "AddFinancialAccount", "افزودن حساب معین");
        public static readonly ActionType QueryOriginalFinancialAccounts = new ActionType((int)ActionTypes.QueryOriginalFinancialAccounts, "QueryOriginalFinancialAccounts", "لیست حسابهای معین سیستم مالی");


        public static readonly ActionType QueryVessels = new ActionType((int)ActionTypes.QueryVessels, "QueryVessels", "لیست شناورها");
        public static readonly ActionType CreateVessel = new ActionType((int)ActionTypes.CreateVessel, "CreateVessel", "ایجاد شناور");
        public static readonly ActionType QueryVesselsInCompany = new ActionType((int)ActionTypes.QueryVesselsInCompany, "QueryVesselsInCompany", "لیست شناورهای شرکت");
        public static readonly ActionType ActivateVesselInCompany = new ActionType((int)ActionTypes.ActivateVesselInCompany, "ActivateVesselInCompany", "فعالسازی شناور در شرکت");
        
       

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ActionType()
        {

        }

        public ActionType(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;

        }

        public static IEnumerable<ActionType> GetAllActions()
        {
            var fields =
                (typeof(ActionType)).GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
            foreach (var fieldInfo in fields)
            {
                ActionType actionType = fieldInfo.GetValue((object)null) as ActionType;
                if (actionType != null)
                    yield return actionType;
            }
        }

        public static ActionType FromValue(int actionTypeId)
        {
            var actionType = GetAllActions().FirstOrDefault(item => item.Id == actionTypeId);

            if (actionType == null)
                throw new Exception();

            return actionType;

        }

        public static IEnumerable<ActionType> GetActionType(string name)
        {
            var fields =
               (typeof(ActionType)).GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
            foreach (var fieldInfo in fields)
            {
                ActionType actionType = fieldInfo.GetValue((object)null) as ActionType;
                if (actionType.Name ==name)
                    yield return actionType;
            }
        }

    }
}





