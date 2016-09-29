using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Presentation;
namespace MITD.Fuel.Presentation.Contracts.DTOs
{
   public  partial class JournalEntryDto
    {

      
        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        long _voucherId;
        public long VoucherId
        {
            get { return _voucherId; }
            set { this.SetField(p => p.VoucherId, ref _voucherId, value); }
        }

        long _typ;
        public long Typ
        {
            get { return _typ; }
            set { this.SetField(p => p.Typ, ref _typ, value); }
        }

       public string TypeJe {
           get
           {
               if (Typ == 1) return "بدهکار";
               else if (Typ ==2) return "بستانکار";
               else return "نا مشخص";
               
           }
       }

    
        private CurrencyDto _currencyDto;
        public CurrencyDto CurrencyDto
        {
            get { return _currencyDto; }
            set { this.SetField(p => p.CurrencyDto, ref _currencyDto, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { this.SetField(p => p.Description, ref _description, value); }
        }

        private string _segmentCode;
        public string SegmentCode
        {
            get { return _segmentCode; }
            set { this.SetField(p => p.SegmentCode, ref _segmentCode, value); }
        }
       
       

        private string _accountNo;
        public string AccountNo
        {
            get { return _accountNo; }
            set { this.SetField(p => p.AccountNo, ref _accountNo, value); }
        }

        private string _voucherRef;
        public string VoucherRef
        {
            get { return _voucherRef; }
            set { this.SetField(p => p.VoucherRef, ref _voucherRef, value); }
        }

       private decimal _foreignAmount;
       public decimal ForeignAmount
        {
            get { return _foreignAmount; }
            set { this.SetField(p => p.ForeignAmount, ref _foreignAmount, value); }
        }
       private decimal _irrAmount;
       public decimal IrrAmount
       {
           get { return _irrAmount; }
           set { this.SetField(p => p.IrrAmount, ref _irrAmount, value); }
       }
  
    
    }
}
