using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Presentation.Contracts.Enums
{
  public enum ResultType
    {
      New=1,
      Accept=2,
      Reject=3,
      Exception=4,
      Failure = 5,
      ExceptionLimitExceeded = 6,


    }
}
