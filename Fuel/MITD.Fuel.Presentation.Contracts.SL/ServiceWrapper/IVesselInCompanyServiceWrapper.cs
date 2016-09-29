using System;
using System.Collections.Generic;
using MITD.Presentation;
using MITD.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;
using System.Collections;

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IVesselInCompanyServiceWrapper : IServiceWrapper
    {
        void GetAll(Action<List<VesselInCompanyDto>, Exception> action, object queryParameters = null);

        void GetAll(Action<PageResultDto<VesselInCompanyDto>, Exception> action, string methodName, int pageSize,
                         int pageIndex);

        void GetById(Action<VesselInCompanyDto, Exception> action, int id, bool includeCompany = true, bool includeTanks = false);

        void GetPagedDataByFilter(Action<PageResultDto<VesselInCompanyDto>, Exception> action, long companyId, int? pageSize, int? pageIndex, bool operatedVessels);

        void GetPagedDataByFilter(Action<PageResultDto<VesselInCompanyDto>, Exception> action, string vesselCode, int? pageSize, int? pageIndex, bool operatedVessels);

        void GetPagedDataByFilter(Action<PageResultDto<VesselInCompanyDto>, Exception> action, long? companyId,
            string vesselCode, bool operatedVessels, int? pageSize, int? pageIndex);

        void GetActivationInfo(Action<VesselActivationDto, Exception> action, string vesselCode);

        void ActivateWarehouseIncludingRecieptsOperation(Action<VesselInCompanyDto, Exception> action, string vesselCode, long companyId, DateTime activationDate,
            List<VesselActivationItemDto> vesselActivationItemDtos);

    }
}
