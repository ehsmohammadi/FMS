#region

using System;

#endregion

namespace MITD.Fuel.Domain.Model.Exceptions
{
    public class WorkFlowException : FuelException
    {

        public WorkFlowException(string message)
            : base(message)
        {
        }
        
    }

    public class InvalidStateException : FuelException
    {

        public InvalidStateException(string message)
            : base(message)
        {
        }

        public string State { get; set; }

        public InvalidStateException(string state,string message)
            : base(message)
        {
            State = state;
        }

    }
}