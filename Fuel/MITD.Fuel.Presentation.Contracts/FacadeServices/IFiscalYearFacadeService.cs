using System;
using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade
{
    public interface IFiscalYearFacadeService : IFacadeService
    {
        List<FiscalYearDto> GetAll();
    }
}