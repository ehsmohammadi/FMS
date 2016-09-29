using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class FuelReportTransferReferenceNoDto
    {
        private long id;
        private string code;
        private string date;

        public long Id
        {
            get { return id; }
            set { this.SetField(P => P.Id, ref id, value); }
        }
        public string Code
        {
            get { return code; }
            set { this.SetField(P => P.Code, ref code, value); }
        }
        public string Date
        {
            get { return date; }
            set { this.SetField(P => P.Date, ref date, value); }
        }

        private ReferenceTypeEnum referenceType;
        public ReferenceTypeEnum ReferenceType
        {
            get { return referenceType; }
            set { this.SetField(P => P.ReferenceType, ref referenceType, value); }
        }
    }
}
