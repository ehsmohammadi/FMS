using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.ACL.Contracts.Adapters;
using MITD.Fuel.ACL.Contracts.Mappers;
using MITD.Fuel.ACL.Contracts.ServiceWrappers;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.ACL.StorageSpace.Adapter
{
    public class GoodAntiCorruptionAdapter : IGoodAntiCorruptionAdapter
    {
        private IGoodAntiCorruptionServiceWrapper ServiceWrapper { get; set; }
        IGoodAntiCorruptionMapper Mapper { get; set; }
        public GoodAntiCorruptionAdapter(IGoodAntiCorruptionServiceWrapper serviceWrapper, IGoodAntiCorruptionMapper mapper)
        {
            this.Mapper = mapper;
            this.ServiceWrapper = serviceWrapper;
        }
        
        public List<Good> Get(List<int> IDs)
        {
            throw new NotImplementedException();
        }

        public List<Good> GetAll(int companyId)
        {
            var dtos = this.ServiceWrapper.GetAll(companyId);
            var result = this.Mapper.MapToEntity(dtos).ToList();
            return result;
        }

        public Good Get(int id)
        {
            throw new NotImplementedException();
        }


        public List<Good> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}