using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Contracts.SL.Events.Security
{
    public class UpdatePartyCustomActionsArgs
    {
        public UpdatePartyCustomActionsArgs(Dictionary<int, bool> customActions, string partyName)
        {
            CustomActions = customActions;
            PartyName = partyName;
        }

        public Dictionary<int, bool> CustomActions
        {
            get;
            private set;
        }

        public string PartyName
        {
            get;
            private set;
        }
    }

    
}
