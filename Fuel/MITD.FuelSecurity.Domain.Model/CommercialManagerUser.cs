using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.FuelSecurity.Domain.Model
{
    public class CommercialManagerUser : User
    {
        public CommercialManagerUser()
        {
            
        }
        public override List<ActionType> Actions
        {
            get
            {
                return new List<ActionType>()
                       {
                            ActionType.QueryCharterIn,
                            ActionType.AddCharterIn,
                            ActionType.EditCharterIn,
                            ActionType.DeleteCharterIn,
                            ActionType.AddCharterInItem,
                            ActionType.EditCharterInItem,
                            ActionType.DeleteCharterInItem,
                            ActionType.QueryCharterOut,
                            ActionType.AddCharterOut,
                            ActionType.EditCharterOut,
                            ActionType.DeleteCharterOut,
                            ActionType.AddCharterOutItem,
                            ActionType.EditCharterOutItem,
                            ActionType.DeleteCharterOutItem,
                            ActionType.ViewFuelReports,
                            ActionType.EditFuelReport,
                            ActionType.ImportFuelReports,
                            ActionType.ManageFuelReportApprovement,
                            ActionType.ManageFuelReportSubmittion,
                            ActionType.ViewScraps,
                            ActionType.CreateScrap,
                            ActionType.EditScrap,
                            ActionType.RemoveScrap,
                            ActionType.ManageScrapApprovement,
                            ActionType.ManageScrapSubmittion,
                            ActionType.ViewOrders,
                            ActionType.CreateOrder,
                            ActionType.EditOrder,
                            ActionType.RemoveOrder,
                            ActionType.ManageOrderApprovement,
                            ActionType.ManageOrderSubmittion,
                            ActionType.CancelOrder,
                            ActionType.ViewInvoices,
                            ActionType.RegisterInvoice,
                            ActionType.EditInvoice,
                            ActionType.RemoveInvoice,
                            ActionType.ManageInvoiceApprovement,
                            ActionType.ManageInvoiceSubmittion,
                            ActionType.ManageEffectiveFactors,
                            ActionType.ViewOffhires,
                            ActionType.ImportOffhire,
                            ActionType.EditOffhire,
                            ActionType.RemoveOffhire,
                            ActionType.ManageOffhireApprovement,
                            ActionType.ManageOffhireSubmittion,
                            ActionType.ViewVoyages, 
                            ActionType.QueryGoods,
                            ActionType.QueryVoyages,
                            ActionType.QueryCompanies,
                            ActionType.QueryVessels,
                            ActionType.QueryVesselsInCompany,
                            ActionType.CancelInvoice,
                            ActionType.CloseOrder,
                            ActionType.ChangePassWord,
                            ActionType.RemoveFuelReport,
                            ActionType.UpdateVoyagesFromRotationData,
                            ActionType.RejectSubmittedOrder,
                            ActionType.ResubmitRejectedOrder,
                            ActionType.RejectSubmittedInvoice,
                            ActionType.ResubmitRejectedInvoice,
                        };
            }
        }

        public CommercialManagerUser(long id, string firstName, string lastName, string email, string userName)
            : base(id, "CommercialManagerUser", firstName, lastName, email, userName)
        {

        }
    }
}
