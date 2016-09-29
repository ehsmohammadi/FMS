using System;
using System.Collections.Generic;
using MITD.Presentation;
using MITD.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;



namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IVoyageLogServiceWrapper : IServiceWrapper
    {
        void GetById(Action<VoyageDto, Exception> action, long id);

        void GetAll(Action<PageResultDto<VoyageDto>, Exception> action, int? pageSize, int? pageIndex);

        void GetByFilter(Action<PageResultDto<VoyageDto>, Exception> action, long companyId, long vesselId, int? pageSize, int? pageIndex);

        void GetChenageHistory(Action<List<VoyageDto>, Exception> action, long voyageId);

    }
}
