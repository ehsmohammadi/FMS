using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{

  public interface IVoucherFacadeService:IFacadeService
    {
      PageResultDto<VoucherDto> GetAll(long companyId, DateTime? fromDate, DateTime? toDate, int voucherTypr, string refNo, string state, int pageIndex, int pageSize);
      VoucherDto GetById(long id);

      VoucherEntityDto GetEntityId(string refNo);
    }
}
