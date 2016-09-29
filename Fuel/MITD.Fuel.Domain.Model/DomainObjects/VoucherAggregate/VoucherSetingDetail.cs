using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iesi.Collections;
using MITD.Core;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public class VoucherSetingDetail
    {
        #region Prop

        private IVoucherSetingRepository _voucherSetingRepository;
        
        public long Id { get; private set; }
        public long GoodId { get; private set; }

        public virtual long VoucherSetingId { get; private set; }

        public bool IsDelete { get; set; }
        public virtual Good Good { get; set; }
        public virtual VoucherSeting VoucherSeting { get; private set; }
        public string VoucherDebitDescription { get; set; }
        public string VoucherDebitRefDescription { get; set; }
        public string VoucherCreditDescription { get; set; }

        public string VoucherCeditRefDescription { get; set; }

        public virtual List<AsgnVoucherAcont> AsgnVoucherAconts { get; set; }



        public virtual List<AsgnSegmentTypeVoucherSetingDetail> AsgnSegmentTypeVoucherSetingDetails { get; set; }
        public List<AsgnSegmentTypeVoucherSetingDetail> DebitSegmentTypes
        {
            get
            {
               
                return AsgnSegmentTypeVoucherSetingDetails.Where(c => c.IsDebit).Select(d=>d).ToList();
            }
        }

        public List<AsgnSegmentTypeVoucherSetingDetail> CreditSegmentTypes
        {
            get
            {
                return AsgnSegmentTypeVoucherSetingDetails.Where(c => c.IsCredit).Select(d => d).ToList();
            }
        }
        #endregion


        #region ctor


        public VoucherSetingDetail(
            long id,
            long goodId,
            long voucherSetingId,
            string voucherDebitDescription,
            string voucherDebitRefDescription,
            string voucherCreditDescription,
            string voucherCeditRefDescription,
             List<AsgnSegmentTypeVoucherSetingDetail> asgnSegmentTypeVoucherSetingDetails,
             List<AsgnVoucherAcont> asgnVoucherAconts,
            IVoucherSetingRepository voucherSetingRepository

            )
        {
            Id = id;
            GoodId = goodId;
            VoucherSetingId = voucherSetingId;
            VoucherDebitDescription = voucherDebitDescription;
            VoucherDebitRefDescription = voucherDebitRefDescription;
            VoucherCreditDescription = voucherCreditDescription;
            VoucherCeditRefDescription = voucherCeditRefDescription;
            AsgnVoucherAconts = asgnVoucherAconts;
            AsgnSegmentTypeVoucherSetingDetails = asgnSegmentTypeVoucherSetingDetails;
            _voucherSetingRepository = voucherSetingRepository;
        }



        public VoucherSetingDetail() { }




        public void SetValue(long goodId,
            string voucherDebitDescription,
            string voucherDebitRefDescription,
            string voucherCreditDescription,
            string voucherCeditRefDescription,
             List<AsgnSegmentTypeVoucherSetingDetail> asgnSegmentTypeVoucherSetingDetails,
             List<AsgnVoucherAcont> asgnVoucherAconts)
        {
            GoodId = goodId;
            VoucherDebitDescription = voucherDebitDescription;
            VoucherDebitRefDescription = voucherDebitRefDescription;
            VoucherCreditDescription = voucherCreditDescription;
            VoucherCeditRefDescription = voucherCeditRefDescription;
            AsgnSegmentTypeVoucherSetingDetails = asgnSegmentTypeVoucherSetingDetails;
            AsgnVoucherAconts = asgnVoucherAconts;
        }

        #endregion


        #region Method

        public void Update(
            long goodId,
            string voucherDebitDescription,
            string voucherDebitRefDescription,
            string voucherCreditDescription,
            string voucherCeditRefDescription,
             List<AsgnSegmentTypeVoucherSetingDetail> asgnSegmentTypeVoucherSetingDetails,
             List<AsgnVoucherAcont> asgnVoucherAconts)
        {

            _voucherSetingRepository=ServiceLocator.Current.GetInstance<IVoucherSetingRepository>();
            
            GoodId = goodId;
            VoucherDebitDescription = voucherDebitDescription;
            VoucherDebitRefDescription = voucherDebitRefDescription;
            VoucherCreditDescription = voucherCreditDescription;
            VoucherCeditRefDescription = voucherCeditRefDescription;
           

            asgnVoucherAconts.ForEach(c =>
                                      {
                                          var res = AsgnVoucherAconts.Single(d=>d.Typ==c.Typ);

                                          if (res!=null)
                                          {
                                              res.AccountId = c.AccountId;
                                          }
                                              
                                          

                                      });

            for (int i = 0; i < AsgnSegmentTypeVoucherSetingDetails.Count ; i++)
            {
                if (asgnSegmentTypeVoucherSetingDetails.SingleOrDefault(c =>
                    c.SegmentTypeId == AsgnSegmentTypeVoucherSetingDetails[i].SegmentTypeId &&
                    c.Typ == AsgnSegmentTypeVoucherSetingDetails[i].Typ
                    ) == null)
                {

                    //AsgnSegmentTypeVoucherSetingDetails.Remove(AsgnSegmentTypeVoucherSetingDetails[i]);
                    _voucherSetingRepository.Deletex(AsgnSegmentTypeVoucherSetingDetails[i].Id);
                    i--;
                }
            }

            for (int i = 0; i < asgnSegmentTypeVoucherSetingDetails.Count - 1; i++)
            {
                if (AsgnSegmentTypeVoucherSetingDetails.SingleOrDefault(c =>
                    c.SegmentTypeId == asgnSegmentTypeVoucherSetingDetails[i].SegmentTypeId &&
                    c.Typ == asgnSegmentTypeVoucherSetingDetails[i].Typ
                    ) == null)
                {
                    AsgnSegmentTypeVoucherSetingDetails.Add(asgnSegmentTypeVoucherSetingDetails[i]);
                }
            }



        }

        #endregion


    }
}
