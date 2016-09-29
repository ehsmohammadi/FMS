using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public class VoucherServiceWrapper : IVoucherServiceWrapper   
    {
        private string hostVoucherAddressController;

        public VoucherServiceWrapper()
        {
            hostVoucherAddressController = ApiConfig.HostAddress + "apiarea/Fuel/Voucher/{0}";
        }

        public void GetByFilter(Action<PageResultDto<VoucherDto>, Exception> action, long companyId, string fromDate, string toDate, int voucherTypr, string refNo, string state, int pageIndex, int pageSize)
        {
            

            var uri = String.Format(hostVoucherAddressController, string.Empty);
            StringBuilder stringBuilder = new StringBuilder(uri);
            stringBuilder.Append("?companyId=" + companyId);
            stringBuilder.Append("&fromDate=" + fromDate);
            stringBuilder.Append("&toDate=" + toDate);
            stringBuilder.Append("&voucherTypr=" + voucherTypr);
            stringBuilder.Append("&refNo=" + refNo);
            stringBuilder.Append("&state=" + state);
            stringBuilder.Append("&pageIndex=" + pageIndex);
            stringBuilder.Append("&pageSize=" + pageSize);
            stringBuilder.Append("&charterType=" + CharterType.In);

            WebClientHelper.Get<PageResultDto<VoucherDto>>(new Uri(stringBuilder.ToString(), UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);

        }

        public void GetById(Action<VoucherDto, Exception> action, long id)
        {
            string uri = string.Format(hostVoucherAddressController, id);
            WebClientHelper.Get<VoucherDto>(new Uri(uri, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }



        public void SendToFinancial(Action<List<long>, Exception> action, List<long> ids, string dateTime,string code)
        {

            var uri = String.Concat(hostVoucherAddressController, "?date=" + dateTime+"&code="+code);
            WebClientHelper.Post<List<long>, List<long>>(new Uri(uri, UriKind.Absolute), action, ids, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }


        public void GetEntityId(Action<VoucherEntityDto, Exception> action, string refNo)
        {
            string uri = string.Format(hostVoucherAddressController, string.Empty);
            uri = uri + "?refNo=" + refNo;
            WebClientHelper.Get<VoucherEntityDto>(new Uri(uri, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }
    }
}
