using MITD.Fuel.ACL.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.AntiCorruption;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.Mapper
{
    public class CompanyAntiCorruptionMapper : BaseAntiCorruptionMapper<Company, EnterprisePartyDto>,
        ICompanyAntiCorruptionMapper
    {
        //TODO: MapToEntity is deprecated.
        public override Company MapToEntity(EnterprisePartyDto model)
        {
            Company result = base.MapToEntity(model);


            //TODO: Fake Vessels list.
            //var vessels = MITD.Fuel.Domain.Model.FakeDomainServices.FakeDomainService.GetVessels();
            //result.SetVessels(vessels.FindAll(domV => model.Warehouses.Contains((int)domV.Id)));
            //result.SetVessels(model.Warehouses.Select(dtoVId => vessels.SingleOrDefault(domV => domV.Id == (long) dtoVId)).ToList());
            //TODO: Direct Vessels assignment is changed to method.
            //result.SetVessels(model.Warehouses.Select(c => (long)c).ToList());
            return result;
        }
    }
}