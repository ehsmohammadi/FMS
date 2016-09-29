using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class VoucherDto
    {

        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        long _voucherDetailTypeId;
        public long VoucherDetailTypeId
        {
            get { return _voucherDetailTypeId; }
            set { this.SetField(p => p.VoucherDetailTypeId, ref _voucherDetailTypeId, value); }
        }


        string _voucherDetailTypeName;
        public string VoucherDetailTypeName
        {
            get { return _voucherDetailTypeName; }
            set { this.SetField(p => p.VoucherDetailTypeName, ref _voucherDetailTypeName, value); }
        }

        string _stateName;
        public string StateName
        {
            get { return _stateName; }
            set { this.SetField(p => p.StateName, ref _stateName, value); }
        }
        int _state;
        public int State
        {
            get { return _state; }
            set { this.SetField(p => p.State, ref _state, value); }
        }


        private CompanyDto _company;
        public CompanyDto Company
        {
            get { return _company; }
            set { this.SetField(p => p.Company, ref _company, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { this.SetField(p => p.Description, ref _description, value); }
        }
        string _financialVoucherDate;
        public string FinancialVoucherDate
        {
            get { return _financialVoucherDate; }
            set { this.SetField(p => p.FinancialVoucherDate, ref _financialVoucherDate, value); }
        }


        private DateTime _localVoucherDate;
        public DateTime LocalVoucherDate
        {
            get { return _localVoucherDate; }
            set { this.SetField(p => p.LocalVoucherDate, ref _localVoucherDate, value); }
        }


         string _localVoucherNo;

        public string LocalVoucherNo
        {
            get { return _localVoucherNo; }
            set { this.SetField(p => p.LocalVoucherNo, ref _localVoucherNo, value); }
        }

        private string _referenceNo;

        public string ReferenceNo
        {
            get { return _referenceNo; }
            set { this.SetField(p => p.ReferenceNo, ref _referenceNo, value); }
        }

        private string _voucherRef;

        public string VoucherRef
        {
            get { return _voucherRef; }
            set { this.SetField(p => p.VoucherRef, ref _voucherRef, value); }
        }

        private string _referenceType;
        public string ReferenceType
        {
            get { return _referenceType; }
            set { this.SetField(p => p.ReferenceType, ref _referenceType, value); }
        }

        private List<JournalEntryDto> _journalEntryDtos;
        public List<JournalEntryDto> JournalEntryDtos
        {
            get { return _journalEntryDtos; }
            set { this.SetField(p => p.JournalEntryDtos, ref _journalEntryDtos, value); }
        }

        string _financialVoucherNo;

        public string FinancialVoucherNo
        {
            get { return _financialVoucherNo; }
            set { this.SetField(p => p.FinancialVoucherNo, ref _financialVoucherNo, value); }
        }

       List<VoucherTransferLogDto> _voucherTransferLogDto;

       public List<VoucherTransferLogDto> VoucherTransferLogDto
        {
            get { return _voucherTransferLogDto; }
            set { this.SetField(p => p.VoucherTransferLogDto, ref _voucherTransferLogDto, value); }
        }

    }
}
