using System;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;
using System.Collections.Generic;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices.Fuel
{
    public interface IVoyageFacadeService : IFacadeService
    {
        List<VoyageDto> GetAll(bool includeFuelReports = false, bool includeInventoryOperations = false);

        List<VoyageDto> GetByFilter(long companyId, long vesselInCompanyId, bool includeFuelReports = false, bool includeInventoryOperations = false);

        VoyageDto GetById(long id, bool includeFuelReports = false, bool includeInventoryOperations = false);

        PageResultDto<VoyageDto> GetPagedData(int pageSize, int pageIndex, bool includeFuelReports = false, bool includeInventoryOperations = false);

        PageResultDto<VoyageDto> GetPagedDataByFilter(long companyId, long vesselInCompanyId, int pageSize, int pageIndex, bool includeFuelReports = false, bool includeInventoryOperations = false);

        PageResultDto<VoyageLogDto> GetChenageHistory(long voyageId, int pageSize, int pageIndex);

        List<VoyageDto> FindVoyages(long companyId, long vesselInCompanyId, DateTime lookingDateTime);

        void UpdateVoyagesFromRotationData();
    }
}
