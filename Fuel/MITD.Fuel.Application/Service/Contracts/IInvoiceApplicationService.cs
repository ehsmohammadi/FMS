using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Specifications;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Application;

namespace MITD.Fuel.Application.Service.Contracts
{
    public interface IInvoiceApplicationService : IApplicationService
    {

        void DeleteById(long id);
//
//        Invoice Add(string invoiceNumber,
//                    long ownerId,
//                    DateTime invoiceDate,
//                    DivisionMethods divisionMethod,
//                    AccountingTypes accountType,
//                    long? invoiceRefrenceId,
//                    List<long> orderRefrencesId,
//                    long currencyId,
//                    long? transporterId,
//                    long? supplierId,
//                    InvoiceTypes invoiceType,
//                    string description,
//                    IEnumerable<InvoiceItemDto> invoiceItemsDto);



        Invoice Add(InvoiceCommand invoiceCommand);
        Invoice Update(InvoiceCommand invoiceCommand);
        Invoice CalculateAdditionalPrice(InvoiceCommand invoice, long userId);
    }
}