﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public enum FinancialVoucherStates
    {
        Sent = 1,
        NotSent = 2,
        Error = 3
    }
}