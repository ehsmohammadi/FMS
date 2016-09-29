using System;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Application;

namespace MITD.Fuel.Application.Service.Contracts
{
    public interface ICurrencyApplicationService : IApplicationService
    {
        void UpdateCurrencies();

        //void UpdateCurrencyRatesFromFinance();

    }
}