using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.AutomaticVoucher.Services
{
    public class AddTransferBarjingInvoiceVoucher : IAddTransferBarjingInvoiceVoucher
    {
        #region Prop
        private readonly IVoucherRepository _voucherRepository;
        private readonly IVoucherSetingRepository _voucherSetingRepository;
        private readonly IInventoryOperationManager inventoryOperationManager;
        private IUnitOfWorkScope _unitOfWorkScope;

        #endregion

        public AddTransferBarjingInvoiceVoucher(IVoucherRepository voucherRepository,
            IUnitOfWorkScope unitOfWorkScope
            , IVoucherSetingRepository voucherSetingRepository, IInventoryOperationManager inventoryOperationManager)
        {
            _voucherRepository = voucherRepository;
            _unitOfWorkScope = unitOfWorkScope;
            _voucherSetingRepository = voucherSetingRepository;
            this.inventoryOperationManager = inventoryOperationManager;
        }
        public void Execute(Invoice invoice, List<Receipt> receipts, string receiptwarehousecode, decimal aditionalCoeff, FuelReport fuelReport
           , string inventoryActionNumber, long userId)
        {
            var voucherSetingHeader = GetVoucherSeting(invoice);

            var voucher = CreateVoucher(voucherSetingHeader, invoice.InvoiceNumber, inventoryActionNumber, invoice, userId);


            receipts.ForEach(c =>
            {
                var voucherSeting =
                    voucherSetingHeader.VoucherSetingDetails.SingleOrDefault(d => d.GoodId == c.GoodId && !d.IsDelete);

                var debiJournalEntry = CreateDebitJournalEntry(c, voucherSeting, receiptwarehousecode, fuelReport, invoice);
                voucher.JournalEntrieses.Add(debiJournalEntry);

                var creditJournalEntry = CreateCreditJournalEntry(c, voucherSeting, invoice, receiptwarehousecode, fuelReport);
                voucher.JournalEntrieses.Add(creditJournalEntry);


            });


            invoice.AdditionalPrices.ForEach(c =>
            {
                if (c.Divisionable)
                {

                    if (invoice.IsCreditor)
                    {
                        var creditJournalEntry = CreateAditionalCreditJournalEntry(invoice, c, receiptwarehousecode, aditionalCoeff);
                        voucher.JournalEntrieses.Add(creditJournalEntry);
                    }
                    else
                    {
                        var debiJournalEntry = CreateAditionalDebitJournalEntry(invoice, c, receiptwarehousecode, aditionalCoeff);
                        voucher.JournalEntrieses.Add(debiJournalEntry);
                    }
                }
            });


            _voucherRepository.Add(voucher);

            inventoryOperationManager.SetInventoryTransactionStatusForRegisteredVoucher(inventoryActionNumber);

            _unitOfWorkScope.Commit();
        }


        VoucherSeting GetVoucherSeting(Invoice invoice)
        {
            var voucherSetingHeader = new VoucherSeting();
            voucherSetingHeader = _voucherSetingRepository.Find(c => c.VoucherDetailTypeId == VoucherDetailType.InvoiceTransfer.Id
                                              && c.CompanyId == invoice.OwnerId)
                 .FirstOrDefault();


            return voucherSetingHeader;
        }

        Voucher CreateVoucher(VoucherSeting voucherSetingHeader, string invoceNumber, string inventoryActionNumber, Invoice invoice, long userId)
        {
            var voucher = new Voucher();
            voucher
                .SetUser(userId)
            .SetVoucherType(voucherSetingHeader.VoucherTypeId)
          .SetVoucherDetailType(voucherSetingHeader.VoucherDetailTypeId)
                .SetCompany(invoice.OwnerId)
                .LocalVoucherDate()
                .FinancialVoucherDate(DateTime.Now)
                .Description(voucherSetingHeader.VoucherMainDescription)
                //A.H 94-02-15  ["Invoice/" + invoceNumber + "|"] part removed.
                //.ReferenceNo("Invoice/" + invoceNumber + "|" + inventoryActionNumber)
                 .ReferenceNo(inventoryActionNumber)
                 .LocalVoucherNo(LocalVoucherNoGenerator(invoice.Owner.Code))
                .VoucherRef(voucherSetingHeader.VoucherMainRefDescription)
                .SetReferenceType(ReferenceType.PurchesInvoice);


            voucher.JournalEntrieses = new List<JournalEntry>();
            return voucher;
        }

        JournalEntry CreateDebitJournalEntry(Receipt receipt, VoucherSetingDetail voucherSeting, string receiptwarehousecode, FuelReport fuelReport, Invoice invoice)
        {
            var debiJournalEntry = new JournalEntry();
            debiJournalEntry.Segments = new List<Segment>();
            debiJournalEntry
                 .InventoryItem(receipt.InventoryItemId)
                .Typ(1)
                .IrrAmount(receipt.ReceiptQuantity * receipt.ReceiptFee * receipt.Coefficient)
                .VoucherRef(fuelReport.VesselInCompany.Vessel.Code)
                .Description(voucherSeting.VoucherDebitDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsDebit).Account.Code)
                .ForeignAmount(receipt.ReceiptFee * receipt.ReceiptQuantity)
                .SetCurrency(receipt.CurrencyId);

            voucherSeting.DebitSegmentTypes.ForEach(c =>
                debiJournalEntry.Segments.Add(CreateDebitSegment(
            c, receiptwarehousecode)));

            debiJournalEntry.Description(DescriptionBuilder(receipt, invoice));


            return debiJournalEntry;
        }
        JournalEntry CreateAditionalDebitJournalEntry(Invoice invoice, InvoiceAdditionalPrice invoiceAdditionalPrice, string recieptWarehouseCode, decimal coeff)
        {
            var debiJournalEntry = new JournalEntry();
            debiJournalEntry.Segments = new List<Segment>();
            debiJournalEntry

                .Typ(1)
                .IrrAmount(invoiceAdditionalPrice.Price * coeff)
                .VoucherRef(invoiceAdditionalPrice.EffectiveFactor.VoucherRefDescription)
                .Description(invoiceAdditionalPrice.EffectiveFactor.VoucherDescription)
                .AccountNo(invoiceAdditionalPrice.EffectiveFactor.Account.Code)
                .ForeignAmount(invoiceAdditionalPrice.Price)
                .SetCurrency(invoice.CurrencyId);

            //A.H : This checking is added to prevent creation of voucher if the EffectiveFactors do not have any Segments
            if (invoiceAdditionalPrice.EffectiveFactor.Segments.Count == 0)
                throw new BusinessRuleException("", "Selected Effective Factor does not have any JournalEntry Segment settings.");

            invoiceAdditionalPrice.EffectiveFactor.Segments.ForEach(c => debiJournalEntry.Segments.Add(
                CreateAditionalDebitSegment(c,
                    recieptWarehouseCode)));




            return debiJournalEntry;
        }

        JournalEntry CreateCreditJournalEntry(Receipt receipt, VoucherSetingDetail voucherSeting, Invoice invoice, string receiptWarehouseCode, FuelReport fuelReport)
        {
            var creditJournalEntry = new JournalEntry();
            creditJournalEntry.Segments = new List<Segment>();
            creditJournalEntry
                .InventoryItem(receipt.InventoryItemId)
                .Typ(2)
                .IrrAmount(receipt.ReceiptQuantity * receipt.ReceiptFee * receipt.Coefficient)
                .VoucherRef(invoice.InvoiceNumber)
                .Description(voucherSeting.VoucherDebitDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsCredit).Account.Code)
                .ForeignAmount(receipt.ReceiptFee * receipt.ReceiptQuantity)
                .SetCurrency(receipt.CurrencyId);

            voucherSeting.CreditSegmentTypes.ForEach(c => creditJournalEntry.Segments.Add(CreateCreditSegment(
            c, invoice)));



            creditJournalEntry.Description(DescriptionBuilder(receipt, invoice));
            return creditJournalEntry;
        }

        JournalEntry CreateAditionalCreditJournalEntry(Invoice invoice, InvoiceAdditionalPrice invoiceAdditionalPrice, string recieptWarehouseCode, decimal coeff)
        {
            var creditJournalEntry = new JournalEntry();
            creditJournalEntry.Segments = new List<Segment>();
            creditJournalEntry

                .Typ(1)
                .IrrAmount(invoiceAdditionalPrice.Price * coeff)
                .VoucherRef(invoiceAdditionalPrice.EffectiveFactor.VoucherRefDescription)
                .Description(invoiceAdditionalPrice.EffectiveFactor.VoucherDescription)
                .AccountNo(invoiceAdditionalPrice.EffectiveFactor.Account.Code)
                .ForeignAmount(invoiceAdditionalPrice.Price)
                .SetCurrency(invoice.CurrencyId);

            //A.H : This checking is added to prevent creation of voucher if the EffectiveFactors do not have any Segments
            if (invoiceAdditionalPrice.EffectiveFactor.Segments.Count == 0)
                throw new BusinessRuleException("", "Selected Effective Factor does not have any JournalEntry Segment settings.");

            invoiceAdditionalPrice.EffectiveFactor.Segments.ForEach(c => creditJournalEntry.Segments.Add(
                CreateAditionalCreditSegment(c,
                    recieptWarehouseCode)));






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

        private Segment CreateAditionalDebitSegment(Segment segmentType, string receiptWarehouseCode)
        {
            var res = new Segment();
            res.Name = "x";
            switch (segmentType.SegmentType.Id)
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
                //Fix
                case 5:
                    {
                        res.Code = segmentType.FreeAccount.Code;
                        res.SegmentType = SegmentType.Fix;

                    }
                    break;

            }
            return res;
        }
        private Segment CreateCreditSegment(AsgnSegmentTypeVoucherSetingDetail segmentType, Invoice invoice)
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
                        res.Code = invoice.Supplier.Code;
                        res.SegmentType = SegmentType.Company;

                    }
                    break;

            }
            return res;
        }
        private Segment CreateAditionalCreditSegment(Segment segmentType, string recieptWarehouseCode)
        {
            var res = new Segment();
            res.Name = "x";
            switch (segmentType.SegmentType.Id)
            {
                //Vessel
                case 1:
                    {
                        res.Code = recieptWarehouseCode;
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
                //Fix
                case 5:
                    {
                        res.Code = segmentType.FreeAccount.Code;
                        res.SegmentType = SegmentType.Fix;

                    }
                    break;

            }
            return res;
        }

        string LocalVoucherNoGenerator(string codeCompany)
        {
            return String.Format(codeCompany + _voucherRepository.GetLocalVoucherNo());
        }



        string DescriptionBuilder(Receipt receipt, Invoice invoice)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format(" بابت انتقال {0}{1} {2}  ", receipt.ReceiptQuantity, receipt.UnitName, receipt.GoodName));
            stringBuilder.Append(string.Format("درتاریخ    : {0}  ", invoice.InvoiceDate.ToShortDateString()));

            return stringBuilder.ToString();

        }

    }
}
