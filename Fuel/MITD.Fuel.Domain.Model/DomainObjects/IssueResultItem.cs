using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class InventoryResultItem
    {
        public long Id { get; set; }

        public Good Good { get; set; }

        public Currency Currency { get; set; }

        public Decimal Price { get; set; }
    }
}
