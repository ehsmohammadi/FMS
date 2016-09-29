#region

using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

#endregion

namespace MITD.Fuel.Domain.Model.Factories
{
    public class InvoiceFactory : IInvoiceFactory
    {
        private readonly IEntityConfigurator<Invoice> invoiceConfigurator;
        private readonly IWorkflowStepRepository _workflowStepRepository;
        private readonly IInvoiceDomainService invoiceDomainService;
        private readonly IInvoiceItemDomainService invoiceItemDomainService;
        readonly IGoodUnitConvertorDomainService goodUnitConvertorDomainService;
        private readonly IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService;
        private IBalanceDomainService balanceDomainService;

        public InvoiceFactory(
                            IEntityConfigurator<Invoice> invoiceConfigurator,
            IWorkflowStepRepository _workflowStepRepository,
            IInvoiceDomainService invoiceDomainService,
            IInvoiceItemDomainService invoiceItemDomainService, IGoodUnitConvertorDomainService goodUnitConvertorDomainService,
            IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService, IBalanceDomainService balanceDomainService)
        {
            this.invoiceConfigurator = invoiceConfigurator;
            this._workflowStepRepository = _workflowStepRepository;
            this.invoiceDomainService = invoiceDomainService;
            this.invoiceItemDomainService = invoiceItemDomainService;
            this.goodUnitConvertorDomainService = goodUnitConvertorDomainService;
            this.invoiceAdditionalPriceDomainService = invoiceAdditionalPriceDomainService;
            this.balanceDomainService = balanceDomainService;
        }

        #region IInvoiceFactory Members



        public Invoice CreateInvoiceObject(InvoiceCommand invoiceCommand, IEnumerable<Good> goods, Company owner, Company transporter, Company supplier, Invoice invoiceRefrence, List<Order> orderRefrences, Currency currency, List<InvoiceItem> invoiceItems, List<InvoiceAdditionalPrice> invoiceAdditionalPriceList, bool forCalculate, long userId)
        {
            
            var invoice = new Invoice
                (
                invoiceCommand.InvoiceType, invoiceCommand.InvoiceNumber, owner, invoiceCommand.InvoiceDate,
                invoiceCommand.DivisionMethod, invoiceCommand.AccountingType, invoiceRefrence, orderRefrences,currency,invoiceCommand.IsCreditor, transporter,
                supplier, invoiceCommand.Description, invoiceItems, invoiceAdditionalPriceList, invoiceConfigurator, invoiceDomainService,
                invoiceItemDomainService,goodUnitConvertorDomainService,invoiceAdditionalPriceDomainService,balanceDomainService);

            if (!forCalculate)
            {
                var initWorkflowStep = _workflowStepRepository.Single
                    (c => c.Workflow.WorkflowEntity == WorkflowEntities.Invoice &&
                         c.CurrentWorkflowStage == WorkflowStages.Initial &&
                         c.Workflow.CompanyId == owner.Id &&
                         c.Workflow.Name == Workflow.DEFAULT_NAME);

                var invoiceWorkflow = new InvoiceWorkflowLog
                    (invoice.Id, WorkflowEntities.Invoice, DateTime.Now, WorkflowActions.Init,
                    userId, "", initWorkflowStep.Id, true);

                invoice.ApproveWorkFlows.Add(invoiceWorkflow);
            }
            return invoice;
        }

        #endregion
    }


}