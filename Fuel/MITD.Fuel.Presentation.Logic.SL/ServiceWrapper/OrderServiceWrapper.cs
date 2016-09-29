#region

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using System.IO;

#endregion

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public partial class OrderServiceWrapper : IOrderServiceWrapper
    {
        #region fields

        private readonly string orderAddressFormatString;

        private readonly string orderItemAddressFormatString;

        #endregion

        public OrderServiceWrapper()
        {
            orderAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/Order/{0}");

            orderItemAddressFormatString = string.Concat(orderAddressFormatString, "/OrderItem/{1}");
        }

        #region methods

        public void GetAll(Action<PageResultDto<OrderDto>, Exception> action, string methodName, int pageSize, int pageIndex)
        {
            var url = string.Format(orderAddressFormatString, string.Empty) + "?PageSize=" + pageSize + "&PageIndex=" + pageIndex;

            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GetById(Action<OrderDto, Exception> action, long id)
        {
            var url = string.Format(orderAddressFormatString, id);

            WebClientHelper.Get<OrderDto>(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void Add(Action<OrderDto, Exception> action, OrderDto ent)
        {
            var url = string.Format(orderAddressFormatString, string.Empty);

            WebClientHelper.Post<OrderDto, OrderDto>
                (new Uri(url, UriKind.Absolute), action, ent, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void Update(Action<OrderDto, Exception> action, OrderDto ent)
        {
            var url = string.Format(orderAddressFormatString, ent.Id);

            WebClientHelper.Put<OrderDto, OrderDto>
                (new Uri(url, UriKind.Absolute), action, ent, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void Delete(Action<string, Exception> action, long id)
        {
            var url = string.Format(orderAddressFormatString, id);

            WebClientHelper.Delete(new Uri(url, UriKind.Absolute), action, ApiConfig.Headers);
        }

        public void GetByFilter(Action<PageResultDto<OrderDto>, Exception> action, long? companyId, DateTime? fromDate, DateTime? toDate, long? orderId, long? orderItemId, string orderNumber, string orderTypes, int pageSize, int pageIndex, long? vesselInCompanyId, long? supplierId = null, long? transporterId = null, bool includeOrderItem = false, string orderIdList = null, bool submitedState = false)
        {
            string url = string.Format(orderAddressFormatString, string.Empty);
            var sb = new StringBuilder(url);
            sb.Append(string.Concat("?companyId=", companyId));
            sb.Append(string.Concat("&fromDate=", HttpUtil.DateToString(fromDate)));
            sb.Append(string.Concat("&toDate=", HttpUtil.DateToString(toDate)));
            sb.Append(string.Concat("&orderId=", orderId));
            sb.Append(string.Concat("&orderItemId=", orderItemId));
            sb.Append(string.Concat("&orderNumber=", orderNumber));
            sb.Append(string.Concat("&orderTypes=", orderTypes));
            
            sb.Append(string.Concat("&pageSize=", pageSize));
            sb.Append(string.Concat("&pageIndex=", pageIndex));
            sb.Append(string.Concat("&vesselInCompanyId=", vesselInCompanyId));

            sb.Append(string.Concat("&supplierId=", supplierId));
            sb.Append(string.Concat("&transporterId=", transporterId));
            sb.Append(string.Concat("&includeOrderItem=", includeOrderItem));

            sb.Append(string.Concat("&orderIdList=", orderIdList));
            sb.Append(string.Concat("&submitedState=", submitedState));
            sb.Append(string.Concat("&fake=", DateTime.Now.Ticks));

            WebClientHelper.Get(new Uri(sb.ToString(), UriKind.Absolute), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        #endregion
    }
}