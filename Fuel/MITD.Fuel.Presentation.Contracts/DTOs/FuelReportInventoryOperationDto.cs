using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{

    public partial class FuelReportInventoryOperationDto
    {
        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        string code;
        [Required(AllowEmptyStrings = false, ErrorMessage = "code can't be empty")]
        public string Code
        {
            get { return code; }
            set { this.SetField(p => p.Code, ref code, value); }
        }

        DateTime _actionDate;
        public DateTime ActionDate
        {
            get { return _actionDate; }
            set { this.SetField(p => p.ActionDate, ref _actionDate, value); }
        }

        string _actionType;
        //[Required(AllowEmptyStrings = false,ErrorMessage = "ActionType can't be empty")]
        public string ActionType
        {
            get { return _actionType; }
            set { this.SetField(p => p.ActionType, ref _actionType, value); }
        }

        private GoodDto good;
        public GoodDto Good
        {
            get { return good; }
            set { this.SetField(p => p.Good, ref good, value); }

        }
    }
}
