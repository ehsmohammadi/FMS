using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.FuelSecurity.Domain.Model.ErrorException;

namespace MITD.FuelSecurity.Domain.Model
{
    public class Party
    {
        #region Prop

        public byte[] RowVersion { get; private set; }

        public virtual List<PartyCustomAction> CustomActions { get; set; }

        private long id;

        public long Id { get; private set; }

        public string PartyName { get; private set; }

        #endregion

        #region ctor

        public Party()
        {
            CustomActions = new List<PartyCustomAction>();
        }

        public Party(long? id, string partyName)
            : this()
        {
            if (id == null || partyName == null)
                throw new ArgumentNullException();
            this.Id = id.Value;
            this.PartyName = partyName;
        }

        #endregion

        #region Method

        public virtual void AssignCustomActions(ActionType actionType, bool isGranted)
        {
            if (actionType == null)
                throw new FuelSecurityException(0, "عدم دسترسی مناسب");


            if (this.CustomActions.Count(p => p.ActionTypeId == actionType.Id) > 0)
            {
                CustomActions.RemoveAll(pa => pa.ActionTypeId == actionType.Id);
            }

            var customAction = new PartyCustomAction(this.Id, actionType.Id, isGranted);

            CustomActions.Add(customAction);

        }

        public virtual void UpdateCustomActions(Dictionary<ActionType, bool> customActionsParam)
        {
            var actions = customActionsParam.Keys;
            foreach (var action in actions)
            {
                var isGranted = customActionsParam[action];

                if (CustomActions.Count(p => p.ActionTypeId == action.Id) > 0)
                {
                    var actionType = CustomActions.Single(ca => ca.ActionTypeId == action.Id);
                    actionType.IsGranted = isGranted;
                }
                else
                {
                    var customAction = new PartyCustomAction(this.Id, action.Id, isGranted);

                    CustomActions.Add(customAction);
                }


                CustomActions.RemoveAll(ca => actions.Count(ac => ac.Id == ca.ActionTypeId) == 0);

            }


        }
        #endregion
    }
}