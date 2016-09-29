#region

using System;

#endregion

namespace MITD.Fuel.Domain.Model.Exceptions
{
    public class InvalidArgument : FuelException
    {
        public string ParameterName { get; private set; }
       
        
        public InvalidArgument(string message, string paramName)
            : base(message)
        {
            ParameterName = paramName;
        }

        public InvalidArgument(string paramName)
            : base(paramName)
        {
            ParameterName = paramName;
        }
      
    }
}