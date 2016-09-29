#region

using System;

#endregion

namespace MITD.Fuel.Domain.Model.Exceptions
{
    public class BusinessRuleException : FuelException
    {
        private readonly string _brCode;
        public BusinessRuleException(string brCode)
        {
            _brCode = brCode;
        }
        public BusinessRuleException(string brCode, string errorMessage):
            base(errorMessage)
        {
            _brCode = brCode;
        }

        public string BusinessRuleCode
        {
            get { return _brCode; }
        }
    }
}