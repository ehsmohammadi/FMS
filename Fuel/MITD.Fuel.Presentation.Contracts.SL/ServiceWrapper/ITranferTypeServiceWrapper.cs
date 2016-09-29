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

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface ITranferTypeServiceWrapper : IServiceWrapper
    {
        void GetAll(Action<List<TransferTypeDto>, Exception> action);
    }
}
