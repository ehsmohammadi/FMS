using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.AntiCorruption.Contracts;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices
{
    public class GoodUnitDomainService : IGoodUnitDomainService
    {
        //TODO: (A.H) Added for Fake data fetch;
        private readonly IRepository<GoodUnit> goodUnitFakeRepository;

        public IAntiCorruptionAdapter<GoodUnit, GoodUnitDto> Adapter { get; set; }


        public GoodUnitDomainService(IAntiCorruptionAdapter<GoodUnit, GoodUnitDto> antiCorruptionAdapter, IRepository<GoodUnit> goodUnitFakeRepository)
        {
            this.goodUnitFakeRepository = goodUnitFakeRepository;
            this.Adapter = antiCorruptionAdapter;
        }

        public List<GoodUnit> Get(List<long> IDs)
        {
            //            var data = this.Adapter.Get(IDs);
            //            return data;
            return goodUnitFakeRepository.GetAll().Where(c => IDs.Contains(c.Id)).ToList();
        }

        public GoodUnit Get(long id)
        {

            return goodUnitFakeRepository.GetAll().SingleOrDefault(c => c.Id == id);
            //            var data =  Adapter.Get(id);
            //            if (data == null)
            //                throw new ObjectNotFound("GoodUnit");
            //            return data;
        }



        public List<GoodUnit> GetAll()
        {
            return goodUnitFakeRepository.GetAll().ToList();

            //            var data = this.Adapter.GetAll();
            //            return data;
        }

        public bool IsValid(long goodUnitId)
        {
            return true;
        }

        public bool Validate(long goodUnitId, long companyId, long goodId, long unitId)
        {
            return true;
        }

        public List<GoodUnit> GetGoodUnits(long goodId)
        {
            return goodUnitFakeRepository.Find(gu => gu.GoodId == goodId).ToList();

        }
    }
}
