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
    public interface IVoucherSetingFacadeService : IFacadeService
    {
        PageResultDto<VoucherSetingDto> GetByFilter(long companyId, int voucherTypeId, int voucherDetailTypeId,
            int pageIndex, int pageSize);

        VoucherSetingDto GetById(long id);

        VoucherSetingDetailDto GetDetailById(long id,long detailId);
        void AddVoucherSeting(VoucherSetingDto voucherSetingDto);

        void UpdateVoucherSeting(VoucherSetingDto voucherSetingDto);

        void AddVoucherSetingDetail(VoucherSetingDetailDto voucherSetingDto);

        void UpdateVoucherSetingDetail(VoucherSetingDetailDto voucherSetingDto);
        void UpdateDelete(long voucherSetingId, long voucherSetingDetailId);
    }
}
