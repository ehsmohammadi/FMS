using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public class GoodTrustReceiveData
    {
        public Good Good { get; set; }

        public decimal Quantity { get; set; }

        public GoodUnit GoodUnit { get; set; }
    }
}
