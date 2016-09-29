using System.Collections.Generic;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Fuel.Application.Facade.Contracts.Mappers;

namespace MITD.Fuel.Application.Facade
{
    public class TransferTypeFacadeService : ITransferTypeFacadeService
    {
        #region Prop
        public ITransferTypeApplicationService TransferTypeApplicationService  { get; set; }
        public ITransferTypeToTransferTypeDtoMapper TransferTypeToTransferTypeDtoMapper { get; set; }

        #endregion

        #region Ctor

        public TransferTypeFacadeService(ITransferTypeApplicationService transferTypeApplicationService, ITransferTypeToTransferTypeDtoMapper transferTypeToTransferTypeDtoMapper)
        {
            this.TransferTypeApplicationService = transferTypeApplicationService;
            this.TransferTypeToTransferTypeDtoMapper = transferTypeToTransferTypeDtoMapper;
        }
        
        #endregion

        #region Method

        public List<TransferTypeDto> GetAll()
        {
            return
                this.TransferTypeToTransferTypeDtoMapper.MapEntityToDto(this.TransferTypeApplicationService.GetAll());
        }

        #endregion
        
        
       
    }
}