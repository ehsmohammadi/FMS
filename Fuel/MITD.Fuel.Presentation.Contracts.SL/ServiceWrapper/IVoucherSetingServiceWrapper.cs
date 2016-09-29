using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IVoucherSetingServiceWrapper:IServiceWrapper
    {
        void GetByFilter(Action<PageResultDto<VoucherSetingDto>,Exception> action,long companyId, int voucherTypeId, int voucherDetailTypeId, int pageIndex, int pageSize);

        void GetById(Action<VoucherSetingDto,Exception> action, long id);
        void GetDetailById(Action<VoucherSetingDetailDto, Exception> action, long id, long detailId);

        void AddVoucherSeting(Action<VoucherSetingDto, Exception> action, VoucherSetingDto voucherSetingDto);

        void UpdateVoucherSeting(Action<VoucherSetingDto, Exception> action,long id, VoucherSetingDto voucherSetingDto);

        void AddVoucherSetingDetail(Action<VoucherSetingDetailDto, Exception> action,
            VoucherSetingDetailDto voucherSetingDto);

        void UpdateVoucherSetingDetail(Action<VoucherSetingDetailDto, Exception> action, long id,
            VoucherSetingDetailDto voucherSetingDetailDto);

        void DeleteVoucherSetingDetail(Action<string, Exception> action, long id, long detailId);
    }
}
