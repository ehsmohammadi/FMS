
using System;
using System.Collections.Generic;
using MITD.Presentation;
using MITD.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;



namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IFiscalYearServiceWrapper : IServiceWrapper
    {
        void GetFiscalYears(Action<List<FiscalYearDto>, Exception> action);
    }
}