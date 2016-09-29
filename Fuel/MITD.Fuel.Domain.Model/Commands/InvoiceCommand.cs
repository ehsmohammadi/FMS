using System;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceStates;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.Commands
{
    public class InvoiceCommand
    {
        #region Properties


        public long Id { get; set; }

        public long ComapnyId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public long CurrencyId { get; set; }

        public States State { get; set; }

        public InvoiceState InvoiceState { get; set; }

        public string Description { get; set; }

        public DivisionMethods DivisionMethod { get; set; }

        public string InvoiceNumber { get; set; }

        public AccountingTypes AccountingType { get; set; }
        public long? InvoiceRefrenceId { get; set; }

        public InvoiceTypes InvoiceType { get; set; }

        public long? TransporterId { get; set; }

        public long? SupplierId { get; set; }

        public long OwnerId { get; set; }

        public Currency Currency { get; set; }

        public Company Owner { get; set; }

        public Invoice InvoiceRefrence { get; set; }

        public IList<long> OrdersRefrenceId { get; set; }

        public List<InvoiceItemCommand> InvoiceItems { get; set; }

        public bool IsCreditor { get; set; }

        public Company Supplier { get; set; }

        public Company Transporter { get; set; }

        public List<InvoiceAdditionalPriceCommand> AdditionalPrices { get; set; }

        #endregion

        public InvoiceCommand()
        {
            AdditionalPrices = new List<InvoiceAdditionalPriceCommand>();
            InvoiceItems = new List<InvoiceItemCommand>();
            OrdersRefrenceId = new List<long>();
        }
        public InvoiceCommand(InvoiceTypes invoiceType,
                       string invoiceNumber,
                       Company owner,
                       DateTime invoiceDate,
                       DivisionMethods divisionMethod,
                       AccountingTypes accountType,
                       Invoice invoiceRefrence,
                       List<long> orderRefrences,
                       Currency currency,
                       Company transporter,
                       Company supplier,
                       string description,
                       List<InvoiceItemCommand> list)
        {
            // TODO: Complete member initialization
            InvoiceType = invoiceType;
            InvoiceNumber = invoiceNumber;
            Owner = owner;
            InvoiceDate = invoiceDate;
            DivisionMethod = divisionMethod;
            AccountingType = accountType;
            InvoiceRefrence = invoiceRefrence;
            OrdersRefrenceId = orderRefrences;
            Currency = currency;
            Transporter = transporter;
            Supplier = supplier;
            Description = description;
            InvoiceItems = list;
        }
    }
}
