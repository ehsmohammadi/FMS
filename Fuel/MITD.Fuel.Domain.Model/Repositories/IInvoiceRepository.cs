#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

#endregion

namespace MITD.Fuel.Domain.Model.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Invoice GetConfiguredInvoice(long id,ISingleResultFetchStrategy<Invoice> fetch ,IEntityConfigurator<Invoice> configuration);

//        PageResult<Invoice> GetByFilter(int companyId, int orderCreatorId, InvoiceTypes orderType, DateTime fromDate,
//                                      DateTime toDate, int pageSize, int pageIndex);
        InvoiceItem SingleInvoiceItem(Expression<Func<InvoiceItem, bool>> where, ISingleResultFetchStrategy<InvoiceItem> fetch);
        IList<EffectiveFactor> GetAllEffectiveFactors();
    }
}