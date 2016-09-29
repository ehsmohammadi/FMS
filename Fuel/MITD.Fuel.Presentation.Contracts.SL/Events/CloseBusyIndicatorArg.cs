using System;

namespace MITD.Fuel.Presentation.Contracts.SL.Events
{
    public class CloseBusyIndicatorArg
    {
        public Guid Guid { get; set; }

        public CloseBusyIndicatorArg(Guid guid)
        {
            this.Guid = guid;
        }
    }
}
