using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels.Security
{
    public class Privilege : ViewModelBase
    {
        private ActionTypeDto actionType;
        public ActionTypeDto ActionType
        {
            get { return actionType; }
            set
            {
                this.SetField(p => p.ActionType, ref actionType, value);
            }
        }

        private bool  isGrant;
        public bool IsGrant
        {
            get { return isGrant; }
            set
            {
                this.SetField(p => p.IsGrant, ref isGrant, value);
            }
        }

        private bool isDeny;
        public bool IsDeny
        {
            get { return isDeny; }
            set
            {
                this.SetField(p => p.IsDeny, ref isDeny, value);
            }
        }

    }
}
