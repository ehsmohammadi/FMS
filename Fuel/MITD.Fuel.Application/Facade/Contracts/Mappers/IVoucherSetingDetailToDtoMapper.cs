using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
  public  interface IVoucherSetingDetailToDtoMapper:IFacadeMapper<VoucherSetingDetail,VoucherSetingDetailDto>
  {
      VoucherSetingDetailDto MapToDtoModel(VoucherSetingDetail voucherSeting);
      List<VoucherSetingDetailDto> MapToDtoModel(List<VoucherSetingDetail> voucherSetings);

    }
}
