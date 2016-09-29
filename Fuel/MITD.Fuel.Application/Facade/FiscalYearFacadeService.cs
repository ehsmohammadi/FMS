#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using Castle.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

#endregion

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class FiscalYearFacadeService : IFiscalYearFacadeService
    {
        #region props

        private readonly IRepository<Inventory_FinancialYear> financialYearRepository;
        
        #endregion

        #region ctor

        public FiscalYearFacadeService(IRepository<Inventory_FinancialYear> financialYearRepository)
        {
            this.financialYearRepository = financialYearRepository;
        }

        #endregion

        #region methods

        public List<FiscalYearDto> GetAll()
        {
            //const int startingFiscalYear = 1393;

            //PersianCalendar pCal = new PersianCalendar();

            //int currentFiscalYear = pCal.GetYear(DateTime.Now); //This logic is valid only for persian fiscal years.

            //List<FiscalYearDto> result = new List<FiscalYearDto>();

            //for (int fiscalYear = startingFiscalYear; fiscalYear <= currentFiscalYear; fiscalYear++)
            //{
            //    var fromDateTime = pCal.ToDateTime(fiscalYear, 1, 1, 0, 0, 0, 0);
            //    var toDateTime = pCal.ToDateTime(fiscalYear + 1, 1, 1, 0, 0, 0, 0).AddSeconds(-1);

            //    result.Add(new FiscalYearDto()
            //                {
            //                    Id = fiscalYear,
            //                    YearNumber = fiscalYear,
            //                    DisplayText = "سال مالی " + fiscalYear,
            //                    FromDateTime = fromDateTime,
            //                    ToDateTime = toDateTime
            //                });
            //}

            //return result;

            PersianCalendar pCal = new PersianCalendar();

            var years =  financialYearRepository.GetAll();

            return years.Select(y => new FiscalYearDto()
                                     {
                                         Id = y.Id,
                                         DisplayText = y.Name,
                                         FromDateTime = y.StartDate,
                                         ToDateTime = y.EndDate,
                                         YearNumber = pCal.GetYear(y.StartDate)
                                     }).ToList();

        }

        #endregion

    }
}