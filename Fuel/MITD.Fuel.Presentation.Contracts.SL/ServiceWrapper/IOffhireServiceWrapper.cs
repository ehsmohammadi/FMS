using System;
using System.Collections.Generic;
using MITD.Presentation;
using MITD.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IOffhireServiceWrapper : IServiceWrapper
    {
        void GetById(Action<OffhireDto, Exception> action, long id);
        void GetPagedOffhireData(Action<PageResultDto<OffhireDto>, Exception> action, int pageSize, int pageIndex);
        void GetPagedOffhireDataByFilter(Action<PageResultDto<OffhireDto>, Exception> action, long? companyId, long? vesselId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex);

        void GetOffhirePreparedData(Action<OffhireDto, Exception> action, long referenceNumber, long introducerId);
        void GetOffhireManagementSystemPagedData(Action<PageResultDto<OffhireManagementSystemDto>, Exception> action, long companyId, long vesselId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex);


        void AddOffhire(Action<OffhireDto, Exception> action, OffhireDto dto);
        void UpdateOffhire(Action<OffhireDto, Exception> action, OffhireDto dto);
        void Delete(Action<string, Exception> action, long id);

        void GetPagedOffhireDetailData(Action<PageResultDto<OffhireDetailDto>, Exception> action, long offhireId, int pageSize, int pageIndex);
        void GetOffhireDetail(Action<OffhireDetailDto, Exception> action, long offhireId, long offhireDetailId);

        void GetPricingValues(Action<List<PricingValueDto>, Exception> action, long introducerId, long vesselId, DateTime startDateTime);

        //void UpdateOffhireDetail(Action<OffhireDetailDto, Exception> action, OffhireDetailDto detailDto);
    }
}