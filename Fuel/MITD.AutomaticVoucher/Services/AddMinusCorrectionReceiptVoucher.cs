﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.AutomaticVoucher.Log;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.AutomaticVoucher.Services
{
    public class AddMinusCorrectionReceiptVoucher : IAddMinusCorrectionReceiptVoucher
    {
        #region Prop
        private readonly IVoucherRepository _voucherRepository;
        private readonly IVoucherSetingRepository _voucherSetingRepository;
        private readonly IInventoryOperationManager inventoryOperationManager;
        private IUnitOfWorkScope _unitOfWorkScope;

        #endregion
        public AddMinusCorrectionReceiptVoucher(IVoucherRepository voucherRepository,
            IUnitOfWorkScope unitOfWorkScope
            , IVoucherSetingRepository voucherSetingRepository, IInventoryOperationManager inventoryOperationManager)
        {
            _voucherRepository = voucherRepository;
            _unitOfWorkScope = unitOfWorkScope;
            _voucherSetingRepository = voucherSetingRepository;
            this.inventoryOperationManager = inventoryOperationManager;
        }



        public void Execute(FuelReport fuelReport, List<Issue> issues,
            string issueWarehouseCode, string issueNumber, long userId)
        {
            try
            {
                var voucherSetingHeader = GetVoucherSeting(fuelReport);

                var voucher = CreateVoucher(voucherSetingHeader, issueNumber, fuelReport, userId);

                issues.ForEach(c =>
                {
                    var voucherSeting =
                        voucherSetingHeader.VoucherSetingDetails.SingleOrDefault(d => d.GoodId == c.GoodId && !d.IsDelete);

                    var debiJournalEntry = CreateDebitJournalEntry(c, voucherSeting, fuelReport, issueWarehouseCode);
                    voucher.JournalEntrieses.Add(debiJournalEntry);

                    var creditJournalEntry = CreateCreditJournalEntry(c, voucherSeting, issueWarehouseCode, fuelReport);
                    voucher.JournalEntrieses.Add(creditJournalEntry);


                });

                _voucherRepository.Add(voucher);
               
                //<A.H>
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
            var voucherSetingHeader = new VoucherSeting();
            voucherSetingHeader = _voucherSetingRepository.Find(c => c.VoucherDetailTypeId == VoucherDetailType.PlusCorrection.Id
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
                .SetReferenceType(ReferenceType.FuelReport);

            voucher.JournalEntrieses = new List<JournalEntry>();
            return voucher;
        }
        JournalEntry CreateDebitJournalEntry(Issue issue, VoucherSetingDetail voucherSeting, FuelReport fuelReport,
            string receiptWarehouseCode)
        {
            var debiJournalEntry = new JournalEntry();
            debiJournalEntry.Segments = new List<Segment>();
            debiJournalEntry
                   .InventoryItem(issue.InventoryItemId)
                .Typ(1)
                .IrrAmount(issue.IssueQuantity * issue.IssueFee * issue.Coefficient)
                 .VoucherRef(fuelReport.VesselInCompany.Vessel.Code + fuelReport.Voyage.VoyageNumber)
                .Description(voucherSeting.VoucherDebitDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsDebit).Account.Code)
                .ForeignAmount(issue.IssueQuantity * issue.IssueFee)
                .SetCurrency(issue.CurrencyId);

            voucherSeting.DebitSegmentTypes.ForEach(c => debiJournalEntry.Segments.Add(CreateDebitSegment(
                c, fuelReport, receiptWarehouseCode)));


            debiJournalEntry.Description
                          (
                          DescriptionBuilder
                          (
                          issue, fuelReport, true
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
           .VoucherRef(fuelReport.VesselInCompany.Vessel.Code + fuelReport.Voyage.VoyageNumber)
                .Description(voucherSeting.VoucherDebitDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsCredit).Account.Code)
                .ForeignAmount(issue.IssueQuantity * issue.IssueFee)
                .SetCurrency(issue.CurrencyId);

            voucherSeting.CreditSegmentTypes.ForEach(c => creditJournalEntry.Segments.Add(CreateCreditSegment(
            c, issueWarehouseCode)));


            creditJournalEntry.Description
                         (
                         DescriptionBuilder
                         (
                         issue, fuelReport, false
                         )
                         );


            return creditJournalEntry;
        }


        private Segment CreateDebitSegment(AsgnSegmentTypeVoucherSetingDetail segmentType, FuelReport fuelReport, string receiptWarehouseCode)
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

                        if (fuelReport.VesselInCompany.CompanyId == 5)
                        {
                            res.Code = "9999";
                        }
                        else if (fuelReport.VesselInCompany.CompanyId == 2)
                        {
                            res.Code = "1102";
                        }
                    }
                    break;
                //Voayage
                case 3:
                    {
                        res.Code = fuelReport.Voyage.VoyageNumber;
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


        private Segment CreateCreditSegment(AsgnSegmentTypeVoucherSetingDetail segmentType, string receiptWarehouseCode)
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

        string DescriptionBuilder(Issue issue, FuelReport fuelReport, bool isDebit)
        {
            var stringBuilder = new StringBuilder();


            fuelReport.FuelReportDetails.ToList().ForEach(c =>
            {
                if (c.GoodId == issue.GoodId)
                {
                    if (c.CorrectionReference.IsEmpty() &&
                        c.CorrectionReference.ReferenceType != MITD.Fuel.Domain.Model.Enums.ReferenceType.Voyage)
                    {

                        if (c.CorrectionPrice == null)
                        {
                            stringBuilder.Append(
                                String.Format("اضافات و کسورات انبارگردانی : {0} {1} {2} با فی آخرین رسید خرید : {3}", issue.IssueQuantity, issue.UnitName, issue.GoodName, issue.IssueFee));
                        }
                        else
                        {
                            stringBuilder.Append(
                                  String.Format("اضافات و کسورات انبارگردانی : {0} {1} {2} با فی  : {3}", issue.IssueQuantity, issue.UnitName, issue.GoodName, issue.IssueFee));

                        }

                    }
                    else
                    {
                        stringBuilder.Append(isDebit
                            ? String.Format("{0} : تعدیل سفر", fuelReport.VesselInCompany.Vessel.Code)
                            : String.Format("{0} {1}  : تعدیل سفر", c.CorrectionReference.Code,
                                fuelReport.VesselInCompany.Vessel.Code));
                    }
                }

            });


            return stringBuilder.ToString();
        }

        string LocalVoucherNoGenerator(string codeCompany)
        {
            return String.Format(codeCompany + _voucherRepository.GetLocalVoucherNo());
        }
    }
}
