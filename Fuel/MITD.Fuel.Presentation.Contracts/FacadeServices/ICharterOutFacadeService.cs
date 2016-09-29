using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
  public  interface ICharterOutFacadeService :IFacadeService
    {
      PageResultDto<CharterDto> GetAll(long vesselInCompanyId, long companyId, long id, DateTime? startdate, DateTime? enddate, int pageIndex, int pageSize);
        CharterDto GetById(long id);
        CharterDto GetCharterEnd(long startId);
        CharterDto GetCharterStart(long id, long vesselInCompanyId, long chartererId);
        void Add(CharterDto data);
        void Update(CharterDto data);
        void Delete(long id);



        PageResultDto<CharterItemDto> GetAllItem(long charterId, int pageIndex, int pageSize);
        CharterItemDto GetItemById(long id, long charterItemId);
        void AddItem(CharterItemDto data);
        void UpdateItem(CharterItemDto data);
        void DeleteItem(long id, long charterItemId);
    }
}
