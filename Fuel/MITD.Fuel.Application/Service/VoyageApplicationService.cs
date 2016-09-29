using System;
using System.Collections.Generic;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Application.Service
{
    public class VoyageApplicationService: IVoyageApplicationService
    {
        private readonly IVoyageRepository voyageRepository;

        public VoyageApplicationService(IVoyageRepository voyageRepository)
        {
            this.voyageRepository = voyageRepository;
        }


        //public List<Voyage> GetByFilter(long companyId, long vesselId)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Voyage> GetAll()
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Voyage> Get(List<long> IDs)
        //{
        //    throw new NotImplementedException();
        //}

        public void UpdateVoyagesFromRotationData()
        {
            voyageRepository.UpdateVoyagesFromRotationData();
        }
    }
}
