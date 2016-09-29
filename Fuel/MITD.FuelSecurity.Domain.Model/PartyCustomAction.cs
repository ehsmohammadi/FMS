using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.FuelSecurity.Domain.Model
{
    public class PartyCustomAction
    {
        public long Id { get; set; }

        public long PartyId { get; set; }

        public virtual Party Party { get;private set; }

        public int ActionTypeId { get; set; }

        public virtual ActionType ActionType { get; set; }

        public bool IsGranted { get; set; }

        private PartyCustomAction()
        {
                
        }

        public PartyCustomAction(long partyId, int actionTypeId, bool isGranted)
        {
            this.PartyId = partyId;
            this.ActionTypeId = actionTypeId;
            this.IsGranted = isGranted;
           // this.ActionType = ActionType.FromValue(actionTypeId);
           // this.Party = new Party(PartyId,"");
        }
    }
}
