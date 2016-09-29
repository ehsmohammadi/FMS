using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class InventoryResult
    {
        public long Id { get; set; }

        public string Number { get; set; }

        public List<InventoryResultItem> InventoryResultItems { get; set; }

    }
}
