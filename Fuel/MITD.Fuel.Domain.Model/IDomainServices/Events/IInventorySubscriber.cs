using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Events;

namespace MITD.Fuel.Domain.Model.IDomainServices.Events
{
    public interface IInventorySubscriber : IEventHandler<CharterInApproveArg>,
        IEventHandler<CharterInDisApproveArg>,
        IEventHandler<CharterInFinalApproveArg> ,
        IEventHandler<CharterOutApproveArg>,
        IEventHandler<CharterOutDisApproveArg>,
        IEventHandler<CharterOutFinalApproveArg> 
    {
    }
}
