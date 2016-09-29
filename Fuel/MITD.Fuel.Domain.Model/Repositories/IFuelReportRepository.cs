#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Domain.Model.Repositories
{
    public interface IFuelReportRepository : IRepository<FuelReport>
    {
        List<long> FindOpenFuelReportIdByVesselInCompany(long? vesselInCompanyId);
    }
}