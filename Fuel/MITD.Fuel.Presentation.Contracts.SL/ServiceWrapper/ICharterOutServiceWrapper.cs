using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface ICharterOutServiceWrapper : IServiceWrapper
    {
        void GetByFilter(Action<PageResultDto<CharterDto>, Exception> action, long companyId, long id, string start,
            string end, int pageIndex, int pageSize, long vesselInCompanyId);
        void GetById(Action<CharterDto, Exception> action, CharterStateTypeEnum charterStateTypeEnum, long id);
        void GetCharterStart(Action<CharterDto, Exception> action, long vesselInCompanyId, long chartererId, long id);
        void Add(Action<CharterDto, Exception> action, CharterDto charterDto);
        void Update(Action<CharterDto, Exception> action, long id, CharterDto charterDto);
        void Delete(Action<string, Exception> action, long id);

        void GetItems(Action<PageResultDto<CharterItemDto>, Exception> action, CharterStateTypeEnum stateTypeEnum, long companyId, int pageIndex, int pageSize);

        void GetItemById(Action<CharterItemDto, Exception> action, CharterStateTypeEnum stateTypeEnum, long id,
                         long charerItemId);
        void AddItem(Action<CharterItemDto, Exception> action, CharterStateTypeEnum stateTypeEnum, CharterItemDto charterDto);

        void UpdateItem(Action<CharterItemDto, Exception> action, long id, long charterItemId,
                        CharterStateTypeEnum stateTypeEnum, CharterItemDto charterDto);
        void DeleteItem(Action<string, Exception> action, CharterStateTypeEnum stateTypeEnum, long id, long charterItemId);

        void GetAllOwner(Action<PageResultDto<CompanyDto>, Exception> action);
        void GetAllIdelVessels(Action<PageResultDto<VesselInCompanyDto>, Exception> action, long companyId);
        void GetByIdVessel(Action<VesselInCompanyDto, Exception> action, long id);
        void GetAllGoods(Action<List<GoodDto>, Exception> action, long companyId);
        void GetAllCurrencies(Action<List<CurrencyDto>, Exception> action);
    }
}
