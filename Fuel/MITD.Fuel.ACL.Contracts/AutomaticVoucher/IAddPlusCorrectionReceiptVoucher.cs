﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.ACL.Contracts.AutomaticVoucher
{
  public  interface IAddPlusCorrectionReceiptVoucher:IAutomaticVoucher
    {
      void Execute(FuelReport fuelReport, List<Receipt> issues,
    string receiptWarehouseCode, string issueNumber, long userId);

    }
}