using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public class Voucher
    {
        #region Prop
        public long Id { get;  set; }
        
      

        public string Description { get; set; }
        public string FinancialVoucherDate { get; set; }
        public string FinancialVoucherNo { get; set; }
        
        //1 Send
        //2 Dont send
        //3 Error
        public int? FinancialVoucherState { get; set; }

        public long? FinancialVoucherSendingUserId { get; set; }


        public DateTime LocalVoucherDate { get; set; }
        public string LocalVoucherNo { get; set; }

        public string ReferenceNo { get; set; }

        public bool  IsReform { get; set; }
        public string VoucherRef { get; set; }

        public ReferenceType ReferenceType { get; set; }

        public virtual Company  Company { get; set; }

        public long? CompanyId { get; set; }

        public int VoucherDetailTypeId { get; set; }
        public int VoucherTypeId { get; set; }

        public int ReferenceTypeId { get; set; }

        public virtual List<JournalEntry> JournalEntrieses { get; set; }

        public virtual FuelUser User { get; set; }

        public long UserId { get; set; }

        public byte[] TimeStamp { get; set; }
        #endregion


        #region ctor

        public Voucher()
        {
            FinancialVoucherState = 2;
        }

        public Voucher(
            long id
            //, long currencyId, 
            ,string description, DateTime financialVoucherDate,
            DateTime localVoucherDate, string localVoucherNo,
            string referenceNo, string voucherRef, int referenceTypeId,
            List<JournalEntry> journalEntrieses,long companyId
            ,int voucherDetailTypeId,int voucherTypeId,long userId
            )
        {
            Id = id;
            Description = description;
            FinancialVoucherDate = financialVoucherDate.ToString();
            LocalVoucherDate = localVoucherDate;
            LocalVoucherNo = localVoucherNo;
            ReferenceNo = referenceNo;
            VoucherRef = voucherRef;
            ReferenceTypeId = referenceTypeId;
            JournalEntrieses = journalEntrieses;
            CompanyId = companyId;
            VoucherDetailTypeId = voucherDetailTypeId;
            VoucherTypeId = voucherTypeId;
            UserId = userId;
            FinancialVoucherState = 2;

        }
        #endregion


    }
}
