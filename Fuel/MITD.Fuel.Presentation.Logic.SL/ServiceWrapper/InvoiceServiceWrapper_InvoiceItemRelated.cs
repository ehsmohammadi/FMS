#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MITD.Presentation;
using System.Linq;
#endregion

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public partial class InvoiceServiceWrapper
    {
        #region Methods

        public void GetItem(Action<InvoiceItemDto, Exception> action, long invoiceId, long invoiceItemId)
        {
            var url = string.Format(invoiceItemAddressFormatString, invoiceId, invoiceItemId);
            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }


        public void UpdateItem(Action<InvoiceItemDto, Exception> action, InvoiceItemDto ent)
        {
            var url = string.Format(invoiceItemAddressFormatString, ent.InvoiceId, ent.Id);

            WebClientHelper.Put(new Uri(url, UriKind.Absolute), action, ent, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void DeleteItem(Action<string, Exception> action, InvoiceItemDto ent)
        {
            var url = string.Format(invoiceItemAddressFormatString, ent.InvoiceId, ent.Id);

            WebClientHelper.Delete(new Uri(url, UriKind.Absolute), action);
        }


        public void GetMainUnit(Action<MainUnitValueDto, Exception> action, long goodId, long goodUnitId, decimal value)
        {
            var url = string.Format(ApiConfig.HostAddress + "apiArea/Fuel/MainUnit/{0}/{1}/{2}", goodId, goodUnitId, value);
            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GenerateItemByFilter(Action<IEnumerable<InvoiceItemDto>, Exception> action, List<long> orderIdList)
        {

            var url = string.Format(invoiceItemAddressFormatString, 0, string.Empty);
            var sb = new StringBuilder(url);
            var strOrderLIst = "";
            orderIdList.Select(c => strOrderLIst = strOrderLIst + c + ",").ToList();
            sb.Append(string.Concat("?orderIdList=", strOrderLIst.Remove(strOrderLIst.Length - 1)));
            WebClientHelper.Get(new Uri(sb.ToString(), UriKind.Absolute), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);

        }

        #endregion
    }
}