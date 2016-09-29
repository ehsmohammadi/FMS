using System.Collections.Generic;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Fuel.Application.Facade.Contracts.Mappers;

namespace MITD.Fuel.Application.Facade
{
    public class ReciveTypeFacadeService : IRecieveTypeFacadeService
    {

        #region Prop

        public IReceiveTypeApplicationService ReceiveTypeApplicationService { get; set; }
        public IRecieveTypeToRecieveTypeDtoMapper RecieveTypeToRecieveTypeDtoMapper { get; set; }

        #endregion

        #region Ctor

        public ReciveTypeFacadeService(IReceiveTypeApplicationService receiveTypeApplicationService,
                                       IRecieveTypeToRecieveTypeDtoMapper recieveTypeToRecieveTypeDtoMapper)
        {
            this.ReceiveTypeApplicationService = receiveTypeApplicationService;
            this.RecieveTypeToRecieveTypeDtoMapper = recieveTypeToRecieveTypeDtoMapper;

        }

        #endregion

        #region Method

        public List<ReceiveTypeDto> GetAll()
        {
           return this.RecieveTypeToRecieveTypeDtoMapper.MapEntityToDto(this.ReceiveTypeApplicationService.GetAll());
        }

        #endregion


    }


}