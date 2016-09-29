using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.FuelSecurity.Domain.Model.ErrorException
{
   [Serializable]
    public class FuelSecurityException : Exception
    {
        #region Prop

        public int Code { get; private set; }
        public string  Message { get; private set; }



        #endregion

        public FuelSecurityException(int code,string message):base(message)
        {
            Code = code;
            Message = message;
        }

    }

    [Serializable]
    public class FuelSecurityAccessException :FuelSecurityException
    {
        public FuelSecurityAccessException(int code,string message):base(code,message)
        {
            
        }
    }

    [Serializable]
    public class FuelSecurityIdentityException:FuelSecurityException
    {
        public FuelSecurityIdentityException(int code, string message)
            : base(code, message)
        {
            
        }
    }

}
