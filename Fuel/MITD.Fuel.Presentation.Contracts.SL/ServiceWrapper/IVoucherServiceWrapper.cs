using System;
using System.Collections.Generic;
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
    public interface IVoucherServiceWrapper:IServiceWrapper
    {
        void GetByFilter(Action<PageResultDto<VoucherDto>, Exception> action, long companyId,string fromDate,string toDate,int voucherTypr,string refNo,string state, int pageIndex, int pageSize);
        void GetById(Action<VoucherDto, Exception> action, long id);
        void SendToFinancial(Action<List<long>, Exception> action, List<long> ids, string dateTime,string code);

        void GetEntityId(Action<VoucherEntityDto, Exception> action, string refNo);
    }
}
