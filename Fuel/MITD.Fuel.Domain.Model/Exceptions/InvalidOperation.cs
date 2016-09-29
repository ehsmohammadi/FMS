#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace MITD.Fuel.Domain.Model.Exceptions
{
    public class InvalidOperation : FuelException
    {
        public InvalidOperation(string operationName, string message)
            : base(message)
        {
            OperationName = operationName;
        }

        public string OperationName { get; private set; }

    }
}