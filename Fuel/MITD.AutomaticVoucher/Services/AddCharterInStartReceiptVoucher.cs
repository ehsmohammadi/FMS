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
    public class AddCharterInStartReceiptVoucher : IAddCharterInStartReceiptVoucher
    {

        private readonly IVoucherRepository _voucherRepository;
        private readonly IVoucherSetingRepository _voucherSetingRepository;
        private readonly IInventoryOperationManager inventoryOperationManager;
        private IUnitOfWorkScope _unitOfWorkScope;
        private bool isreform;

        public AddCharterInStartReceiptVoucher(IVoucherRepository voucherRepository,
            IUnitOfWorkScope unitOfWorkScope
            , IVoucherSetingRepository voucherSetingRepository,
            IInventoryOperationManager inventoryOperationManager )
        {
            _voucherRepository = voucherRepository;
            _unitOfWorkScope = unitOfWorkScope;
            _voucherSetingRepository = voucherSetingRepository;
            this.inventoryOperationManager = inventoryOperationManager;
        }

        public void Execute(CharterIn charterIn, List<Receipt> receipts,
            string receiptWarehouseCode, string receiptNumber, long userId,
             string lineCode, string voyageCode, bool isReform = false)
        {

            try
            {
                isreform = isReform;
                var voucherSetingHeader = GetVoucherSeting(charterIn);

                var voucher = CreateVoucher(voucherSetingHeader, receiptNumber, charterIn, userId);
               
                receipts.ForEach(c =>
                {
                    var voucherSeting =
                        voucherSetingHeader.VoucherSetingDetails.SingleOrDefault(d => d.GoodId == c.GoodId && !d.IsDelete);

                    var debiJournalEntry = CreateDebitJournalEntry(c, voucherSeting,
                        receiptWarehouseCode, lineCode, voyageCode, isReform, charterIn,
                        charterIn.VesselInCompany.Vessel.Code);
                        voucher.JournalEntrieses.Add(debiJournalEntry);

                        var creditJournalEntry = CreateCreditJournalEntry(isReform, c, voucherSeting, charterIn, charterIn.VesselInCompany.Vessel.Code, lineCode, voyageCode, receiptWarehouseCode);
                    voucher.JournalEntrieses.Add(creditJournalEntry);


                });


                _voucherRepository.Add(voucher);

                inventoryOperationManager.SetInventoryTransactionStatusForRegisteredVoucher(receiptNumber);

                _unitOfWorkScope.Commit();

            }
            catch (Exception exp)
            {

                VoucherLogService voucherLogService=new VoucherLogService();
                voucherLogService.Add(receiptNumber, "1", exp);
                throw exp;
            
               
            }
        
        }

    VoucherSeting GetVoucherSeting(CharterIn charterIn)
    {
        var voucherSetingHeader =
            _voucherSetingRepository.Find(c => c.VoucherDetailTypeId == VoucherDetailType.CharterInStart.Id
                                            && c.CompanyId == charterIn.Charterer.Id)
                .FirstOrDefault();
        return voucherSetingHeader;
    }

    Voucher CreateVoucher(VoucherSeting voucherSetingHeader, string receiptNumber, CharterIn charterIn, long userId)
    {
        var voucher = new Voucher();
        voucher.LocalVoucherDate()
            .SetUser(userId)
              .SetVoucherType(voucherSetingHeader.VoucherTypeId)
              .SetVoucherDetailType(voucherSetingHeader.VoucherDetailTypeId)
            .SetCompany(charterIn.Charterer.Id)
            .FinancialVoucherDate(DateTime.Now)
            .Description(voucherSetingHeader.VoucherMainDescription)
            .ReferenceNo(receiptNumber)
            .LocalVoucherNo(LocalVoucherNoGenerator(charterIn.Charterer.Code))
            .VoucherRef(voucherSetingHeader.VoucherMainRefDescription)
            .SetReferenceType(ReferenceType.CharterIn);
        voucher.IsReform = isreform;
       
        if (isreform)
            voucher.Description(voucherSetingHeader.VoucherMainDescription + " / سند اصلاحی");
        else
            voucher.Description(voucherSetingHeader.VoucherMainDescription);
        
        voucher.JournalEntrieses = new List<JournalEntry>();
        return voucher;
    }


    JournalEntry CreateDebitJournalEntry(Receipt receipt, VoucherSetingDetail voucherSeting, string receiptWarehouseCode
       , string lineCode, string voyageCode, bool isReform, CharterIn charterIn, string vesselCode
       )
    {
        var debiJournalEntry = new JournalEntry();

        if (isReform)
        {
           debiJournalEntry= CreateReformDebitJournalEntry(receipt, voucherSeting, charterIn,
                vesselCode, lineCode, voyageCode);

        }
        else
        {
            debiJournalEntry = CreateDebitJournalEntry(receipt, voucherSeting, receiptWarehouseCode
                , lineCode, voyageCode
                );
        }


        return debiJournalEntry;
    }
        
        JournalEntry CreateDebitJournalEntry(Receipt receipt, VoucherSetingDetail voucherSeting, string receiptWarehouseCode
            ,  string lineCode, string voyageCode
            )
        {
            var debiJournalEntry = new JournalEntry();
            debiJournalEntry.Segments = new List<Segment>();
            debiJournalEntry
                   .InventoryItem(receipt.InventoryItemId)
                .Typ(1)
                .IrrAmount(receipt.ReceiptQuantity * receipt.ReceiptFee * receipt.Coefficient)
                .VoucherRef(lineCode+voyageCode)
                .Description(voucherSeting.VoucherDebitDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsDebit).Account.Code)
                .ForeignAmount(receipt.ReceiptFee * receipt.ReceiptQuantity)
            .SetCurrency(receipt.CurrencyId);

            voucherSeting.DebitSegmentTypes.ForEach(c => debiJournalEntry.Segments.Add(CreateDebitSegment(
                          c, receiptWarehouseCode)));


            debiJournalEntry.Description(DescriptionBuilder(receipt, receipt.CurrencyName));
            return debiJournalEntry;
        }


        JournalEntry CreateCreditJournalEntry(bool isReform, Receipt receipt, VoucherSetingDetail voucherSeting, CharterIn charterIn,
            string vesselCode, string lineCode, string voyageCode, string receiptWarehouseCode)
        {
            var creditJournalEntry = new JournalEntry();
            
            if (isReform)
            {
               creditJournalEntry = CreateReformCreditJournalEntry( receipt,  voucherSeting,  receiptWarehouseCode
                 , lineCode,  voyageCode);
           
            }
            else
            {
               creditJournalEntry = CreateCreditJournalEntry(receipt, voucherSeting, charterIn,
                    vesselCode, lineCode, voyageCode);
            }

            return creditJournalEntry;
        }
        
        JournalEntry CreateCreditJournalEntry(Receipt receipt, VoucherSetingDetail voucherSeting, CharterIn charterIn,
            string vesselCode, string lineCode, string voyageCode)
        {
            var creditJournalEntry = new JournalEntry();
            creditJournalEntry.Segments=new List<Segment>();
            creditJournalEntry
                   .InventoryItem(receipt.InventoryItemId)
                .Typ(2)
                .IrrAmount(receipt.ReceiptQuantity * receipt.ReceiptFee * receipt.Coefficient)
                .VoucherRef(vesselCode+lineCode+voyageCode)
                .Description(voucherSeting.VoucherCreditDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsCredit).Account.Code)
                .ForeignAmount(receipt.ReceiptFee * receipt.ReceiptQuantity)
                .SetCurrency(receipt.CurrencyId);
            
            voucherSeting.CreditSegmentTypes.ForEach(c => creditJournalEntry.Segments.Add(CreateCreditSegment(
             c, charterIn)));

            creditJournalEntry.Description(DescriptionBuilder(receipt, receipt.CurrencyName));
            return creditJournalEntry;
        }


        JournalEntry CreateReformCreditJournalEntry(Receipt receipt, VoucherSetingDetail voucherSeting, string receiptWarehouseCode
           , string lineCode, string voyageCode
           )
        {
            var debiJournalEntry = new JournalEntry();
            debiJournalEntry.Segments = new List<Segment>();
            debiJournalEntry
                   .InventoryItem(receipt.InventoryItemId)
                .Typ(2)
                .IrrAmount(receipt.ReceiptQuantity * receipt.ReceiptFee * receipt.Coefficient)
                .VoucherRef(lineCode + voyageCode)
                .Description(voucherSeting.VoucherDebitDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsDebit).Account.Code)
                .ForeignAmount(receipt.ReceiptFee * receipt.ReceiptQuantity)
            .SetCurrency(receipt.CurrencyId);

            voucherSeting.DebitSegmentTypes.ForEach(c => debiJournalEntry.Segments.Add(CreateDebitSegment(
                          c, receiptWarehouseCode)));


            debiJournalEntry.Description(DescriptionBuilder(receipt, receipt.CurrencyName));
            return debiJournalEntry;
        }

        JournalEntry CreateReformDebitJournalEntry(Receipt receipt, VoucherSetingDetail voucherSeting, CharterIn charterIn,
          string vesselCode, string lineCode, string voyageCode)
        {
            var creditJournalEntry = new JournalEntry();
            creditJournalEntry.Segments = new List<Segment>();
            creditJournalEntry
                   .InventoryItem(receipt.InventoryItemId)
                .Typ(1)
                .IrrAmount(receipt.ReceiptQuantity * receipt.ReceiptFee * receipt.Coefficient)
                .VoucherRef(vesselCode + lineCode + voyageCode)
                .Description(voucherSeting.VoucherCreditDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsCredit).Account.Code)
                .ForeignAmount(receipt.ReceiptFee * receipt.ReceiptQuantity)
                .SetCurrency(receipt.CurrencyId);

            voucherSeting.CreditSegmentTypes.ForEach(c => creditJournalEntry.Segments.Add(CreateCreditSegment(
             c, charterIn)));

            creditJournalEntry.Description(DescriptionBuilder(receipt, receipt.CurrencyName));
            return creditJournalEntry;
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

        private Segment CreateCreditSegment(AsgnSegmentTypeVoucherSetingDetail segmentType, CharterIn charterIn)
        {
            var res = new Segment();
            res.Name = "x";
            switch (segmentType.SegmentTypeId)
            {
                //Vessel
                case 1:
                    {
                        throw new BusinessRuleException("001", "Invalid Segment Type");
                       
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
                    res.Code = charterIn.Owner.Code;
                    res.SegmentType = SegmentType.Company;
                }
                    break;

            }
            return res;
        }

        string DescriptionBuilder(Receipt receipt, string currencyName)
        {
            var stringBuilder = new StringBuilder();
            if (isreform)
            {
                
            }
            else
            {
                stringBuilder.Append(string.Format("  {0}{1}  {2}  ", receipt.ReceiptQuantity, receipt.UnitName, receipt.GoodName));
                stringBuilder.Append(string.Format("اول دوره در تاریخ  : {0}  ", receipt.ReceiptDate.ToShortDateString()));
                stringBuilder.Append(string.Format("  هر {0} ", receipt.UnitName));
                stringBuilder.Append(string.Format(" {0}{1} ", receipt.ReceiptFee, currencyName));
            }
            
          

            return stringBuilder.ToString();
        }

        string LocalVoucherNoGenerator(string codeCompany)
        {
            return String.Format(codeCompany + _voucherRepository.GetLocalVoucherNo());
        }

    }
}
