using System;
using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
    public interface IScrapFacadeService : IFacadeService
    {
        ScrapDto GetById(long id);
        PageResultDto<ScrapDto> GetPagedData(int pageSize, int pageIndex);
        PageResultDto<ScrapDto> GetPagedDataByFilter(long? companyId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex);

        ScrapDto ScrapVessel(ScrapDto dto);
        ScrapDto Update(ScrapDto dto);
        void Delete(long id);

        PageResultDto<ScrapDetailDto> GetPagedScrapDetailData(long scrapId, int pageSize, int pageIndex);
        ScrapDetailDto GetScrapDetail(long scrapId, long scrapDetailId);

        ScrapDetailDto AddScrapDetail(ScrapDetailDto detailDto);
        ScrapDetailDto UpdateScrapDetail(ScrapDetailDto detailDto);
        void DeleteScrapDetail(long scrapId, long scrapDetailId);

        PageResultDto<FuelReportInventoryOperationDto> GetInventoryOperations(long id, int? pageSize, int? pageIndex);
    }
}