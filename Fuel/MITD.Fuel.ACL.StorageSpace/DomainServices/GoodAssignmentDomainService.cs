#region

using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.ACL.Contracts.Adapters;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.FakeDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices;

#endregion

namespace MITD.Fuel.ACL.StorageSpace.DomainServices
{
    public class GoodPartyAssignmentDomainService : IGoodPartyAssignmentDomainService
    {
        public GoodPartyAssignmentDomainService(IGoodPartyAssignmentAntiCorruptionAdapter goodAssignmentAntiCorruptionAdapter)
        {
            Adapter = goodAssignmentAntiCorruptionAdapter;
        }

        public IGoodPartyAssignmentAntiCorruptionAdapter Adapter { get; set; }

        #region IGoodPartyAssignmentDomainService Members

        public bool IsNotOverMaximumOrder(double quantity, long company, long goodId, long wareHouseId)
        {
            return true;// Adapter.IsNotOverMaximumOrder(quantity, company, goodId, wareHouseId);
        }

        public List<GoodPartyAssignment> Get(List<long> IDs)
        {
            return FakeDomainService.GetGoodPartyAssignments().Where(c => IDs.Contains(c.Id)).ToList();
        }

        public GoodPartyAssignment Get(long id)
        {
            return FakeDomainService.GetGoodPartyAssignments().SingleOrDefault(c => c.Id == id);

            //            var data = Adapter.Get(id);
            //            if (data == null)
            //                throw new ObjectNotFound("GoodPartyAssignment");
            //            return data;
        }



        public List<GoodPartyAssignment> GetAll()
        {
            return FakeDomainService.GetGoodPartyAssignments().ToList();

        }


        public bool IsEqualFixOrder(double quantity, long company, long goodId, long wareHouseId)
        {
            return true;// return Adapter.IsEqualFixOrder(quantity, company, goodId, wareHouseId);
        }


        public bool CanBeOrderWithReOrderLevelCheck(long company, long goodId, long wareHouseId)
        {
            return true; //return Adapter.CanBeOrderWithReOrderLevelCheck(company, goodId,
                                                     //      wareHouseId);
        }

        //public bool GoodAssignIdValidForCompanyAndGood(long assigneBuessinessPartyForGoodId, long companyId, long goodId)
        //{
        //    return true;
        //}

        ////public bool IsValid(long assigneBuessinessPartyForGoodId)
        ////{
        ////    return true;
        ////}

        #endregion
    }
}