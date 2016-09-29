using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.AutomaticVoucher.Data;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;

namespace MITD.AutomaticVoucher.FinancialService
{
    public class VoucherTransferLogService
    {
        public List<VoucherTransferLog> GetLogByVoucherId(long Id)
        {
            var context = new VoucherLogEntities();
            var result = new List<VoucherTransferLog>();
            var res = context.VoucherTransferLogs.Where(c => c.VoucherIds.Contains(Id.ToString())).ToList();
            res.ForEach(r =>
            {
                r.VoucherIds.Split(',').ToList().ForEach(d =>
                {
                    if (d == Id.ToString())
                    {
                        result.Add(r);

                    }
                });


            });

            return result;
        }

        public void Add(List<Voucher> vouchers, string date, string departmenCode, string message, long userId)
        {
            
            var log = new VoucherTransferLog();
            var contex = new VoucherLogEntities();
            log.ConfigCode = departmenCode;
            log.ConfigDate = date;
            log.FinancialExceptionMessage = message;
            vouchers.ForEach(c =>
            {
                log.VoucherIds += c.Id.ToString() + ",";
            });
            log.UserId = userId;
            log.SendDate = DateTime.Now;
            contex.Entry(log).State = EntityState.Added;
            contex.VoucherTransferLogs.Add(log);
            contex.SaveChanges();
        }
    }
}
