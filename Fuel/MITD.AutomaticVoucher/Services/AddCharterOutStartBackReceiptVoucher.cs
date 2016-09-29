using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MITD.AutomaticVoucher.Log;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.AutomaticVoucher.Services
{
    public class AddCharterOutStartBackReceiptVoucher : IAddCharterOutStartBackReceiptVoucher
    {
        #region Prop
        private readonly IVoucherRepository _voucherRepository;
        private readonly IVoucherSetingRepository _voucherSetingRepository;
        private IUnitOfWorkScope _unitOfWorkScope;
        private readonly IInventoryOperationManager inventoryOperationManager;

        #endregion

        public AddCharterOutStartBackReceiptVoucher(IVoucherRepository voucherRepository,
            IUnitOfWorkScope unitOfWorkScope
            , IVoucherSetingRepository voucherSetingRepository, IInventoryOperationManager inventoryOperationManager)
        {
            _voucherRepository = voucherRepository;
            _unitOfWorkScope = unitOfWorkScope;
            _voucherSetingRepository = voucherSetingRepository;
            this.inventoryOperationManager = inventoryOperationManager;
        }

        public void Execute(CharterOut charterOut, List<Receipt> receipts, string receiptWarehouseCode, string receiptNumber, long userId,
             string lineCode, string voyageCode)
        {
            try
            {
                var voucherSetingHeader = GetVoucherSeting(charterOut);

                var voucher = CreateVoucher(voucherSetingHeader, receiptNumber, charterOut, userId);

                receipts.ForEach(c =>
                {
                    var voucherSeting =
                        voucherSetingHeader.VoucherSetingDetails.SingleOrDefault(d => d.GoodId == c.GoodId && !d.IsDelete);

                    var debiJournalEntry = CreateDebitJournalEntry(c, voucherSeting, charterOut, receiptWarehouseCode,lineCode,voyageCode);
                    voucher.JournalEntrieses.Add(debiJournalEntry);

                    var creditJournalEntry = CreateCreditJournalEntry(c, voucherSeting, receiptWarehouseCode, charterOut,lineCode,voyageCode);
                    voucher.JournalEntrieses.Add(creditJournalEntry);


                });

                _voucherRepository.Add(voucher);

                inventoryOperationManager.SetInventoryTransactionStatusForRegisteredVoucher(receiptNumber);

                _unitOfWorkScope.Commit();

            }
            catch (Exception exp)
            {

                VoucherLogService voucherLogService = new VoucherLogService();
                voucherLogService.Add(receiptNumber, "2", exp);
                throw exp;


            }
        }

        VoucherSeting GetVoucherSeting(CharterOut charterOut)
        {
            var voucherSetingHeader = new VoucherSeting();
            voucherSetingHeader = _voucherSetingRepository.Find(c => c.VoucherDetailTypeId == VoucherDetailType.PlusCorrection.Id
                                              && c.CompanyId == charterOut.Owner.Id)
                 .FirstOrDefault();


            return voucherSetingHeader;
        }

        Voucher CreateVoucher(VoucherSeting voucherSetingHeader, string receiptNumber, CharterOut charterOut, long userId)
        {
            var voucher = new Voucher();
            voucher
                .SetUser(userId)
                .SetVoucherType(voucherSetingHeader.VoucherTypeId)
                .SetVoucherDetailType(voucherSetingHeader.VoucherDetailTypeId)
                .SetCompany(charterOut.Owner.Id)
                .LocalVoucherDate()
                .FinancialVoucherDate(DateTime.Now)
                .Description(voucherSetingHeader.VoucherMainDescription)
                .ReferenceNo(receiptNumber)
                .LocalVoucherNo(LocalVoucherNoGenerator(charterOut.Owner.Code))
                .VoucherRef(voucherSetingHeader.VoucherMainRefDescription)
                .SetReferenceType(ReferenceType.CharterOut);


            voucher.JournalEntrieses = new List<JournalEntry>();
            return voucher;
        }

        JournalEntry CreateDebitJournalEntry(Receipt receipt, VoucherSetingDetail voucherSeting, CharterOut charterOut,
         string receiptWarehouseCode, string lineCode, string voyageCode)
        {
            var debiJournalEntry = new JournalEntry();
            debiJournalEntry.Segments = new List<Segment>();
            debiJournalEntry
                   .InventoryItem(receipt.InventoryItemId)
                .Typ(1)
                .IrrAmount(receipt.ReceiptQuantity * receipt.ReceiptFee * receipt.Coefficient)
                .VoucherRef(charterOut.VesselInCompany.Vessel.Code + lineCode+voyageCode)
                .Description(voucherSeting.VoucherDebitDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsDebit).Account.Code)
                .ForeignAmount(receipt.ReceiptQuantity * receipt.ReceiptFee)
                .SetCurrency(receipt.CurrencyId);

            voucherSeting.DebitSegmentTypes.ForEach(c => debiJournalEntry.Segments.Add(CreateDebitSegment(
                c, receiptWarehouseCode)));


            debiJournalEntry.Description
               (
               DescriptionBuilder
               (
               receipt
               )
               );

            return debiJournalEntry;
        }


        JournalEntry CreateCreditJournalEntry(Receipt receipt, VoucherSetingDetail voucherSeting, string issueWarehouseCode,
            CharterOut charterOut, string lineCode, string voyageCode)
        {
            var creditJournalEntry = new JournalEntry();
            creditJournalEntry.Segments = new List<Segment>();
            creditJournalEntry
                   .InventoryItem(receipt.InventoryItemId)
                .Typ(2)
                .IrrAmount(receipt.ReceiptQuantity * receipt.ReceiptFee * receipt.Coefficient)
              .VoucherRef(charterOut.VesselInCompany.Vessel.Code + lineCode + voyageCode)
                .Description(voucherSeting.VoucherDebitDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsCredit).Account.Code)
                .ForeignAmount(receipt.ReceiptQuantity * receipt.ReceiptFee)
                .SetCurrency(receipt.CurrencyId);

            voucherSeting.CreditSegmentTypes.ForEach(c => creditJournalEntry.Segments.Add(CreateCreditSegment(
            c, charterOut, issueWarehouseCode,lineCode,voyageCode)));



            creditJournalEntry.Description
              (
              DescriptionBuilder
              (
              receipt
              )
              );

            return creditJournalEntry;
        }

        
        private Segment CreateCreditSegment(AsgnSegmentTypeVoucherSetingDetail segmentType, CharterOut charterOut, string receiptWarehouseCode,
             string lineCode, string voyageCode)
        {
            var res = new Segment();
            res.Name = "x";
            switch (segmentType.SegmentTypeId)
            {
                //Vessel
                case 1:
                    {
                        res.Code = receiptWarehouseCode;
                        res.SegmentType = SegmentType.Vessel;
                    }
                    break;
                //Port
                case 2:
                    {

                        res.SegmentType = SegmentType.Port;

                        if (charterOut.OwnerId == 5)
                        {
                            res.Code = "9999";
                        }
                        else if (charterOut.OwnerId == 2)
                        {
                            res.Code = "1102";
                        }
                    }
                    break;
                //Voayage
                case 3:
                    {
                        res.Code = lineCode + voyageCode; ;
                        res.SegmentType = SegmentType.Voayage;
                    }
                    break;
                //Company
                case 4:
                    {
                        throw new BusinessRuleException("001", "Invalid Segment Type");
                    }
                    break;

            }
            return res;
        }
        
        private Segment CreateDebitSegment(AsgnSegmentTypeVoucherSetingDetail segmentType, string receiptWarehouseCode)
        {
            var res = new Segment();
            res.Name = "x";
            switch (segmentType.SegmentTypeId)
            {
                //Vessel
                case 1:
                    {
                        res.Code = receiptWarehouseCode;
                        res.SegmentType = SegmentType.Vessel;
                    }
                    break;
                //Port
                case 2:
                    {
                        throw new BusinessRuleException("001", "Invalid Segment Type");
                    }
                    break;
                //Voayage
                case 3:
                    {
                        throw new BusinessRuleException("001", "Invalid Segment Type");
                    }
                    break;
                //Company
                case 4:
                    {
                        throw new BusinessRuleException("001", "Invalid Segment Type");
                    }
                    break;

            }
            return res;
        }

        private string DescriptionBuilder(Receipt receipt)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(String.Format("اختلاف موجودی و Charter Out Start  : {0} {1} {2} درتاریخ : {3} بافی : {4} {5}", 
                receipt.ReceiptQuantity, receipt.UnitName, receipt.GoodName, receipt.ReceiptDate,receipt.ReceiptFee, receipt.CurrencyName));
          
            return stringBuilder.ToString();
        }

        string LocalVoucherNoGenerator(string codeCompany)
        {
            return String.Format(codeCompany + _voucherRepository.GetLocalVoucherNo());
        }
    }
}
