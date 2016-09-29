#region

using System;
using System.Collections.Generic;
using MITD.Fuel.ACL.Contracts.Adapters;
using MITD.Fuel.ACL.Contracts.ServiceWrappers;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.ACL.StorageSpace.Adapter
{
    public class GoodPartyAssignmentAntiCorruptionAdapter : IGoodPartyAssignmentAntiCorruptionAdapter
    {
        public GoodPartyAssignmentAntiCorruptionAdapter(IGoodPartyAssignmentAntiCorruptionServiceWrapper serviceWrapper)
        {
            ServiceWrapper = serviceWrapper;
        }

        private IGoodPartyAssignmentAntiCorruptionServiceWrapper ServiceWrapper { get; set; }

        #region IGoodPartyAssignmentAntiCorruptionAdapter Members

        public bool IsNotOverMaximumOrder(double quantity, int companyId, int goodId, int wareHouseId)
        {
            return ServiceWrapper.IsNotOverMaximumOrder(quantity, companyId, goodId, wareHouseId);
        }

        public List<GoodPartyAssignment> Get(List<int> IDs)
        {
            throw new NotImplementedException();
        }

        public List<GoodPartyAssignment> GetAll()
        {
            throw new NotImplementedException();
        }

        public GoodPartyAssignment Get(int id)
        {
            throw new NotImplementedException();
        }


        public bool IsEqualFixOrder(double quantity, int companyId, int goodId, int wareHouseId)
        {
            return ServiceWrapper.IsNotOverMaximumOrder(quantity, companyId, goodId, wareHouseId);
        }


        public bool CanBeOrderWithReOrderLevelCheck(int companyId, int goodId, int wareHouseId)
        {
            return ServiceWrapper.CanBeOrderWithReOrderLevelCheck(companyId, goodId, wareHouseId);
        }

        #endregion
    }
}