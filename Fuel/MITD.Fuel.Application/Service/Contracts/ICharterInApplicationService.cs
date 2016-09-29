using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Services.Application;

namespace MITD.Fuel.Application.Service.Contracts
{
    public interface ICharterInApplicationService : IApplicationService
    {
        void AddStart(long chartererId, long ownerId, long vesselInCompanyId, long currencyId, DateTime actionDate, OffHirePricingType offHirePricingType, long userId);

         void AddEnd(long chartererId, long ownerId, long vesselInCompanyId, long currencyId, DateTime actionDate, CharterEndType charterEndType, OffHirePricingType offHirePricingType, long userId);
        

         void UpdateStart(long id, long chartererId, long ownerId, long vesselInCompanyId, long currencyId,
                                DateTime actionDate,
                                OffHirePricingType offHirePricingType);
       

        void UpdateEnd(long id, long chartererId, long ownerId, long vesselInCompanyId, long currencyId,
                       DateTime actionDate,
                      CharterEndType charterEndType);
       


        void Delete(long id);

        void AddItem(long id, long charterId, decimal rob, decimal fee,decimal feeOffhire,
                         long goodId, long tankId, long unitId);

        void UpdateItem(long id, long charterId, decimal rob, decimal fee, decimal feeOffhire,
                     long goodId, long tankId, long unitId);

        void DeleteItem(long id, long charterId);
    }
}
