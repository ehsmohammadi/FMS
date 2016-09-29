using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Events;
using MITD.Fuel.Domain.Model.IDomainServices.Events;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.DomainServices
{
    public class InventorySubscriber : IInventorySubscriber
    {

        public void Handle(CharterInFinalApproveArg eventData)
        {
            var obj = eventData.SendObject;
        }
        public void Handle(CharterInApproveArg eventData)
        {
            var obj = eventData.SendObject;
        }

        public void Handle(CharterInDisApproveArg eventData)
        {
            var obj = eventData.SendObject;
        }

        public void Handle(CharterOutApproveArg eventData)
        {
            var obj = eventData.SendObject;
        }

        public void Handle(CharterOutDisApproveArg eventData)
        {
            var obj = eventData.SendObject;
        }

        public void Handle(CharterOutFinalApproveArg eventData)
        {
            var obj = eventData.SendObject;
        }
    }
}
