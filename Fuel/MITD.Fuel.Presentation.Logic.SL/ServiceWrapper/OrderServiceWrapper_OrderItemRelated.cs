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
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public partial class OrderServiceWrapper
    {
        #region Methods


        public void GetItem(Action<OrderItemDto, Exception> action, long orderId, long orderItemId)
        {
            var url = string.Format(orderItemAddressFormatString, orderId, orderItemId);
            WebClientHelper.Get<OrderItemDto>(new Uri(url, UriKind.Absolute),
                                                    action,
                                                    WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void AddItem(Action<OrderItemDto, Exception> action, OrderItemDto ent)
        {
            var url = string.Format(orderItemAddressFormatString, ent.OrderId, string.Empty);

            WebClientHelper.Post<OrderItemDto, OrderItemDto>(new Uri(url, UriKind.Absolute),
                                                                           action, ent,
                                                                           WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void UpdateItem(Action<OrderItemDto, Exception> action, OrderItemDto ent)
        {
            var url = string.Format(orderItemAddressFormatString, ent.OrderId, ent.Id);

            WebClientHelper.Put<OrderItemDto, OrderItemDto>(new Uri(url, UriKind.Absolute),
                                                                          action, ent,
                                                                          WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void DeleteItem(Action<string, Exception> action, OrderItemDto ent)
        {
            var url = string.Format(orderItemAddressFormatString, ent.OrderId, ent.Id);

            WebClientHelper.Delete(new Uri(url, UriKind.Absolute), action, ApiConfig.Headers);
        }


        public void GetMainUnit(Action<MainUnitValueDto, Exception> action, long goodId, long goodUnitId, decimal value)
        {
            var url = string.Format(ApiConfig.HostAddress + "apiArea/Fuel/MainUnit?goodId={0}&goodUnitId={1}&value={2}", goodId, goodUnitId, value);
            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }


        #endregion
    }
}
