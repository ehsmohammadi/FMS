#region

using System.Collections.Generic;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class GoodFullInfo
    {
        public Good Good { get; set; }
        public bool CanBeOrderedThisGood { get; set; }
        public List<GoodUnit> CompanyGoodUnits { get; set; }
        public List<Company> GoodTransporters { get; set; }
        public List<Company> GoodSuppliers { get; set; }
    }
}