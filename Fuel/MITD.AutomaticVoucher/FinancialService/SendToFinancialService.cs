using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using MITD.AutomaticVoucher.HDAFinancialService;
using MITD.AutomaticVoucher.SAPIDFinancialService;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.AutomaticVoucher.FinancialService
{
    public class SendToFinancialService : ISendToFinancial
    {
        #region Prop
        private readonly IVoucherRepository _voucherRepository;
        private IUnitOfWorkScope _unitOfWorkScope;

        #endregion
        public SendToFinancialService(IVoucherRepository voucherRepository,IUnitOfWorkScope unitOfWorkScope)
        {
            _unitOfWorkScope = unitOfWorkScope;
            _voucherRepository = voucherRepository;
        }
       public void Send(List<long> ids, string date,string code)
       {
           var userId = long.Parse(ClaimsPrincipal.Current.Claims.First(c => c.Type == "CurrentUserId").Value);
            var fetchstartegy = new ListFetchStrategy<Voucher>(Enums.FetchInUnitOfWorkOption.NoTracking).Include(j => j.JournalEntrieses)
                .Include(c => c.JournalEntrieses.SelectMany(d => d.Segments)); ;
           var vouchers = _voucherRepository.Find(c => ids.Contains(c.Id),fetchstartegy).ToList();
           var companyId=(vouchers.Count > 0) ? vouchers.FirstOrDefault().CompanyId.Value : 0;

           switch (companyId)
           {
               //IRISL
               case 1:
               {
                   break;
               }
               //SAPID
               case 2:
               {
                   var service = new SAPIDFinancialVoucherService(_voucherRepository, _unitOfWorkScope);
                   service.Send(vouchers, date, code, userId);
                   break;
               }
               //HAFIZ
               case 3:
               {
                   break;
               }
               //IMSENGCO
               case 4:
               {
                   break;
               }
               //HAFEZ
               case 5:
               {
                   var service = new HafezFinancialVoucherService(_voucherRepository,_unitOfWorkScope);
                   service.Send(vouchers,date,code,userId);
                   break;
               }    

           }

           
       }

   
    }
}
