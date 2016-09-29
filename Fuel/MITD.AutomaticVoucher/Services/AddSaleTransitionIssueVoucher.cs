using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.AutomaticVoucher.Log;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using ReferenceType = MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate.ReferenceType;

namespace MITD.AutomaticVoucher.Services
{
    
    public class AddSaleTransitionIssueVoucher : IAddSaleTransitionIssueVoucher
    {
        #region Prop
        private readonly IVoucherRepository _voucherRepository;
        private readonly IVoucherSetingRepository _voucherSetingRepository;
        private readonly IInventoryOperationManager inventoryOperationManager;
        private IUnitOfWorkScope _unitOfWorkScope;

        #endregion

        public AddSaleTransitionIssueVoucher(IVoucherRepository voucherRepository,
            IUnitOfWorkScope unitOfWorkScope
            , IVoucherSetingRepository voucherSetingRepository, IInventoryOperationManager inventoryOperationManager)
        {
            _voucherRepository = voucherRepository;
            _unitOfWorkScope = unitOfWorkScope;
            _voucherSetingRepository = voucherSetingRepository;
            this.inventoryOperationManager = inventoryOperationManager;
        }

        public void Execute(List<Issue> issues, FuelReport fuelReport, string issueWarehouseCodeSupplier,string issueWarehouseCodeReciver, string issueNumber, long userId)
        {
            try
            {
                var voucherSetingHeader = GetVoucherSeting(fuelReport);

                var voucher = CreateVoucher(voucherSetingHeader, issueNumber, fuelReport, userId);

                issues.ForEach(c =>
                {
                    var voucherSeting =
                        voucherSetingHeader.VoucherSetingDetails.SingleOrDefault(d => d.GoodId == c.GoodId && !d.IsDelete);

                    var debiJournalEntry = CreateDebitJournalEntry(c, voucherSeting, fuelReport, issueWarehouseCodeSupplier);
                    voucher.JournalEntrieses.Add(debiJournalEntry);

                    var creditJournalEntry = CreateCreditJournalEntry(c, voucherSeting, issueWarehouseCodeReciver, fuelReport);
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



        VoucherSeting GetVoucherSeting(FuelReport fuelReport)
        {
            var voucherSetingHeader =
                 _voucherSetingRepository.Find(c => c.VoucherDetailTypeId == VoucherDetailType.Transfer.Id
                                                  && c.CompanyId == fuelReport.VesselInCompany.CompanyId)
                     .FirstOrDefault();

            return voucherSetingHeader;
        }
        Voucher CreateVoucher(VoucherSeting voucherSetingHeader, string issueNumber, FuelReport fuelReport, long userId)
        {
            var voucher = new Voucher();
            voucher
                .SetUser(userId)
                .SetVoucherType(voucherSetingHeader.VoucherTypeId)
                .SetVoucherDetailType(voucherSetingHeader.VoucherDetailTypeId)
                .SetCompany(fuelReport.VesselInCompany.CompanyId)
                .LocalVoucherDate()
                .FinancialVoucherDate(DateTime.Now)
                .Description(voucherSetingHeader.VoucherMainDescription)
                .ReferenceNo(issueNumber)
                   .LocalVoucherNo(LocalVoucherNoGenerator(fuelReport.VesselInCompany.Company.Code))
                .VoucherRef(voucherSetingHeader.VoucherMainRefDescription)
                .SetReferenceType(ReferenceType.Invoice);


            voucher.JournalEntrieses = new List<JournalEntry>();
            return voucher;
        }
        JournalEntry CreateDebitJournalEntry(Issue issue, VoucherSetingDetail voucherSeting, FuelReport fuelReport, string issueWarehouseCode)
        {
            var debiJournalEntry = new JournalEntry();
            debiJournalEntry.Segments = new List<Segment>();
            debiJournalEntry
                .InventoryItem(issue.InventoryItemId)
                .Typ(1)
                .IrrAmount(issue.IssueQuantity * issue.IssueFee * issue.Coefficient)
                .VoucherRef(fuelReport.Voyage.VoyageNumber + fuelReport.VesselInCompany.Vessel.Code)
                .Description(voucherSeting.VoucherDebitDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsDebit).Account.Code)
                .ForeignAmount(issue.IssueFee * issue.IssueQuantity)
                .SetCurrency(issue.CurrencyId);

            voucherSeting.DebitSegmentTypes.ForEach(c => debiJournalEntry.Segments.Add(CreateSegment(
                c, issueWarehouseCode)));


            string dis = "";
            debiJournalEntry.Segments.Where(c => c.SegmentType.Id == 1 || c.SegmentType.Id == 2 || c.SegmentType.Id == 3)
                .ToList()
                .ForEach(
                    c =>
                    {
                        dis += c.Code;
                    });
            debiJournalEntry.Description
                (
                DescriptionBuilder
                (
                issue
                )
                );

            return debiJournalEntry;
        }
        JournalEntry CreateCreditJournalEntry(Issue issue, VoucherSetingDetail voucherSeting, string issueWarehouseCode, FuelReport fuelReport)
        {
            var creditJournalEntry = new JournalEntry();
            creditJournalEntry.Segments = new List<Segment>();
            creditJournalEntry
                 .InventoryItem(issue.InventoryItemId)
                .Typ(2)
                .IrrAmount(issue.IssueQuantity * issue.IssueFee * issue.Coefficient)
                 .VoucherRef(fuelReport.Voyage.VoyageNumber + fuelReport.VesselInCompany.Vessel.Code)
                .Description(voucherSeting.VoucherDebitDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsCredit).Account.Code)
                .ForeignAmount(issue.IssueFee * issue.IssueQuantity)
                .SetCurrency(issue.CurrencyId);

            voucherSeting.CreditSegmentTypes.ForEach(c => creditJournalEntry.Segments.Add(CreateSegment(
            c, issueWarehouseCode)));



            creditJournalEntry.Description
                (
                DescriptionBuilder
                (
                issue
                )
                );

            return creditJournalEntry;
        }

        private Segment CreateSegment(AsgnSegmentTypeVoucherSetingDetail segmentType, string issueWarehouseCode)
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
        
        string DescriptionBuilder(Issue issue)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(String.Format("انتقال  {0} {1}  {2} درتاریخ : {3} بافی : {4} {5}",
              issue.IssueQuantity, issue.UnitName, issue.GoodName, issue.IssueDate, issue.IssueFee, issue.CurrencyName));


            return stringBuilder.ToString();
        }
        string LocalVoucherNoGenerator(string codeCompany)
        {
            return String.Format(codeCompany + _voucherRepository.GetLocalVoucherNo());
        }

    }

    
}
