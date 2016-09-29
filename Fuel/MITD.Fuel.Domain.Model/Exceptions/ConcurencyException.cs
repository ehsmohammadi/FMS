#region

using System;

#endregion

namespace MITD.Fuel.Domain.Model.Exceptions
{
    public class ConcurencyException : FuelException
    {
        public ConcurencyException(string paramName)
            : base(paramName)
        {
        }

    }
}