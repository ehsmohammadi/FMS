using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.CharterStates
{
   public class OpenState:CharterState
    {


       public OpenState(ICharterStateFactory charterStateFactory):base(charterStateFactory)
       {
           
       }
    
       public override void Approve(Charter charter, long approverId)
       {

           charter.Submited(approverId);
           charter.SetStateType(States.Submitted);
           charter.SetStateType(_charterStateFactory.CreateSubmittedState());
       }

     
    }
}
