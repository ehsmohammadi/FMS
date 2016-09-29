using System;
using MITD.Presentation;
using MITD.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IScrapServiceWrapper : IServiceWrapper
    {
        void GetById(Action<ScrapDto, Exception> action, long id);
        void GetPagedScrapData(Action<PageResultDto<ScrapDto>, Exception> action, int pageSize, int pageIndex);
        void GetPagedScrapDataByFilter(Action<PageResultDto<ScrapDto>, Exception> action, long? companyId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex);

        void AddScrap(Action<ScrapDto, Exception> action, ScrapDto dto);
        void UpdateScrap(Action<ScrapDto, Exception> action, ScrapDto dto);
        void Delete(Action<string, Exception> action, long id);

        void GetPagedScrapDetailData(Action<PageResultDto<ScrapDetailDto>, Exception> action, long scrapId, int pageSize, int pageIndex);
        void GetScrapDetail(Action<ScrapDetailDto, Exception> action, long scrapId, long scrapDetailId);

        void AddScrapDetail(Action<ScrapDetailDto, Exception> action, long scrapId, ScrapDetailDto detailDto);
        void UpdateScrapDetail(Action<ScrapDetailDto, Exception> action, ScrapDetailDto detailDto);
        void DeleteScrapDetail(Action<string, Exception> action, long scrapId, long scrapDetailId);
    }
}