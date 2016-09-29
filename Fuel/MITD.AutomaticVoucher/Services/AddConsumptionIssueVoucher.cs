using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.AutomaticVoucher.Log;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.Specifications;
using ReferenceType = MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate.ReferenceType;

namespace MITD.AutomaticVoucher.Services
{
    public class AddConsumptionIssueVoucher : IAddConsumptionIssueVoucher
    {
        private Voucher _voucherx;
        private JournalEntry debiJournalEntry;
        private JournalEntry creditJournalEntry;

        #region Prop
        private IVoucherRepository _voucherRepository;
        private IVoucherSetingRepository _voucherSetingRepository;
        private readonly IInventoryOperationManager inventoryOperationManager;
        private IUnitOfWorkScope _unitOfWorkScope;

        #endregion

        public AddConsumptionIssueVoucher(IVoucherRepository voucherRepository,
            IUnitOfWorkScope unitOfWorkScope
            , IVoucherSetingRepository voucherSetingRepository, IInventoryOperationManager inventoryOperationManager)
        {
            _voucherRepository = ServiceLocator.Current.GetInstance<IVoucherRepository>(); //voucherRepository;
            //_unitOfWorkScope = unitOfWorkScope;
            _unitOfWorkScope = ServiceLocator.Current.GetInstance<IUnitOfWorkScope>();
            _voucherSetingRepository = voucherSetingRepository;
            this.inventoryOperationManager = inventoryOperationManager;
        }



        public void Execute(List<Issue> issues, FuelReport fuelReport, string issueWarehouseCode, string issueNumber, long userId)
        {
            try
            {
                var voucherSetingHeader = GetVoucherSeting(fuelReport);

                _voucherx = CreateVoucher(voucherSetingHeader, issueNumber, fuelReport, userId);

                issues.ForEach(c =>
                {
                    var voucherSeting =
                        voucherSetingHeader.VoucherSetingDetails.SingleOrDefault(d => d.GoodId == c.GoodId && !d.IsDelete);

                    debiJournalEntry = CreateDebitJournalEntry(c, voucherSeting, fuelReport, issueWarehouseCode);
                    _voucherx.JournalEntrieses.Add(debiJournalEntry);

                    creditJournalEntry = CreateCreditJournalEntry(c, voucherSeting, issueWarehouseCode, fuelReport);
                    _voucherx.JournalEntrieses.Add(creditJournalEntry);


                });

                _voucherRepository.Add(_voucherx);

                //A.H
                inventoryOperationManager.SetInventoryTransactionStatusForRegisteredVoucher(issueNumber);

                _unitOfWorkScope.Commit();

            }
            catch (Exception exp)
            {
                VoucherLogService voucherLogService = new VoucherLogService();
                voucherLogService.Add(issueNumber, "2", exp);
                //_voucherRepository.Detach(_voucherx);

                _voucherRepository.Detach();





                // var w = new List<object>();
                //_unitOfWorkScope.CurrentUnitOfWork.GetManagedEntities().ToList().ForEach(c => w.Add(c));

                //for (int i = 0; i < w.Count; i++)
                //{
                //    if (w[i] != null)
                //        if (w[i].GetType().Name == "Voucher")
                //        {

                //            ServiceLocator.Current.Release(_unitOfWorkScope.CurrentUnitOfWork.GetManagedEntities()[i]);


                //        }
                //}



                throw exp;
            }

        }

        VoucherSeting GetVoucherSeting(FuelReport fuelReport)
        {
            var voucherSetingHeader = new VoucherSeting();
            if (fuelReport.FuelReportType == FuelReportTypes.EndOfVoyage)
            {
                voucherSetingHeader = _voucherSetingRepository.Find(c => c.VoucherDetailTypeId == VoucherDetailType.EndOfVoyageFuelReport.Id
                                                  && c.CompanyId == fuelReport.VesselInCompany.CompanyId)
                     .FirstOrDefault();
            }
            else if (fuelReport.IsEndOfYearReport())
            //<A.H>  -   Following line is replaced by "IsEndOfYearReport()" implementation.
            //if (fuelReport.FuelReportType == FuelReportTypes.EndOfYear)
            {
                voucherSetingHeader =
                    _voucherSetingRepository.Find(
                        c => c.VoucherDetailTypeId == VoucherDetailType.EndOfYearFuelReport.Id
                             && c.CompanyId == fuelReport.VesselInCompany.CompanyId).FirstOrDefault();

            }
            else
            {
                //<A.H> Following line is commented and replaced by throwing an exception because there is no longer any "End of Month" report type in system.
                //voucherSetingHeader =
                //   _voucherSetingRepository.Find(
                //       c => c.VoucherDetailTypeId == VoucherDetailType.EndOfMonthFuelReport.Id
                //            && c.CompanyId == fuelReport.VesselInCompany.CompanyId).FirstOrDefault();

                //A.H added by Hatefi.
                throw new InvalidOperation("GetVoucherSeting", "GetVoucherSeting is invalid at current step.");
            }



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
            voucher.FinancialVoucherState = 2;

            voucher.JournalEntrieses = new List<JournalEntry>();
            return voucher;
        }

        JournalEntry CreateDebitJournalEntry(Issue issue, VoucherSetingDetail voucherSeting, FuelReport fuelReport, string issueWarehouseCode)
        {
            debiJournalEntry = new JournalEntry();
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

            voucherSeting.DebitSegmentTypes.ForEach(c => debiJournalEntry.Segments.Add(CreateDebitSegment(
                c, fuelReport, issueWarehouseCode)));


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
             dis
             )
             );

            return debiJournalEntry;
        }


        JournalEntry CreateCreditJournalEntry(Issue issue, VoucherSetingDetail voucherSeting, string issueWarehouseCode, FuelReport fuelReport)
        {
            creditJournalEntry = new JournalEntry();
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

            voucherSeting.CreditSegmentTypes.ForEach(c => creditJournalEntry.Segments.Add(CreateCreditSegment(
            c, issueWarehouseCode, fuelReport)));



            creditJournalEntry.Description
                (
                DescriptionBuilder
                (
                creditJournalEntry.Segments.SingleOrDefault(c => c.SegmentType.Id == 1).Code
                )
                );

            return creditJournalEntry;
        }


        private Segment CreateDebitSegment(AsgnSegmentTypeVoucherSetingDetail segmentType, FuelReport fuelReport, string issueWarehouseCode)
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

                        if (fuelReport.VesselInCompany.CompanyId == 5)
                        {
                            res.Code = "9999";
                        }
                        else if (fuelReport.VesselInCompany.CompanyId == 2)
                        {
                            res.Code = "1102";
                        }
                        else if (fuelReport.VesselInCompany.CompanyId == 1)
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


        private Segment CreateCreditSegment(AsgnSegmentTypeVoucherSetingDetail segmentType, string issueWarehouseCode, FuelReport fuelReport)
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

        string DescriptionBuilder(string code)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("MASRAF : {0} ", code));


            return stringBuilder.ToString();
        }
        string LocalVoucherNoGenerator(string codeCompany)
        {
            return String.Format(codeCompany + _voucherRepository.GetLocalVoucherNo());
        }
    }
}
