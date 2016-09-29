using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    /// <summary>
    /// Contains inventory transaction goods data.
    /// </summary>
    public class InventoryResultItem
    {
        public long Id { get; set; }

        public Good Good { get; set; }

        public Currency Currency { get; set; }

        public Decimal Fee { get; set; }

        public Decimal Quantity { get; set; }
        public long TransactionId { get; set; }
    }
}
