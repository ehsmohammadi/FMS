using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Presentation;
namespace MITD.Fuel.Presentation.Contracts.DTOs
{
  public partial  class VoucherSetingDetailDto
    {


        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        private GoodDto _goodDto;
        public GoodDto GoodDto
        {
            get { return _goodDto; }
            set { this.SetField(p => p.GoodDto, ref _goodDto, value); }
        }

        long _voucherSetingId;
        public long VoucherSetingId
        {
            get { return _voucherSetingId; }
            set { this.SetField(p => p.VoucherSetingId, ref _voucherSetingId, value); }
        }


      
        string _voucherDebitDescription;
        public string VoucherDebitDescription
        {
            get { return _voucherDebitDescription; }
            set { this.SetField(p => p.VoucherDebitDescription, ref _voucherDebitDescription, value); }
        }

        string _voucherDebitRefDescription;
        public string VoucherDebitRefDescription
        {
            get { return _voucherDebitRefDescription; }
            set { this.SetField(p => p.VoucherDebitRefDescription, ref _voucherDebitRefDescription, value); }
        }

        string _voucherCreditDescription;
        public string VoucherCreditDescription
        {
            get { return _voucherCreditDescription; }
            set { this.SetField(p => p.VoucherCreditDescription, ref _voucherCreditDescription, value); }
        }


        string _voucherCeditRefDescription;
        public string VoucherCeditRefDescription
        {
            get { return _voucherCeditRefDescription; }
            set { this.SetField(p => p.VoucherCeditRefDescription, ref _voucherCeditRefDescription, value); }
        }



        AccountDto _debitAccountDto;
        public  AccountDto DebitAccountDto
        {
            get { return _debitAccountDto; }
            set { this.SetField(p => p.DebitAccountDto, ref _debitAccountDto, value); }
        }
        AccountDto _creditAccountDto;
        public AccountDto CreditAccountDto
        {
            get { return _creditAccountDto; }
            set { this.SetField(p => p.CreditAccountDto, ref _creditAccountDto, value); }
        }


        List<int> _debitSegmentTypes;
       public List<int> DebitSegmentTypes
        {
            get { return _debitSegmentTypes; }
            set { this.SetField(p => p.DebitSegmentTypes, ref _debitSegmentTypes, value); }
        }

       List<int> _creditSegmentTypes;
       public List<int> CreditSegmentTypes
       {
           get { return _creditSegmentTypes; }
           set { this.SetField(p => p.CreditSegmentTypes, ref _creditSegmentTypes, value); }
       }

       

    }
}
