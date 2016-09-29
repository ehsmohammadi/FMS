using MITD.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class ApproveFlowLogDto : DTOBase
    {
        public DateTime ActionDate { get; set; }
        public int ActionTypeId { get; set; }
        public int EntityTypeId { get; set; }
        public long EntityId { get; set; }
        public string Remark { get; set; }
    }
}
