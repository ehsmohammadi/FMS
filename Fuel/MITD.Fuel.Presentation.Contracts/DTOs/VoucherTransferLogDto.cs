using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class VoucherTransferLogDto
    {

        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }


        string _financialExceptionMessage;
        public string FinancialExceptionMessage
        {
            get { return _financialExceptionMessage; }
            set { this.SetField(p => p.FinancialExceptionMessage, ref _financialExceptionMessage, value); }
        }

        
        long _userId;
        public long UserId
        {
            get { return _userId; }
            set { this.SetField(p => p.UserId, ref _userId, value); }
        }

        
        string _userName;
        public string UserName
        {
            get { return _userName; }
            set { this.SetField(p => p.UserName, ref _userName, value); }
        }



        string _voucherIds;
        public string VoucherIds
        {
            get { return _voucherIds; }
            set { this.SetField(p => p.VoucherIds, ref _voucherIds, value); }
        }

        private DateTime _sendDate;
        public DateTime SendDate
        {
            get { return _sendDate; }
            set { this.SetField(p => p.SendDate, ref _sendDate, value); }
        }



        private string _configDate;
        public string ConfigDate
        {
            get { return _configDate; }
            set { this.SetField(p => p.ConfigDate, ref _configDate, value); }
        }

        

        string _configCode;

         public string ConfigCode
        {
            get { return _configCode; }
            set { this.SetField(p => p.ConfigCode, ref _configCode, value); }
        }

       

    }
}
