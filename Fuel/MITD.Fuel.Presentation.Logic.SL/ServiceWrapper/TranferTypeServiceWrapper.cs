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
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public class TranferTypeServiceWrapper : ITranferTypeServiceWrapper
    {

        //private string baseAddress = String.Format("{0}://{1}:{2}/apiarea/Fuel/TransferType/",
        //                                       Application.Current.Host.Source.Scheme,
        //                                       Application.Current.Host.Source.Host,
        //                                       HostAddressHelper.Port);

        private string baseAddress = String.Format("{0}/apiarea/Fuel/TransferType/"
                                             , ApiConfig.HostAddress);
        public void GetAll(Action<List<TransferTypeDto>, Exception> action)
        {
            var url = baseAddress;// +"GetAll";
            WebClientHelper.Get<List<TransferTypeDto>>(new Uri(url,UriKind.Absolute), action,WebClientHelper.MessageFormat.Json);
        }
    }
}
