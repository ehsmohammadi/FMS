using System;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.Commands;
using MITD.Presentation.Contracts;
using MITD.Services.Application;

namespace MITD.Fuel.Application.Service.Contracts
{
    public interface IOffhireApplicationService : IApplicationService
    {
        Offhire AddOffhire(OffhireCommand command, long userId);
        Offhire UpdateOffhire(OffhireCommand command);
        void DeleteOffhire(long offhireId);

        //OffhireDetail AddOffhireDetail(long offhireId, decimal quantity, decimal feeInVoucherCurrency, decimal feeInMainCurrency, long goodId, long unitId, long tankId);
        //OffhireDetail UpdateOffhireDetail(long offhireId, long offhireDetailId, double rob, double price, long currencyId, long goodId, long unitId, long tankId);
        //void DeleteOffhireDetail(long offhireId, long offhireDetailId);

        OffhirePreparedData GetPreparedData(long referenceNumber, long introducerId);

        PricingValue GetOffhirePricingValueInVoucherCurrency(long introducerId, long vesselInCompanyId, DateTime startDateTime, long goodId, long voucherCurrencyId, DateTime voucherDate);
        PricingValue GetOffhirePricingValueInMainCurrency(long introducerId, long vesselInCompanyId, DateTime startDateTime, long goodId);
        List<PricingValue> GetOffhirePricingValuesInVoucherCurrency(long introducerId, long vesselInCompanyId, DateTime startDateTime, long voucherCurrencyId, DateTime voucherDate);
        List<PricingValue> GetOffhirePricingValuesInMainCurrency(long introducerId, long vesselInCompanyId, DateTime startDateTime);
    }
}