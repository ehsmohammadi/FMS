using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public class ReferenceType
    {

        #region Prop
        public int Id { get; set; }
        public string Name { get; set; }

        public static ReferenceType CharterIn
        {
            get { return new ReferenceType(1, "CharterIn"); }
        }
        public static ReferenceType CharterOut
        {
            get { return new ReferenceType(2, "CharterOut"); }
        }
        public static ReferenceType FuelReport
        {
            get { return new ReferenceType(3, "FuelReport"); }
        }

        public static ReferenceType PurchesInvoice
        {
            get { return new ReferenceType(4, "PurchesInvoice"); }
        }

        public static ReferenceType Offhire
        {
            get { return new ReferenceType(5, "Offhire"); }
        }
        public static ReferenceType Invoice
        {
            get { return new ReferenceType(6, "Invoice"); }
        }

        #endregion

        public ReferenceType()
        {

        }

        public ReferenceType(int id, string name)
        {
            this.Name = name;
            this.Id = id;

        }


    }
}
