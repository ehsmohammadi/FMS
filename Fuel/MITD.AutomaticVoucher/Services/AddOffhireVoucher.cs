using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.AutomaticVoucher.Log;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.AutomaticVoucher.Services
{
    public class AddOffhireVoucher : IAddOffhireVoucher
    {

              #region Prop
        private readonly IVoucherRepository _voucherRepository;
        private readonly IVoucherSetingRepository _voucherSetingRepository;
        private IUnitOfWorkScope _unitOfWorkScope;

        #endregion

        public AddOffhireVoucher(IVoucherRepository voucherRepository,
            IUnitOfWorkScope unitOfWorkScope
            , IVoucherSetingRepository voucherSetingRepository)
        {
            _voucherRepository = voucherRepository;
            _unitOfWorkScope = unitOfWorkScope;
            _voucherSetingRepository = voucherSetingRepository;
        }

        public void Execute(Offhire offhire, long userId,VoucherDetailType voucherDetailType)
        {
            try
            {
                var voucherSetingHeader = GetVoucherSeting(offhire,voucherDetailType);

                var voucher = CreateVoucher(voucherSetingHeader, offhire, userId);

                offhire.OffhireDetails.ForEach(c =>
                {
                    var voucherSeting =
                        voucherSetingHeader.VoucherSetingDetails.SingleOrDefault(d => d.GoodId == c.GoodId && !d.IsDelete);

                    var debiJournalEntry = CreateDebitJournalEntry(offhire, c, voucherSeting, voucherDetailType);
                    voucher.JournalEntrieses.Add(debiJournalEntry);

                    var creditJournalEntry = CreateCreditJournalEntry(offhire, c, voucherSeting, voucherDetailType);
                    voucher.JournalEntrieses.Add(creditJournalEntry);


                });


                _voucherRepository.Add(voucher);
                _unitOfWorkScope.Commit();
            }
            catch (Exception exp)
            {

                VoucherLogService voucherLogService = new VoucherLogService();
                voucherLogService.Add(offhire.ReferenceNumber.ToString(), "OffHire", exp);
                throw exp;


            } 
            
           
        }

        VoucherSeting GetVoucherSeting(Offhire offhire,VoucherDetailType voucherDetailType)
        {
            var voucherSetingHeader =
                _voucherSetingRepository.Find(c => c.VoucherDetailTypeId == voucherDetailType.Id
                                                 && c.CompanyId == offhire.VesselInCompany.CompanyId)
                    .FirstOrDefault();

            return voucherSetingHeader;
        }
        Voucher CreateVoucher(VoucherSeting voucherSetingHeader, Offhire offhire, long userId)
        {
            var voucher = new Voucher();
            voucher
                .SetUser(userId)
                 .SetVoucherType(voucherSetingHeader.VoucherTypeId)
                .SetVoucherDetailType(voucherSetingHeader.VoucherDetailTypeId)
                .SetCompany(offhire.VesselInCompany.CompanyId)
                .LocalVoucherDate()
                .FinancialVoucherDate(offhire.VoucherDate)
                .Description(voucherSetingHeader.VoucherMainDescription)
                .ReferenceNo(offhire.ReferenceNumber.ToString())
                     .LocalVoucherNo(LocalVoucherNoGenerator(offhire.VesselInCompany.Company.Code))
                .VoucherRef(voucherSetingHeader.VoucherMainRefDescription)
                .SetReferenceType(ReferenceType.Offhire);


            voucher.JournalEntrieses = new List<JournalEntry>();
            return voucher;
        }


        JournalEntry CreateDebitJournalEntry(Offhire offhire, OffhireDetail offhireDetail, VoucherSetingDetail voucherSeting, VoucherDetailType voucherDetailType)
        {
            var debiJournalEntry = new JournalEntry();
            debiJournalEntry.Segments = new List<Segment>();
            debiJournalEntry
                .Typ(1)
                .IrrAmount(offhireDetail.Quantity * offhireDetail.FeeInMainCurrency)
                .VoucherRef(offhire.Voyage.VoyageNumber+offhire.VesselInCompany.Vessel.Code)
                .Description(voucherSeting.VoucherDebitDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsDebit).Account.Code)
                .ForeignAmount(offhireDetail.Quantity * offhireDetail.FeeInVoucherCurrency)
            .SetCurrency(offhire.VoucherCurrency.Id);

            if (voucherDetailType.Id == 10)
            {

                voucherSeting.DebitSegmentTypes.ForEach(c => debiJournalEntry.Segments.Add(CreateDebitSegment(
                 c, offhire)));
            }
            else if (voucherDetailType.Id == 9)
            {
                voucherSeting.DebitSegmentTypes.ForEach(c => debiJournalEntry.Segments.Add(CreateCreditSegment(
                c, offhire)));
            }


            debiJournalEntry.Description(DescriptionBuilder(offhire, offhireDetail));
            return debiJournalEntry;
        }

        JournalEntry CreateCreditJournalEntry(Offhire offhire, OffhireDetail offhireDetail, VoucherSetingDetail voucherSeting,VoucherDetailType voucherDetailType)
        {
            var creditJournalEntry = new JournalEntry();
            creditJournalEntry.Segments = new List<Segment>();
            creditJournalEntry
                .Typ(2)
                .IrrAmount(offhireDetail.Quantity * offhireDetail.FeeInMainCurrency)
                .VoucherRef(offhire.Voyage.VoyageNumber + offhire.VesselInCompany.Vessel.Code)
                .Description(voucherSeting.VoucherCreditDescription)
                .AccountNo(voucherSeting.AsgnVoucherAconts.Single(d => d.IsCredit).Account.Code)
                .ForeignAmount(offhireDetail.Quantity * offhireDetail.FeeInVoucherCurrency)
                .SetCurrency(offhire.VoucherCurrency.Id);

            if (voucherDetailType.Id==10)
            {

                voucherSeting.CreditSegmentTypes.ForEach(c => creditJournalEntry.Segments.Add(CreateCreditSegment(
                 c, offhire)));
            }
            else if (voucherDetailType.Id == 9)
            {
                voucherSeting.CreditSegmentTypes.ForEach(c => creditJournalEntry.Segments.Add(CreateDebitSegment(
                c, offhire)));
            }


            creditJournalEntry.Description(DescriptionBuilder(offhire, offhireDetail));
            return creditJournalEntry;
        }



        private Segment CreateDebitSegment(AsgnSegmentTypeVoucherSetingDetail segmentType, Offhire offhire)
        {
            var res = new Segment();
            res.Name = "x";
           
            
            switch (segmentType.SegmentTypeId)
            {
                //Vessel
                case 1:
                    {
                        res.Code = offhire.Voyage.VesselInCompany.Code;
                        res.SegmentType = SegmentType.Vessel;
                    }
                    break;
                //Port
                case 2:
                {
                    res.SegmentType = SegmentType.Port;

                    if (offhire.VesselInCompany.CompanyId == 5)
                    {
                        res.Code = "9999";
                    }
                    else if (offhire.VesselInCompany.CompanyId == 2)
                    {
                        res.Code = "1102";
                    }
                   
                }
                    break;
                //Voayage
                case 3:
                    {
                        res.Code = offhire.Voyage.VoyageNumber;
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

        private Segment CreateCreditSegment(AsgnSegmentTypeVoucherSetingDetail segmentType, Offhire offhire)
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
                        res.Code =offhire.VesselInCompany.Company.Code ;
                        res.SegmentType = SegmentType.Company;
                    }
                    break;

            }
            return res;
        }

        string DescriptionBuilder(Offhire offhire,OffhireDetail offhireDetail)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("  {0}{1} {2}  ", offhireDetail.Quantity, offhireDetail.Unit.Name, offhireDetail.Good.Name));
            stringBuilder.Append(string.Format("اف هایر در   : {0}  ", offhire.StartDateTime.ToShortDateString()));
            stringBuilder.Append(string.Format("  هر {0} ", offhireDetail.Unit.Name));
            stringBuilder.Append(string.Format(" {0}{1} ", offhireDetail.FeeInVoucherCurrency, offhire.VoucherCurrency.Name));

            return stringBuilder.ToString();
        }

        string LocalVoucherNoGenerator(string codeCompany)
        {
            return String.Format(codeCompany + _voucherRepository.GetLocalVoucherNo());
        }
    }
}
