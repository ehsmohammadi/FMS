using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Services.Application;

namespace MITD.Fuel.Application.Service.Contracts
{
   public interface IVoucherSetingApplicationService:IApplicationService
   {
       void AddVoucherSeting(
             long companyId
            , int voucherDetailTypeId
            , int voucherTypeId,
            string voucherMainRefDescription,
            string voucherMainDescription);

       void UpdateVoucherSeting(long id,
             long companyId
            , int voucherDetailTypeId
            , int voucherTypeId,
            string voucherMainRefDescription,
            string voucherMainDescription);

       void AddVoucherSetingDetail(long goodId, long voucherSetingId, string voucherDebitDescription,
           string voucherDebitRefDescription, string voucherCreditDescription, string voucherCeditRefDescription,
           List<int> debitSegmentTypes, int debitAccountId, List<int> creditSegmentTypes, int creditAccountId);


       void UpdateVoucherSetingDetail(long id,long goodId, long voucherSetingId, string voucherDebitDescription,
           string voucherDebitRefDescription, string voucherCreditDescription, string voucherCeditRefDescription,
           List<int> debitSegmentTypes, int debitAccountId, List<int> creditSegmentTypes, int creditAccountId);

       void UpdateDelete(long voucherSetingId, long voucherSetingDetailId);
   }
}
