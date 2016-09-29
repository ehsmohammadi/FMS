using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Application;

namespace MITD.Fuel.Application.Service.Contracts
{
   public interface ITransferTypeApplicationService : IApplicationService<TransferType>
    {
       List<TransferType> GetAll();
    }
}
