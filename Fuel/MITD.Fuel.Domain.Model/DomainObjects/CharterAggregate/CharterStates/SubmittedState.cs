using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.CharterStates
{
    public class SubmittedState : CharterState
    {

        public SubmittedState(ICharterStateFactory charterStateFactory)
            : base(charterStateFactory)
        {

        }


        public override void Reject(Charter charter, long approverId)
        {
            charter.RejectSubmited(approverId);
            charter.SetStateType(States.SubmitRejected);
            charter.SetStateType(_charterStateFactory.CreateSubmitRejectedState());

        }

    }
}
