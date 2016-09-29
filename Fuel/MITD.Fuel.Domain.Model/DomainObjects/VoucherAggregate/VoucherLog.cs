using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public class VoucherLog
    {
        public long Id { get;private set; }
        public string ReferenceNo { get; private set; }

        public string ExceptionMessage { get; private set; }

        public string StackTrace { get; private set; }
        public string VoucherType { get;private set; }


        public VoucherLog()
        {

        }

        public VoucherLog(long id
                          , string referenceNo
                          , string exceptionMessage
                          , string stackTrace
                          , string voucherType)
        {
            Id = id;
            ReferenceNo = referenceNo;
            ExceptionMessage = exceptionMessage;
            StackTrace = stackTrace;
            VoucherType = voucherType;
        }
    }
}
