using System;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainServices;
using MITD.Services.AntiCorruption.Contracts;
using MITD.StorageSpace.Presentation.Contracts.DTOs;


namespace MITD.Fuel.Data.EF.DomainServices
{
    public class VesselDomainService:IVesselDomainService
    {
        IAntiCorruptionAdapter<Vessel, WarehouseDto> Adapter { get; set; }

        public VesselDomainService(IAntiCorruptionAdapter<Vessel, WarehouseDto> adapter)
        {
            this.Adapter = adapter;
        }
         
        public List<Vessel> Get(List<int> IDs)
        {
            var data = this.Adapter.Get(IDs);
            return data;
        }




        public List<Vessel> GetAll()
        {
            var x =this.Adapter.GetType();
            var entities = this.Adapter.GetAll();
            return entities;
        }

        public Vessel Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
