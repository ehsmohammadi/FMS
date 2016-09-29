using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;

namespace MITD.Fuel.Domain.Model.Repositories
{
    public interface ICharterInRepository:ICharterRepository
    {
        PageResult<CharterIn> GetByFilter(long vesselInCompanyId, long companyId, long id, DateTime? startdate,
            DateTime? enddate, int pageSize, int pageIndex);
        CharterIn GetCharterStartById(long id);
        CharterIn GetCharterEnd(long startId);
        CharterIn GetById(long id);
        CharterIn GetCharterStart(long vesselInCompanyId, long chartererId);
        IQueryable<CharterIn> GetQueryable();
        IQueryable<CharterIn> GetQueryInclude();

    }
}
