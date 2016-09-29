#region

using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
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
    public partial class InvoiceServiceWrapper : IInvoiceServiceWrapper
    {
        #region fields

        private readonly string invoiceAddressFormatString;

        private readonly string invoiceItemAddressFormatString;
        private readonly string invoiceAdditionalPriceAddressFormatString;
        readonly string invoiceEffectiveAddressFormatString;

        #endregion

        public InvoiceServiceWrapper()
        {
            invoiceAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/Invoice/{0}");

            invoiceItemAddressFormatString = string.Concat(invoiceAddressFormatString, "/InvoiceItem/{1}");
            invoiceEffectiveAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/InvoiceEffectiveFactor/{0}");
            invoiceAdditionalPriceAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/InvoiceAdditionalPrice/{0}");
        }

        #region methods

        public void GetByFilter(Action<PageResultDto<InvoiceDto>, Exception> action, long? companyId, DateTime? fromDate, DateTime? toDate, string invoiceIds, string invoiceItemIds, string invoiceNumber, long? vesselInCompanyId, string orderNumbers, InvoiceTypeEnum? invoiceType, int pageSize, int pageIndex, bool submitedState)
        {
            string url = string.Format(invoiceAddressFormatString, string.Empty);

            var sb = new StringBuilder(url);
            sb.Append(string.Concat("?companyId=", companyId));
            sb.Append(string.Concat("&fromDate=", HttpUtil.DateToString(fromDate)));
            sb.Append(string.Concat("&toDate=", HttpUtil.DateToString(toDate)));
            sb.Append(string.Concat("&invoiceIds=", invoiceIds));
            sb.Append(string.Concat("&invoiceItemIds=", invoiceItemIds));
            sb.Append(string.Concat("&invoiceNumber=",invoiceNumber));
            sb.Append(string.Concat("&vesselInCompanyId=", vesselInCompanyId));
            sb.Append(string.Concat("&orderNumbers=", orderNumbers));
            sb.Append(string.Concat("&invoiceType=", invoiceType));
            sb.Append(string.Concat("&pageSize=", pageSize));
            sb.Append(string.Concat("&pageIndex=", pageIndex));
            sb.Append(string.Concat("&submitedState=", submitedState));

            WebClientHelper.Get(new Uri(sb.ToString(), UriKind.Absolute),
                                action,
                                WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GetAll(Action<PageResultDto<InvoiceDto>, Exception> action, string methodName, int pageSize,
                           int pageIndex)
        {
            var url = string.Format(invoiceAddressFormatString, string.Empty)
                + "?PageSize=" + pageSize + "&PageIndex=" + pageIndex;

            WebClientHelper.Get(new Uri(url, UriKind.Absolute),
                                action,
                                WebClientHelper.MessageFormat.Json,ApiConfig.Headers
                );
        }

        public void GetById(Action<InvoiceDto, Exception> action, long id)
        {
            var url = string.Format(invoiceAddressFormatString, id);

            WebClientHelper.Get(new Uri(url, UriKind.Absolute),
                                action,
                                WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void Add(Action<InvoiceDto, Exception> action, InvoiceDto ent)
        {
            var url = string.Format(invoiceAddressFormatString, string.Empty);

            WebClientHelper.Post<InvoiceDto, InvoiceDto>(new Uri(url, UriKind.Absolute),
                                                         action, ent,
                                                         WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void Update(Action<InvoiceDto, Exception> action, InvoiceDto ent)
        {
            var url = string.Format(invoiceAddressFormatString, ent.Id);

            WebClientHelper.Put<InvoiceDto, InvoiceDto>(new Uri(url, UriKind.Absolute),
                                                        action, ent,
                                                        WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void Delete(Action<string, Exception> action, long id)
        {
            var url = string.Format(invoiceAddressFormatString, id);

            WebClientHelper.Delete(new Uri(url, UriKind.Absolute), action, ApiConfig.Headers);
        }

      

        #endregion
    }
}