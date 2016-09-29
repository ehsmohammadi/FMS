using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.AutomaticVoucher.Log;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.AutomaticVoucher.Services
{
    public class AddCharterOutStartConsumptionIssueVoucher : IAddCharterOutStartConsumptionIssueVoucher
    {
        #region Prop
        private readonly IVoucherRepository _voucherRepository;
        private readonly IVoucherSetingRepository _voucherSetingRepository;
        private readonly IInventoryOperationManager inventoryOperationManager;
        private IUnitOfWorkScope _unitOfWorkScope;

        #endregion

        public AddCharterOutStartConsumptionIssueVoucher(IVoucherRepository voucherRepository,
            IUnitOfWorkScope unitOfWorkScope
            , IVoucherSetingRepository voucherSetingRepository, IInventoryOperationManager inventoryOperationManager)
        {
            _voucherRepository = voucherRepository;
            _unitOfWorkScope = unitOfWorkScope;
            _voucherSetingRepository = voucherSetingRepository;
            this.inventoryOperationManager = inventoryOperationManager;
        }

        public void Execute(CharterOut charterOut, List<Issue> issues, string issueWarehouseCode, string issueNumber, long userId, string lineCode, string voyageCode)
        {
            try
            {
                var voucherSetingHeader = GetVoucherSeting(charterOut);

                var voucher = CreateVoucher(voucherSetingHeader, issueNumber, charterOut, userId);

                issues.ForEach(c =>
                {
                    var voucherSeting =
                        voucherSetingHeader.VoucherSetingDetails.SingleOrDefault(d => d.GoodId == c.GoodId && !d.IsDelete);

                    var debiJournalEntry = CreateDebitJournalEntry(c, voucherSeting, charterOut, issueWarehouseCode, lineCode, voyageCode);
                    voucher.JournalEntrieses.Add(debiJournalEntry);

                    var creditJournalEntry = CreateCreditJournalEntry(c, voucherSeting, issueWarehouseCode,charterOut.VesselInCompany.Code, lineCode, voyageCode);
                    voucher.JournalEntrieses.Add(creditJournalEntry);


                });

                _voucherRepository.Add(voucher);

                inventoryOperationManager.SetInventoryTransactionStatusForRegisteredVoucher(issueNumber);

                _unitOfWorkScope.Commit();

            }
            catch (Exception exp)
            {

                VoucherLogService voucherLogService = new VoucherLogService();
                voucherLogService.Add(issueNumber, "2", exp);
                throw exp;


            }
        }

        VoucherSeting GetVoucherSeting(CharterOut charterOut)
        {
            var voucherSetingHeader =
                _voucherSetingRepository.Find(c => c.VoucherDetailTypeId == VoucherDetailType.CharterOutStart.Id
                                                 && c.CompanyId == charterOut.Owner.Id)
                    .FirstOrDefault();

            return voucherSetingHeader;
        }

        Voucher CreateVoucher(VoucherSeting voucherSetingHeader, string issueNumber, CharterOut charterOut, long userId)
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
                .ReferenceNo(issueNumber)
                   .LocalVoucherNo(LocalVoucherNoGenerator(charterOut.Owner.Code))
                .VoucherRef(voucherSetingHeader.VoucherMainRefDescription)
                .SetReferenceType(ReferenceType.CharterOut);


            voucher.JournalEntrieses = new List<JournalEntry>();
            return voucher;
        }

        JournalEntry CreateDebitJournalEntry(Issue issue, VoucherSetingDetail voucherSeting, CharterOut charterOut,
           string lineCode, string voyageCode, string issueWarehouseCode)
        {
            var debiJournalEntry = new JournalEntry();
            debiJournalEntry.Segments = new List<Segment>();
            debiJournalEntry
                .InventoryItem(issue.InventoryItemId)
                .Typ(1)
                .IrrAmount(issue.IssueQuantity * issue.IssueFee * issue.Coefficient)
                .VoucherRef(lineCode + " " + voyageCode)
                .Description(voucherSeting.VoucherDebitDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsDebit).Account.Code)
                .ForeignAmount(issue.IssueFee * issue.IssueQuantity)
                .SetCurrency(issue.CurrencyId);


            voucherSeting.DebitSegmentTypes.ForEach(c => debiJournalEntry.Segments.Add(CreateDebitSegment(
                          c, issueWarehouseCode,lineCode,voyageCode,charterOut)));


            debiJournalEntry.Description(DescriptionBuilder(issue));

            return debiJournalEntry;
        }

        JournalEntry CreateCreditJournalEntry(Issue issue, VoucherSetingDetail voucherSeting, string issueWarehouseCode,
            string vesselCode, string lineCode, string voyageCode)
        {
            var creditJournalEntry = new JournalEntry();
            creditJournalEntry.Segments = new List<Segment>();
            creditJournalEntry
                .InventoryItem(issue.InventoryItemId)
                .Typ(2)
                .IrrAmount(issue.IssueQuantity * issue.IssueFee * issue.Coefficient)
                .VoucherRef(vesselCode + " " + lineCode + " " + voyageCode)
                .Description(voucherSeting.VoucherCreditDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsCredit).Account.Code)
                .ForeignAmount(issue.IssueFee * issue.IssueQuantity)
            .SetCurrency(issue.CurrencyId);

            voucherSeting.CreditSegmentTypes.ForEach(c => creditJournalEntry.Segments.Add(CreateCreditSegment(
                c, issueWarehouseCode)));

            creditJournalEntry.Description(DescriptionBuilder(issue));
            return creditJournalEntry;
        }

        private Segment CreateDebitSegment(AsgnSegmentTypeVoucherSetingDetail segmentType, string issueWarehouseCode,
             string lineCode, string voyageCode,CharterOut charterOut)
        {
            var res = new Segment();
            res.Name = "x";
            switch (segmentType.SegmentTypeId)
            {
                //Vessel
                case 1:
                    {
                        res.Code = issueWarehouseCode;
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

        private Segment CreateCreditSegment(AsgnSegmentTypeVoucherSetingDetail segmentType, string issueWarehouseCode)
        {
            var res = new Segment();
            res.Name = "x";
            switch (segmentType.SegmentTypeId)
            {
                //Vessel
                case 1:
                    {
                        res.Code = issueWarehouseCode;
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


        private string DescriptionBuilder(Issue issue)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(String.Format("اختلاف موجودی و Charter Out Start  : {0} {1} {2} درتاریخ : {3} بافی : {4} {5}",
                issue.IssueQuantity, issue.UnitName, issue.GoodName, issue.IssueDate, issue.IssueFee, issue.CurrencyName));

            return stringBuilder.ToString();
        }
        string LocalVoucherNoGenerator(string codeCompany)
        {
            return String.Format(codeCompany + _voucherRepository.GetLocalVoucherNo());
        }

    }
}
