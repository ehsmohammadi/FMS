#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;

#endregion

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IGoodDomainService : IDomainService<Good>
    {
        List<Good> GetCompanyGoods(long companyId);

        void GetGoodSuppliersAndTransporters(long goodId, List<Company> supplierCompanies,
                                             List<Company> transporterCompanies);

        List<Company> GetGoodSuppliers(long goodId);
        List<Company> GetGoodTransporters(long goodId);
        bool CanBeOrderAmountOfGood(long goodId, double quantity, List<string> exceptionList);
        bool CanBeOrderThisGood(long goodId);
        GoodFullInfo GetGoodInfoes(long companyId, long goodId);
        Good GetGoodWithUnit(long companyId, long goodId);
        IEnumerable<Good> GetCompanyGoodsWithUnits(long companyId);

        List<Good> GetMandatoryVesselGoods(long vesselInCompanyId, DateTime date);
        Good GetGoodWithUnitAndMainUnit(long goodId, long goodUnitId);

        Good FindGood(long companyId, long sharedGoodId);
    }
}
