using System;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Factories;

namespace MITD.Fuel.Domain.Model.DomainObjects.Factories
{
    public interface IOffhireFactory : IFactory
    {
        Offhire CreateOffhire(long referenceNumber, DateTime startDateTime, DateTime endDateTime, Company introducer, VesselInCompany vesselInCompany, Voyage voyage, ActivityLocation offhireLocation, DateTime voucherDate, Currency voucherCurrency, long userId);

        OffhireDetail CreateOffhireDetail(decimal quantity, decimal feeInVoucherCurrency, decimal feeInMainCurrency, Good good, GoodUnit unit, Tank tank, Offhire offhire);
    }
}