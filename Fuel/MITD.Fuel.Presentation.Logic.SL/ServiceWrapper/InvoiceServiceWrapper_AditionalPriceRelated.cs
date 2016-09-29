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

        public void CalculateAdditionalPrice(Action<InvoiceDto, Exception> action, InvoiceDto ent)
        {
            var url = string.Format(invoiceAdditionalPriceAddressFormatString, ent.Id, string.Empty);

            WebClientHelper.Put(new Uri(url, UriKind.Absolute), action, ent, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GetEffectiveFactors(Action<ObservableCollection<EffectiveFactorDto>, Exception> action)
        {
            var url = string.Format(invoiceEffectiveAddressFormatString, string.Empty);
            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        #endregion
    }
}