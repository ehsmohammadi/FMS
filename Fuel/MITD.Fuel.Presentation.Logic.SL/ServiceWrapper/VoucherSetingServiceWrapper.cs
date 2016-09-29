using System;
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
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public class VoucherSetingServiceWrapper : IVoucherSetingServiceWrapper
    {
        private string hostVoucherAddressController;
        private string hostVoucherSetinDetailAddressController;
        public VoucherSetingServiceWrapper()
        {
            hostVoucherAddressController = ApiConfig.HostAddress + "apiarea/Fuel/VoucherSeting/{0}";
            hostVoucherSetinDetailAddressController = ApiConfig.HostAddress + "apiarea/Fuel/VoucherSetingDetail/{0}";
        }
        public void GetByFilter(Action<PageResultDto<VoucherSetingDto>, Exception> action, long companyId, int voucherTypeId, int voucherDetailTypeId, int pageIndex, int pageSize)
        {
            var uri = hostVoucherAddressController;
            var stringBuilder = new StringBuilder(uri);
            stringBuilder.Append("?companyId=" + companyId);
            stringBuilder.Append("&voucherTypeId=" + voucherTypeId);
            stringBuilder.Append("&voucherDetailTypeId=" + voucherDetailTypeId);
            stringBuilder.Append("&pageIndex=" + pageIndex);
            stringBuilder.Append("&pageSize=" + pageSize);
          

            WebClientHelper.Get<PageResultDto<VoucherSetingDto>>(new Uri(stringBuilder.ToString(), UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);

        }

        public void GetById(Action<VoucherSetingDto, Exception> action, long id)
        {
            string uri = string.Format(hostVoucherAddressController, id);
            WebClientHelper.Get<VoucherSetingDto>(new Uri(uri, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void GetDetailById(Action<VoucherSetingDetailDto, Exception> action, long id,long detailId)
        {
            var uri = hostVoucherAddressController;
            var stringBuilder = new StringBuilder(uri);
            stringBuilder.Append("?id=" + id);
            stringBuilder.Append("&detailId=" + detailId);
            uri = stringBuilder.ToString();
            WebClientHelper.Get<VoucherSetingDetailDto>(new Uri(uri, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }




        public void AddVoucherSeting(Action<VoucherSetingDto, Exception> action, VoucherSetingDto voucherSetingDto)
        {
            WebClientHelper.Post<VoucherSetingDto,VoucherSetingDto>(new Uri(hostVoucherAddressController,UriKind.Absolute),action,voucherSetingDto,WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void AddVoucherSetingDetail(Action<VoucherSetingDetailDto, Exception> action, VoucherSetingDetailDto voucherSetingDto)
        {
            WebClientHelper.Post<VoucherSetingDetailDto, VoucherSetingDetailDto>(new Uri(hostVoucherSetinDetailAddressController, UriKind.Absolute), action, voucherSetingDto, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }




        public void UpdateVoucherSeting(Action<VoucherSetingDto, Exception> action, long id, VoucherSetingDto voucherSetingDto)
        {
            var uri = string.Format(hostVoucherAddressController, id);
            WebClientHelper.Put<VoucherSetingDto, VoucherSetingDto>(new Uri(uri), action, voucherSetingDto, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }


        public void UpdateVoucherSetingDetail(Action<VoucherSetingDetailDto, Exception> action, long id, VoucherSetingDetailDto voucherSetingDetailDto)
        {
            var uri = string.Format(hostVoucherSetinDetailAddressController, id);
            WebClientHelper.Put<VoucherSetingDetailDto, VoucherSetingDetailDto>(new Uri(uri), action, voucherSetingDetailDto, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void DeleteVoucherSetingDetail(Action<string, Exception> action, long id, long detailId)
        {
            string uri = String.Concat(String.Format(hostVoucherAddressController, id));
            uri += "?detailId=" + detailId.ToString();
            WebClientHelper.Delete(new Uri(uri, UriKind.Absolute), action, ApiConfig.Headers);
        }
    }
}
