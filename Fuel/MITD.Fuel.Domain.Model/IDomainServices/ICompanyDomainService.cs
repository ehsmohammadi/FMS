#region

using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface ICompanyDomainService : IDomainService<Company>
    {
        bool CanBePoGood(long goodId, long companyId);

        void IsValid(long ownerId);

        bool GoodHaveValidSuplier(long companyId, long goodId, long supplierId);
        bool CompanyHaveAnySupliersForGood(long companyId, long goodId);

        bool CompanyHaveAnyTransporterForGood(long companyId, long goodId, long transporterId);
        bool GoodHaveValidTransporter(long goodId, long partyGoodId);

        List<Company> Get(List<long> idList);
        List<VesselInCompany> GetCompanyVessels(long enterpriseId);

        List<Company> GetUserCompanies(long userId);
    }
}