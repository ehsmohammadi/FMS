using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.AutomaticVoucher.Log;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.AutomaticVoucher.Services
{
    public class DeleteVoucher : IDeleteVoucher
    {
        private IVoucherRepository _voucherRepository;
        private IUnitOfWorkScope _unitOfWorkScope;
        private readonly IInventoryOperationManager inventoryOperationManager;

        public DeleteVoucher(IVoucherRepository voucherRepository, IUnitOfWorkScope unitOfWorkScope, IInventoryOperationManager inventoryOperationManager)
        {
            _unitOfWorkScope = unitOfWorkScope;
            this.inventoryOperationManager = inventoryOperationManager;
            _voucherRepository = voucherRepository;
        }

        public void Done(long inventoryItemId, string headerCode)
        {

            var rep = ServiceLocator.Current.GetInstance<IRepository<JournalEntry>>();
            var rep1 = ServiceLocator.Current.GetInstance<IRepository<Segment>>();
            try
            {
                var res = _voucherRepository.GetAll().Where(c => c.ReferenceNo == headerCode).ToList();
                if (res.Count > 0)
                    foreach (var voucher in res)
                    {

                        if (!voucher.IsReform)
                        {


                            var jRes = new List<JournalEntry>();
                            jRes.AddRange(voucher.JournalEntrieses.ToList());
                            //jRes.AddRange(voucher.JournalEntrieses.Where(c => c.InventoryItemId == inventoryItemId).Select(d => d));
                            foreach (var journalEntry in jRes)
                            {
                                var x = new List<Segment>();
                                journalEntry.Segments.ForEach(c => x.Add(c));
                                foreach (var seg in x)
                                {
                                    var z = journalEntry.Segments.SingleOrDefault(c => c.Id == seg.Id);
                                    rep1.Delete(z);
                            }
                            }

                            foreach (var journalEntry in jRes)
                            {
                                rep.Delete(voucher.JournalEntrieses.SingleOrDefault(c => c.Id == journalEntry.Id));
                            }



                            _voucherRepository.Delete(voucher);
                        }
                    }

                inventoryOperationManager.SetInventoryTransactionStatusForDeletedVoucher(headerCode);

                _unitOfWorkScope.Commit();
            }
            catch (Exception exp)
            {

                var voucherLogService = new VoucherLogService();
                voucherLogService.Add("Delete" + " inventoryItemId" + inventoryItemId.ToString() + "headerCode" + headerCode, "2", exp);
                throw exp;
            }

        }
    }
}
