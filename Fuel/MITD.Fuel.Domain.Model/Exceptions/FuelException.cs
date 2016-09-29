#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace MITD.Fuel.Domain.Model.Exceptions
{
    public class FuelException : Exception
    {
        public FuelException()
        {
        }

        public FuelException(string message) : base(message)
        {
        }

        [SecuritySafeCritical]
        protected FuelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public FuelException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}