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
    public class AddCharterInEndIssueVoucher : IAddCharterInEndIssueVoucher
    {
                #region Prop
        private readonly IVoucherRepository _voucherRepository;
        private readonly IVoucherSetingRepository _voucherSetingRepository;
        private readonly IInventoryOperationManager inventoryOperationManager;
        private IUnitOfWorkScope _unitOfWorkScope;
        private bool isReform;
        #endregion

        public AddCharterInEndIssueVoucher(IVoucherRepository voucherRepository,
            IUnitOfWorkScope unitOfWorkScope
            , IVoucherSetingRepository voucherSetingRepository, IInventoryOperationManager inventoryOperationManager)
        {
            _voucherRepository = voucherRepository;
            _unitOfWorkScope = unitOfWorkScope;
            _voucherSetingRepository = voucherSetingRepository;
            this.inventoryOperationManager = inventoryOperationManager;
        }

        
        public void Execute(CharterIn charterIn, List<Issue> issues, string issueWarehouseCode,
            string issueNumber,long userId,string lineCode,string voyageCode,bool isReform = false)
        {
            try
            {

                this.isReform = isReform;
                var voucherSetingHeader = GetVoucherSeting(charterIn);

                var voucher = CreateVoucher(voucherSetingHeader, issueNumber,charterIn,userId);
                
                issues.ForEach(c =>
                {
                    var voucherSeting =
                        voucherSetingHeader.VoucherSetingDetails.SingleOrDefault(d => d.GoodId == c.GoodId && !d.IsDelete);

                    var debiJournalEntry = CreateDebitJournalEntry(c, voucherSeting, charterIn, lineCode, voyageCode, isReform, issueWarehouseCode, charterIn.VesselInCompany.Vessel.Code);
                    voucher.JournalEntrieses.Add(debiJournalEntry);

                    var creditJournalEntry = CreateCreditJournalEntry(c, voucherSeting, issueWarehouseCode, charterIn.VesselInCompany.Vessel.Code, lineCode, voyageCode,isReform,charterIn);
                    voucher.JournalEntrieses.Add(creditJournalEntry);


                });

                _voucherRepository.Add(voucher);

                inventoryOperationManager.SetInventoryTransactionStatusForRegisteredVoucher(issueNumber);

                _unitOfWorkScope.Commit();
            }
            catch (Exception exp)
            {

                VoucherLogService voucherLogService=new VoucherLogService();
                voucherLogService.Add(issueNumber,"2",exp);
                throw exp;
            }
            
   
        }


        VoucherSeting GetVoucherSeting(CharterIn charterIn)
        {
            var voucherSetingHeader =
                _voucherSetingRepository.Find(c => c.VoucherDetailTypeId == VoucherDetailType.CharterInEnd.Id
                                                 && c.CompanyId == charterIn.Charterer.Id)
                    .FirstOrDefault();

            return voucherSetingHeader;
        }

        Voucher CreateVoucher(VoucherSeting voucherSetingHeader, string issueNumber,CharterIn charterIn,long userId)
        {
            var voucher = new Voucher();
            voucher
                .SetUser(userId)
                 .SetVoucherType(voucherSetingHeader.VoucherTypeId)
                .SetVoucherDetailType(voucherSetingHeader.VoucherDetailTypeId)
                .SetCompany(charterIn.Charterer.Id)
                .LocalVoucherDate()
                .FinancialVoucherDate(DateTime.Now)
                .Description(voucherSetingHeader.VoucherMainDescription)
                .ReferenceNo(issueNumber)
                .LocalVoucherNo(LocalVoucherNoGenerator(charterIn.Charterer.Code))
                .VoucherRef(voucherSetingHeader.VoucherMainRefDescription)
                .SetReferenceType(ReferenceType.CharterIn);

            voucher.IsReform = isReform;

            if (isReform)
                voucher.Description(voucherSetingHeader.VoucherMainDescription + " / سند اصلاحی");
            else
                voucher.Description(voucherSetingHeader.VoucherMainDescription);


            voucher.JournalEntrieses = new List<JournalEntry>();
            return voucher;
        }


        JournalEntry CreateDebitJournalEntry(Issue issue, VoucherSetingDetail voucherSeting, CharterIn charterIn, string lineCode, string voyageCode, bool isReform, string issueWarehouseCode, string vesselCode)
        {
            var debiJournalEntry = new JournalEntry();
            if (isReform)
            {
                debiJournalEntry = CreateReformDebitJournalEntry(issue, voucherSeting, issueWarehouseCode, vesselCode,
                    lineCode, voyageCode);
                ;
            }
            else
            {
                debiJournalEntry = CreateDebitJournalEntry(issue, voucherSeting, charterIn, lineCode, voyageCode);
                ;
            }
            return debiJournalEntry;
        }
        JournalEntry CreateReformDebitJournalEntry(Issue issue, VoucherSetingDetail voucherSeting, string issueWarehouseCode, string vesselCode, string lineCode, string voyageCode)
        {
            var creditJournalEntry = new JournalEntry();
            creditJournalEntry.Segments = new List<Segment>();
            creditJournalEntry
                  .InventoryItem(issue.InventoryItemId)
                .Typ(1)
                .IrrAmount(issue.IssueQuantity * issue.IssueFee * issue.Coefficient)
                .VoucherRef(vesselCode + lineCode + voyageCode)
                .Description(voucherSeting.VoucherCreditDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsCredit).Account.Code)
                .ForeignAmount(issue.IssueFee * issue.IssueQuantity)
            .SetCurrency(issue.CurrencyId);

            voucherSeting.CreditSegmentTypes.ForEach(c => creditJournalEntry.Segments.Add(CreateCreditSegment(
                c, issueWarehouseCode)));

            creditJournalEntry.Description(DescriptionBuilder(issue, issue.CurrencyName));
            return creditJournalEntry;
        }

        JournalEntry CreateDebitJournalEntry(Issue issue, VoucherSetingDetail voucherSeting, CharterIn charterIn, string lineCode, string voyageCode)
        {
            var debiJournalEntry = new JournalEntry();
            debiJournalEntry.Segments = new List<Segment>();
            debiJournalEntry
                   .InventoryItem(issue.InventoryItemId)
                .Typ(1)
                .IrrAmount(issue.IssueQuantity * issue.IssueFee * issue.Coefficient)
                .VoucherRef(voyageCode + "," + lineCode)
                .Description(voucherSeting.VoucherDebitDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsDebit).Account.Code)
                .ForeignAmount(issue.IssueFee * issue.IssueQuantity)
                .SetCurrency(issue.CurrencyId);


            voucherSeting.DebitSegmentTypes.ForEach(c => debiJournalEntry.Segments.Add(CreateDebitSegment(
                          c, charterIn)));


            debiJournalEntry.Description(DescriptionBuilder(issue, issue.CurrencyName));

            return debiJournalEntry;
        }


        JournalEntry CreateCreditJournalEntry(Issue issue, VoucherSetingDetail voucherSeting, string issueWarehouseCode, string vesselCode, string lineCode, string voyageCode,bool isReform,CharterIn charterIn)
        {
            var creditJournalEntry = new JournalEntry();
            if (isReform)
            {
                creditJournalEntry = CreateReformCreditJournalEntry(issue, voucherSeting, charterIn, lineCode,
                    voyageCode);
                ;
            }
            else
            {
                creditJournalEntry = CreateCreditJournalEntry(issue, voucherSeting, issueWarehouseCode, vesselCode,
                    lineCode, voyageCode);
                ;
            }
            return creditJournalEntry;
        }

        JournalEntry CreateReformCreditJournalEntry(Issue issue, VoucherSetingDetail voucherSeting, CharterIn charterIn, string lineCode, string voyageCode)
        {
            var debiJournalEntry = new JournalEntry();
            debiJournalEntry.Segments = new List<Segment>();
            debiJournalEntry
                   .InventoryItem(issue.InventoryItemId)
                .Typ(2)
                .IrrAmount(issue.IssueQuantity * issue.IssueFee * issue.Coefficient)
                .VoucherRef(voyageCode + "," + lineCode)
                .Description(voucherSeting.VoucherDebitDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsDebit).Account.Code)
                .ForeignAmount(issue.IssueFee * issue.IssueQuantity)
                .SetCurrency(issue.CurrencyId);


            voucherSeting.DebitSegmentTypes.ForEach(c => debiJournalEntry.Segments.Add(CreateDebitSegment(
                          c, charterIn)));


            debiJournalEntry.Description(DescriptionBuilder(issue, issue.CurrencyName));

            return debiJournalEntry;
        }


        JournalEntry CreateCreditJournalEntry(Issue issue, VoucherSetingDetail voucherSeting, string issueWarehouseCode, string vesselCode, string lineCode, string voyageCode)
        {
            var creditJournalEntry = new JournalEntry();
            creditJournalEntry.Segments = new List<Segment>();
            creditJournalEntry
                  .InventoryItem(issue.InventoryItemId)
                .Typ(2)
                .IrrAmount(issue.IssueQuantity * issue.IssueFee * issue.Coefficient)
                .VoucherRef(vesselCode + lineCode + voyageCode)
                .Description(voucherSeting.VoucherCreditDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsCredit).Account.Code)
                .ForeignAmount(issue.IssueFee * issue.IssueQuantity)
            .SetCurrency(issue.CurrencyId);

            voucherSeting.CreditSegmentTypes.ForEach(c => creditJournalEntry.Segments.Add(CreateCreditSegment(
                c, issueWarehouseCode)));

            creditJournalEntry.Description(DescriptionBuilder(issue, issue.CurrencyName));
            return creditJournalEntry;
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

        private Segment CreateDebitSegment(AsgnSegmentTypeVoucherSetingDetail segmentType, CharterIn charterIn)
        {
            var res = new Segment();
            res.Name = "x";
            switch (segmentType.SegmentTypeId)
            {
                //Vessel
                case 1:
                    {
                        //<A.H> Commented due to invalid implementation  :93-06-11 12:10
                        //res.Code = charterIn.Owner.Code;
                        //res.SegmentType = SegmentType.Vessel;

                        //<A.H> Code added : 93-06-11 12:10
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
                        //<A.H> Commented due to invalid implementation  :93-06-11 12:10
                        //throw new BusinessRuleException("001", "Invalid Segment Type");


                        //<A.H> Code added : 93-06-11 12:10
                        res.Code = charterIn.Owner.Code;
                        res.SegmentType = SegmentType.Company;
                    }
                    break;

            }
            return res;
        }


        string DescriptionBuilder(Issue issue, string currencyName)
        {
            var stringBuilder = new StringBuilder();
            if (isReform)
            {
                
            }
            else
            {
                stringBuilder.Append(string.Format("  {0}{1}  {2}", issue.IssueQuantity, issue.UnitName, issue.GoodName));
                stringBuilder.Append(string.Format("پایان دوره در تاریخ  : {0}  ", issue.IssueDate.ToShortDateString()));
                stringBuilder.Append(string.Format("  هر {0} ", issue.UnitName));
                stringBuilder.Append(string.Format(" {0}{1} ", issue.IssueFee, currencyName));
            }
            
            return stringBuilder.ToString();
        }


        string LocalVoucherNoGenerator(string codeCompany)
        {
            return String.Format(codeCompany +_voucherRepository.GetLocalVoucherNo());
        }



    }
}
