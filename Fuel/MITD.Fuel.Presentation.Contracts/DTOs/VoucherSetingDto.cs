using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Presentation;
namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class VoucherSetingDto
    {
        
        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }
        
        private CompanyDto _company;
        public CompanyDto Company
        {
            get { return _company; }
            set { this.SetField(p => p.Company, ref _company, value); }
        }


        string _voucherMainDescription;
        public string VoucherMainDescription
        {
            get { return _voucherMainDescription; }
            set { this.SetField(p => p.VoucherMainDescription, ref _voucherMainDescription, value); }
        }


        string _voucherMainRefDescription;
        public string VoucherMainRefDescription
        {
            get { return _voucherMainDescription; }
            set { this.SetField(p => p.VoucherMainRefDescription, ref _voucherMainRefDescription, value); }
        }

        int _voucherDetailTypeId;
        public int VoucherDetailTypeId
        {
            get { return _voucherDetailTypeId; }
            set { this.SetField(p => p.VoucherDetailTypeId, ref _voucherDetailTypeId, value); }
        }

        int _voucherTypeId;
        public int VoucherTypeId
        {
            get { return _voucherTypeId; }
            set { this.SetField(p => p.VoucherTypeId, ref _voucherTypeId, value); }
        }


        string _voucherTypeName;
        public string VoucherTypeName
        {
            get { return _voucherTypeName; }
            set { this.SetField(p => p.VoucherTypeName, ref _voucherTypeName, value); }
        }

      

        public virtual List<VoucherSetingDetailDto> VoucherSetingDetails { get; set; }
        public virtual List<VoucherSetingDetailDto> HistoryVoucherSetingDetails { get; set; }
        
    }
}
