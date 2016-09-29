using System;
using System.Collections.Generic;
using MITD.Presentation;
using MITD.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;
using System.Collections.ObjectModel;



namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IVoyageServiceWrapper : IServiceWrapper
    {
        void GetById(Action<VoyageDto, Exception> action, long id);

        void GetAll(Action<PageResultDto<VoyageDto>, Exception> action, int? pageSize, int? pageIndex);

        void GetByFilter(Action<PageResultDto<VoyageDto>, Exception> action, long companyId, long vesselId, int? pageSize, int? pageIndex);

        void GetChenageHistory(Action<PageResultDto<VoyageLogDto>, Exception> action, long voyageId, int pageSize, int pageIndex);

        void FindVoyages(Action<ObservableCollection<VoyageDto>, Exception> action, long companyId, long vesselId, DateTime lookingDateTime);

        void UpdateVoyagesFromRotationData(Action<object, Exception> action);
    }
}
