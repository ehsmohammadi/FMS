#region

using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.Exceptions;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class Company
    {

        public long Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public bool IsMemberOfHolding { get; set; }

        public long BasisId { get; set; }

        public virtual List<VesselInCompany> VesselsOperationInCompany { get; set; }
        public virtual List<Vessel> Fleet { get; set; }

        public virtual List<FuelUser> Users { get; set; }

        public virtual List<Good> Goods { get; set; }


        public Company()
        {
            //VesselsOperationInCompany = new List<VesselInCompany>();
        }

        public Company(long id, string code, string name)
            : this()
        {
            Id = id;

            Code = code;

            Name = name;

            //Vessels = vessels;
        }
    }
}