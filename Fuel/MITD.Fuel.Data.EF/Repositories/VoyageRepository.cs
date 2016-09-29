using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class VoyageRepository : EFRepository<Voyage>, IVoyageRepository
    {
        public VoyageRepository(EFUnitOfWork unitofwork)
            : base(unitofwork)
        {
        }

        public VoyageRepository(IUnitOfWorkScope unitofworkscope)
            : base(unitofworkscope)
        {
        }


        //public List<Voyage> GetByFilter(long companyId, long vesselInCompanyId)
        //{
        //    var result = Find(v => v.CompanyId == companyId &&
        //                           v.VesselInCompanyId == vesselInCompanyId).ToList();

        //    return result ; 
        //}

        public void UpdateVoyagesFromRotationData()
        {
            this.Context.ExecuteStoreCommand("exec Fuel.UpdateVoyagesFromRotationData");
        }
    }
}