using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface IScrapController
    {
        void ShowList();
        void Add();
        void Edit(ScrapDto scrapDto);
        void AddScrapDetail(ScrapDto scrapDto);
        void EditScrapDetail(ScrapDto scrapDto, ScrapDetailDto scrapDetailDto);

        //void Delete(ScrapDto dto);
    }
}
