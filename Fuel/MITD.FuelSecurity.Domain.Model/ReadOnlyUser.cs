using System.Collections.Generic;

namespace MITD.FuelSecurity.Domain.Model
{
    public class ReadOnlyUser : User
    {
        public ReadOnlyUser()
        {
            
        }

        public ReadOnlyUser(string firstName, string lastName, string email, string userName)
            : base(0, "ReadOnlyUser", firstName, lastName, email, userName)
        {

        }

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
                };
            }
        }
    }
}
