using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate.Rule;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public class VoucherSeting
    {
        #region Prop

        private List<IBusinessRule> _ruleSet;

        private VoucherSetingDetail _voucherSetingDetail;

        public long Id { get; private set; }

        public long? CompanyId { get; private set; }
        public virtual Company Company { get; private set; }


        public virtual VoucherDetailType VoucherDetailType { get; private set; }

        public virtual List<VoucherSetingDetail> VoucherSetingDetails { get; set; }
        public int VoucherDetailTypeId { get; set; }

        public int VoucherTypeId { get; set; }


        public string VoucherMainRefDescription { get; set; }

        public string VoucherMainDescription { get; set; }
        //public virtual List<CharterWorkflowLog> ApproveWorkflows { get; private set; }
        public byte[] TimeStamp { get; set; }

        #endregion

        #region ctor

        public VoucherSeting()
        {
            SetRule();
        }

        public VoucherSeting(
            long id,
            long companyId
            , int voucherDetailTypeId
            , int voucherTypeId,
            string voucherMainRefDescription,
            string voucherMainDescription
            )
        {

            Id = id;
            CompanyId = companyId;
            //Company=new Company(companyId,"","");
            VoucherDetailTypeId = voucherDetailTypeId;
            VoucherTypeId = voucherTypeId;
            VoucherMainRefDescription = voucherMainRefDescription;
            VoucherMainDescription = voucherMainDescription;
            SetRule();
            IsValid("Add");
        }


        void SetRule()
        {
            _voucherSetingDetail = new VoucherSetingDetail();
            _ruleSet = new List<IBusinessRule>();
            _ruleSet.Add(new HasNotHeaderType(c => c == "Add" || c == "Update", this));
            _ruleSet.Add(new NullValidationHeader(c => c == "Add" || c == "Edit", this));
            _ruleSet.Add(new IsChangeTypeOrCompany(c => c == "Update", this));
            _ruleSet.Add(new IsAlonePort(c => c == "UpdateItem" || c == "AddItem", _voucherSetingDetail));
            _ruleSet.Add(new IsChoseVoyagePortTogether(c => c == "UpdateItem" || c == "AddItem", _voucherSetingDetail));
            _ruleSet.Add(new HasSegment(c => c == "UpdateItem" || c == "AddItem", _voucherSetingDetail));
            _ruleSet.Add(new HasAccount(c => c == "UpdateItem" || c == "AddItem", _voucherSetingDetail));
            _ruleSet.Add(new IsNotChoseVoyageVessel(c => c == "UpdateItem" || c == "AddItem", _voucherSetingDetail));
        }

        void IsValid(string name)
        {
            foreach (var tuple in _ruleSet)
            {

                tuple.Validate(name);

            }
        }


        public void Update(int voucherDetailTypeId, int voucherTypeId)
        {
            IsValid("Update");

            VoucherDetailTypeId = voucherDetailTypeId;
            VoucherTypeId = voucherTypeId;

        }

        public void AddItem(VoucherSetingDetail voucherSetinDetail)
        {
            _voucherSetingDetail.SetValue(voucherSetinDetail.GoodId, voucherSetinDetail.VoucherDebitDescription, 
                voucherSetinDetail.VoucherDebitRefDescription,
                voucherSetinDetail.VoucherCreditDescription,
                voucherSetinDetail.VoucherCeditRefDescription,
                voucherSetinDetail.AsgnSegmentTypeVoucherSetingDetails,
                voucherSetinDetail.AsgnVoucherAconts);
            IsValid("AddItem");


        }

        public void DeleteItem(VoucherSetingDetail voucherSetinDetail)
        {

            IsValid("DeleteItem");
            voucherSetinDetail.IsDelete = true;
        }


        public void UpdateItem(long id, long goodId, long voucherSetingId, string voucherDebitDescription, string voucherDebitRefDescription, string voucherCreditDescription, string voucherCeditRefDescription, List<int> debitSegmentTypes, int debitAccountId, List<int> creditSegmentTypes, int creditAccountId)
        {
            var segments = new List<AsgnSegmentTypeVoucherSetingDetail>();
            segments.AddRange(CreateSegmentType(debitSegmentTypes, 1, id));
            segments.AddRange(CreateSegmentType(creditSegmentTypes, 2, id));
            _voucherSetingDetail.SetValue(goodId, voucherDebitDescription,
              voucherDebitRefDescription,
              voucherCreditDescription,
              voucherCeditRefDescription,
              segments,
              CreateAccount(debitAccountId, creditAccountId));

            IsValid("UpdateItem");

            IVoucherSetingRepository voucherSetingRepository =
                ServiceLocator.Current.GetInstance<IVoucherSetingRepository>();
            _voucherSetingDetail = voucherSetingRepository.FindByKey(voucherSetingId).VoucherSetingDetails.Find(c => c.Id == id);
           
          
          
            _voucherSetingDetail.Update(goodId, voucherDebitDescription, voucherDebitRefDescription, voucherCreditDescription, voucherCeditRefDescription, segments, CreateAccount(debitAccountId, creditAccountId)
              );

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
        #endregion
    }
}


