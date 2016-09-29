using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.FuelSecurity.Domain.Model
{
    public class FinancialUser : User
    {
        public override List<ActionType> Actions
        {
            get
            {
                return new List<ActionType>()
    {
                    ActionType.QueryCharterIn,
                    ActionType.QueryCharterOut,
                    ActionType.ViewFuelReports,
                    ActionType.ViewScraps,
                    ActionType.ViewOrders,
                    ActionType.ViewInvoices,
                    ActionType.ViewOffhires,
                    ActionType.ViewVoyages, 
                    ActionType.QueryGoods,
                    ActionType.QueryVoyages,
                    ActionType.QueryCompanies,
                    ActionType.QueryVoucher,
                    ActionType.QueryVoucherSeting,
                       ActionType.QueryVessels,
                       ActionType.QueryVesselsInCompany,
                       ActionType.ChangePassWord,

                    ActionType.EditFinancialFuelReport,
                    ActionType.ViewInvoices,
                    ActionType.RegisterInvoice,
                    ActionType.EditInvoice,
                    ActionType.RemoveInvoice,
                    ActionType.ManageInvoiceApprovement,
                    ActionType.ManageInvoiceSubmittion,
                    ActionType.ManageEffectiveFactors,
                    //ActionType.ImportOffhire,
                    //ActionType.EditOffhire,
                    //ActionType.RemoveOffhire,
                    //ActionType.ManageOffhireApprovement,
                    //ActionType.ManageOffhireSubmittion,

                    //ActionType.AddVoucherSeting,
                    //ActionType.UpdateVoucherSeting,
                    //ActionType.AddVoucherSetingDetail,
                    //ActionType.UpdateVoucherSetingDetail,
                    //ActionType.QueryFinancialAccounts,
                    //ActionType.AddFinancialAccount,
                    //ActionType.QueryOriginalFinancialAccounts,
                    
                    ActionType.CancelInvoice,
                    //ActionType.CloseOrder,
                    ActionType.ManageFuelReportFinancialSubmition,
                    ActionType.ManageFuelReportFinancialReject,
                   };
            }
        }

        public FinancialUser()
        {
            
        }
        public FinancialUser(long id, string firstName, string lastName, string email, string userName)
            : base(id, "FinancialUser", firstName, lastName, email, userName)
        {

        }

    }
}
