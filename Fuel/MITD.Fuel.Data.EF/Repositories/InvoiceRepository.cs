#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using System.Linq;

#endregion

namespace MITD.Fuel.Data.EF.Repositories
{
    public class InvoiceRepository : EFRepository<Invoice>, IInvoiceRepository
    {
        private readonly IRepository<EffectiveFactor> effectiveFactorRepository;
     
        private readonly IRepository<InvoiceItem> invoiceItemRepository;

        
        public InvoiceRepository(IUnitOfWorkScope iUnitOfWorkScope,
                                
                                 IRepository<InvoiceItem> invoiceItemRepository,
                                 IRepository<EffectiveFactor> effectiveFactorRepository)
            : base(iUnitOfWorkScope)
        {
         
            this.invoiceItemRepository = invoiceItemRepository;
            this.effectiveFactorRepository = effectiveFactorRepository;
        }

        #region IInvoiceRepository Members

        public Invoice GetConfiguredInvoice(long id, ISingleResultFetchStrategy<Invoice> fetch, IEntityConfigurator<Invoice> invoiceConfigurator)
        {
            var invoice = this.Single(c => c.Id == id, fetch);
            return invoiceConfigurator.Configure(invoice);
        }

        public InvoiceItem SingleInvoiceItem(Expression<Func<InvoiceItem, bool>> where, ISingleResultFetchStrategy<InvoiceItem> fetch)
        {
            return invoiceItemRepository.Single(where, fetch);
        }

        #endregion

        public IList<EffectiveFactor> GetAllEffectiveFactors()
        {
            return effectiveFactorRepository.GetAll();
        }
    }
}