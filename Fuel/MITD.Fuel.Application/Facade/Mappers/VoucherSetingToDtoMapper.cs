using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class VoucherSetingToDtoMapper : IVoucherSetingToDtoMapper
    {
        public IEnumerable<VoucherSeting> MapToEntity(IEnumerable<VoucherSetingDto> models)
        {
            throw new NotImplementedException();
        }

        public VoucherSeting MapToEntity(VoucherSetingDto model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VoucherSetingDto> MapToModel(IEnumerable<VoucherSeting> entities)
        {
            throw new NotImplementedException();
        }

        public VoucherSetingDto MapToModel(VoucherSeting entity)
        {
            throw new NotImplementedException();
        }

        public VoucherSetingDto RemapModel(VoucherSetingDto model)
        {
            throw new NotImplementedException();
        }

        public VoucherSetingDto MapToDtoModel(VoucherSeting voucherSeting)
        {
           return new VoucherSetingDto()
                  {
                      Id=voucherSeting.Id,
                      Company = new CompanyDto() { Id=voucherSeting.Company.Id,Name = voucherSeting.Company.Name},
                      VoucherMainDescription = voucherSeting.VoucherMainDescription,
                      VoucherMainRefDescription = voucherSeting.VoucherMainRefDescription,
                      VoucherDetailTypeId = voucherSeting.VoucherDetailTypeId,
                      VoucherTypeId=voucherSeting.VoucherTypeId 
                    
                      
                  };
        }

        public List<VoucherSetingDto> MapToDtoModel(List<VoucherSeting> voucherSetings)
        {
            var res = new List<VoucherSetingDto>();

            voucherSetings.ForEach(c => res.Add(MapToDtoModel(c)));

            return res;

        }
    }
}
