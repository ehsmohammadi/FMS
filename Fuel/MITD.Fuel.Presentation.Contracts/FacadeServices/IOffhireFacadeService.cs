using System;
using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
    public interface IOffhireFacadeService : IFacadeService
    {
        OffhireDto GetById(long id);
        PageResultDto<OffhireDto> GetPagedData(int pageSize, int pageIndex);
        PageResultDto<OffhireDto> GetPagedDataByFilter(long? companyId, long? vesselId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex);

        OffhireDto Add(OffhireDto dto);
        OffhireDto Update(OffhireDto dto);
        void Delete(long id);

        PageResultDto<OffhireDetailDto> GetPagedOffhireDetailData(long offhireId, int pageSize, int pageIndex);
        OffhireDetailDto GetOffhireDetail(long offhireId, long offhireDetailId);

        //OffhireDetailDto AddOffhireDetail(OffhireDetailDto detailDto);
        //OffhireDetailDto UpdateOffhireDetail(OffhireDetailDto detailDto);
        //void DeleteOffhireDetail(long offhireId, long offhireDetailId);


        PageResultDto<OffhireManagementSystemDto> GetOffhireManagementSystemPagedDataByFilter(long companyId, long vesselId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex);

        OffhireDto PrepareOffhireData(long referenceNumber, long introducerId);

        PricingValueDto GetOffhirePricingValueInVoucherCurrency(long introducerId, long vesselId, DateTime startDateTime, long goodId, long voucherCurrencyId, DateTime voucherDate);
        PricingValueDto GetOffhirePricingValueInMainCurrency(long introducerId, long vesselId, DateTime startDateTime, long goodId);
        List<PricingValueDto> GetOffhirePricingValuesInVoucherCurrency(long introducerId, long vesselId, DateTime startDateTime, long voucherCurrencyId, DateTime voucherDate);
        List<PricingValueDto> GetOffhirePricingValuesInMainCurrency(long introducerId, long vesselId, DateTime startDateTime);

    }
}