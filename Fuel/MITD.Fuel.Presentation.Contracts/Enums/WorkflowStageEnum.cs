using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Presentation.Contracts.Enums
{
    public enum WorkflowStageEnum
    {
        None = -1,
        [Description("ثبت اولیه")]
        Initial = 0,
        [Description("تأیید میانی")]
        Approved = 1,
        [Description("تأیید نهایی")]
        FinalApproved = 2,
        [Description("تأیید نهایی")]
        Submited = 3,
        [Description("بسته شده")]
        Closed = 4,
        [Description("ابطال")]
        Canceled = 5,
        [Description("لغو تأیید")]
        SubmitRejected = 6,
        [Description("تأیید توسط مالی")]
        FinancialSubmitted = 7,
    }
}
