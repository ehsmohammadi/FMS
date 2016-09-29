using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Application.Service
{
    public class VoucherSetingApplicationService : IVoucherSetingApplicationService
    {

        private IVoucherSetingRepository _voucherSetingRepository;
        private IUnitOfWorkScope _unitOfWorkScope;

        public VoucherSetingApplicationService(
                                               IUnitOfWorkScope unitOfWorkScope,
                                               IVoucherSetingRepository voucherSetingRepository)
        {
            this._unitOfWorkScope = unitOfWorkScope;
            this._voucherSetingRepository = voucherSetingRepository;
        }

        public void AddVoucherSeting(long companyId, int voucherDetailTypeId, int voucherTypeId, string voucherMainRefDescription, string voucherMainDescription)
        {
            var voucherSeting =
                new VoucherSeting(0, companyId, voucherDetailTypeId, voucherTypeId, voucherMainRefDescription, voucherMainDescription);
            _voucherSetingRepository.Add(voucherSeting);
            _unitOfWorkScope.Commit();

        }

        public void UpdateVoucherSeting(long id, long companyId, int voucherDetailTypeId, int voucherTypeId, string voucherMainRefDescription, string voucherMainDescription)
        {
            var voucherseting = _voucherSetingRepository.FindByKey(id);
            voucherseting.Update(voucherDetailTypeId, voucherTypeId);
            _unitOfWorkScope.Commit();
        }





        public void AddVoucherSetingDetail(long goodId, long voucherSetingId, string voucherDebitDescription, string voucherDebitRefDescription, string voucherCreditDescription, string voucherCeditRefDescription,
           List<int> debitSegmentTypes, int debitAccountId, List<int> creditSegmentTypes, int creditAccountId)
        {
            var voucherSetingDetail = VoucherSetingDetailFactory(0,goodId, voucherSetingId, debitSegmentTypes, debitAccountId, creditSegmentTypes,
                  creditAccountId);

            var voucher = _voucherSetingRepository.FindByKey(voucherSetingId);
            voucher.AddItem(voucherSetingDetail);

            voucher.VoucherSetingDetails.Add(voucherSetingDetail);
            _unitOfWorkScope.Commit();

        }

        VoucherSetingDetail VoucherSetingDetailFactory(long id,long goodId, long voucherSetingId, List<int> debitSegmentTypes, int debitAccountId, List<int> creditSegmentTypes, int creditAccountId)
        {
            var segments = new List<AsgnSegmentTypeVoucherSetingDetail>();
           
            segments.AddRange(CreateSegmentType(debitSegmentTypes,1,id));
            segments.AddRange(CreateSegmentType(creditSegmentTypes, 2,id));

            var accounts = CreateAccount(debitAccountId, creditAccountId);
            
            return  new VoucherSetingDetail(0, goodId, voucherSetingId, "", "", "", "", segments,
                accounts, _voucherSetingRepository);
        }


        List<AsgnVoucherAcont> CreateAccount(int debitAccountId, int creditAccountId)
        {

            return new List<AsgnVoucherAcont>()
                           {
                               new AsgnVoucherAcont(0,1,debitAccountId,0),
                                new AsgnVoucherAcont(0,2,creditAccountId,0)
                               
                           };

        }



        List<AsgnSegmentTypeVoucherSetingDetail> CreateSegmentType(List<int> list, int typ, long voucherSetingDetailId)
        {
            var segments = new List<AsgnSegmentTypeVoucherSetingDetail>();
            list.ForEach(c =>
            {
                var item = new AsgnSegmentTypeVoucherSetingDetail()
                {
                    SegmentTypeId = c,
                    Typ = typ,
                    VoucherSetingDetailId = voucherSetingDetailId

                };
                segments.Add(item);
            });

            return segments;
        }

        public void UpdateVoucherSetingDetail(long id, long goodId, long voucherSetingId, string voucherDebitDescription, string voucherDebitRefDescription, string voucherCreditDescription, string voucherCeditRefDescription, List<int> debitSegmentTypes, int debitAccountId, List<int> creditSegmentTypes, int creditAccountId)
        {
            var voucher = _voucherSetingRepository.FindByKey(voucherSetingId);

            voucher.UpdateItem(id, goodId, voucherSetingId, voucherDebitDescription, voucherDebitRefDescription,
                voucherCreditDescription, voucherCeditRefDescription, debitSegmentTypes, debitAccountId,
                creditSegmentTypes, creditAccountId);

            _unitOfWorkScope.Commit();

        }

        public void UpdateDelete(long voucherSetingId, long voucherSetingDetailId)
        {
            var voucher = _voucherSetingRepository.FindByKey(voucherSetingId);
            var voucherSetinDetail = voucher.VoucherSetingDetails.Find(c => c.Id == voucherSetingDetailId);
            voucher.DeleteItem(voucherSetinDetail);
            _unitOfWorkScope.Commit();
        }

    }
}
