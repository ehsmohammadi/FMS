using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.CharterStates
{
   public class CharterState
   {
       protected ICharterStateFactory _charterStateFactory;
       public CharterState(ICharterStateFactory charterStateFactory)
       {
           _charterStateFactory = charterStateFactory;
       }
       
       public virtual void Approve(Charter charter, long approverId)
        {
            throw new InvalidStateException("Approve", string.Format("Cannot Approve {0} State", charter.CurrentState.ToString()));
        }

       public virtual void Reject(Charter charter, long approverId)
        {
            throw new InvalidStateException("Reject", string.Format("Cannot Reject {0} State", charter.CurrentState.ToString()));
        }

       public virtual void Cancel(Charter charter, long approverId)
        {
            throw new InvalidStateException("Cancel", string.Format("Cannot Cancel {0} State", charter.CurrentState.ToString()));
        }
    }
}
