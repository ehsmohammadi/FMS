#region

using System;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.Factories;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Factories
{
    public interface IInvoiceFactory : IFactory

    {

//        InvoiceItem CreateInvoiceItemObject(Good good,
//                                            decimal quantity,
//                                            decimal fee,
//                                            long divisionPercent,
//                                            GoodUnit goodUnit,
//                                            decimal divisionPrice,
//                                            string description);
//
//        Invoice CreateInvoiceObject2(InvoiceTypes invoiceType,
//                                    string invoiceNumber,
//                                    Company owner,
//                                    DateTime invoiceDate,
//                                    DivisionMethods divisionMethod,
//                                    AccountingTypes accountType,
//                                    Invoice invoiceRefrence,
//                                    List<Order> orderRefrences,
//                                    Currency currency,
//                                    Company transporter,
//                                    Company supplier,
//                                    string description,
//                                    List<InvoiceItem> list);


        Invoice CreateInvoiceObject(InvoiceCommand invoiceCommand, IEnumerable<Good> goods, Company owner, Company transporter, Company supplier, Invoice invoiceRefrence, List<Order> orderRefrences, Currency currency, List<InvoiceItem> invoiceItems, List<InvoiceAdditionalPrice> invoiceAdditionalPriceList, bool forCalculate, long userId);
    }
}