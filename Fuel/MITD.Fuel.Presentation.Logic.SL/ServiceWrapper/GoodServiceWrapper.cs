using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public class GoodServiceWrapper : IGoodServiceWrapper
    {
        private string goodAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/Good/{0}");

        public void GetAll(Action<List<GoodDto>, Exception> action, string methodName, long companyId)
        {
            var url = string.Format(goodAddressFormatString, companyId);

            WebClientHelper.Get<List<GoodDto>>(new Uri(url, UriKind.Absolute),
                                                                  (res, exp) =>
                                                                  {
                                                                      action(res, exp);
                                                                  },
                                                                  WebClientHelper.MessageFormat.Json,ApiConfig.Headers
              );

        }
    }
}
