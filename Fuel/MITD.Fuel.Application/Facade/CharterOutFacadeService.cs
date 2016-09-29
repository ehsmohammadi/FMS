using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Application.Service.Security;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class CharterOutFacadeService : ICharterOutFacadeService
    {
        #region Prop
        static ObservableCollection<CharterDto> CharterDtos;

        private ICharterOutApplicationService _charterOutApplicationService;
        private ICharterOutRepository _charterOutRepository;
        private ICharterOutToDtoMapper _charterOutToDtoMapper;
        private ICharterItemToDtoMapper _charterItemToDtoMapper;
        private IInventoryOperationToInventoryOperationDtoMapper _inventoryOperationDtoMapper;
        private readonly IFuelUserRepository fuelUserRepository;

        #endregion


        #region Ctor

        public CharterOutFacadeService(

         ICharterOutApplicationService charterOutApplicationService,
         ICharterOutRepository charterOutRepository,
         ICharterOutToDtoMapper charterOutToDtoMapper,
         ICharterItemToDtoMapper charterItemToDtoMapper,
            IInventoryOperationToInventoryOperationDtoMapper inventoryOperationDtoMapper, IFuelUserRepository fuelUserRepository)
        {
            _charterOutApplicationService = charterOutApplicationService;
            _charterOutRepository = charterOutRepository;
            _charterOutToDtoMapper = charterOutToDtoMapper;
            _charterItemToDtoMapper = charterItemToDtoMapper;
            _inventoryOperationDtoMapper = inventoryOperationDtoMapper;
            this.fuelUserRepository = fuelUserRepository;
        }

        #endregion


        #region Method

        public PageResultDto<CharterDto> GetAll(long vesselInCompanyId,long companyId, long id, DateTime? startdate, DateTime? enddate, int pageIndex, int pageSize)
        {
            var data = _charterOutRepository.GetByFilter(vesselInCompanyId, companyId, id, startdate, enddate, pageSize, pageIndex);
            var res = _charterOutToDtoMapper.MapToDtoModels(data);

      
            res.Result.ToList().ForEach(c => c.EndDateStr = SetEndDate(c.Id));

            return res;
        }

        string SetEndDate(long startId)
        {
            var end = _charterOutRepository.GetCharterEnd(startId);
            return (end != null && end.CurrentState == States.Submitted) ? end.ActionDate.ToShortDateString() : "-------";
        }

        public CharterDto GetCharterEnd(long startId)
        {
            var entity = _charterOutRepository.GetCharterEnd(startId);
            var res = new CharterDto();

            if (entity != null)
            {
                res = _charterOutToDtoMapper.MapToDtoModel(entity);

                if (entity.CharterItems.Count > 0)
                {
                    var items = _charterItemToDtoMapper.MapToDtoModels(
                        new PageResult<CharterItem>() {Result = entity.CharterItems});
                    items.Result.ToList().ForEach(c => res.CharterItems.Add(c));
                }

                if (entity.InventoryOperationItems.Count > 0)
                {
                    var invOperts = _charterOutToDtoMapper.MapToInvDtoModels(
                        new PageResult<InventoryOperation>() {Result = entity.InventoryOperationItems});
                    invOperts.Result.ToList().ForEach(c => res.InventoryOperationDtos.Add(c));
                }
                return res;
            }
            else
            {
                return null;
            }
            
        }


        public CharterDto GetById(long id)
        {
            var entity = _charterOutRepository.GetCharterStartById(id);
            var res = _charterOutToDtoMapper.MapToDtoModel(entity);
            if (entity.CharterItems.Count > 0)
            {
                var items = _charterItemToDtoMapper.MapToDtoModels(
               new PageResult<CharterItem>() { Result = entity.CharterItems });
                items.Result.ToList().ForEach(c => res.CharterItems.Add(c));
            }

            if (entity.InventoryOperationItems.Count > 0)
            {
                var invOperts = _charterOutToDtoMapper.MapToInvDtoModels(
                new PageResult<InventoryOperation>() { Result = entity.InventoryOperationItems });
                invOperts.Result.ToList().ForEach(c => res.InventoryOperationDtos.Add(c));
            }
            return res;
        }

        public void Add(CharterDto data)
        {
            if (data.CharterStateType == CharterStateTypeEnum.Start)
            {
                _charterOutApplicationService.AddStart(data.Charterer.Id, data.Owner.Id, data.VesselInCompany.Id,
                    data.Currency.Id, data.StartDate, this._charterOutToDtoMapper.DtoToOfHreConvertor(data.OffHirePricingType), getFuelUserId());
            }
            else
            {
                _charterOutApplicationService.AddEnd(data.Charterer.Id, data.Owner.Id, data.VesselInCompany.Id,
                      data.Currency.Id, data.EndDate, this._charterOutToDtoMapper.CharterEndTypeEnumConvertor(data.CharterEndType), this._charterOutToDtoMapper.DtoToOfHreConvertor(data.OffHirePricingType), getFuelUserId());
            }


        }

        public void Update(CharterDto data)
        {
            if (data.CharterStateType == CharterStateTypeEnum.Start)
            {
                _charterOutApplicationService.UpdateStart(data.Id, data.Charterer.Id, data.Owner.Id, data.VesselInCompany.Id,
                    data.Currency.Id, data.StartDate, _charterOutToDtoMapper.DtoToOfHreConvertor(data.OffHirePricingType));
            }
            else
            {
                _charterOutApplicationService.UpdateEnd(data.Id, data.Charterer.Id, data.Owner.Id, data.VesselInCompany.Id,
                      data.Currency.Id, data.EndDate, _charterOutToDtoMapper.CharterEndTypeEnumConvertor(data.CharterEndType));
            }
        }

        public void Delete(long id)
        {
            _charterOutApplicationService.Delete(id);
        }

        public PageResultDto<CharterItemDto> GetAllItem(long charterId, int pageIndex, int pageSize)
        {
            var entity = _charterOutRepository.GetById(charterId).CharterItems;
            var res = new PageResultDto<CharterItemDto>();
            res.Result = new List<CharterItemDto>();
            entity.ForEach(c =>
            {
                res.Result.Add(_charterItemToDtoMapper.MapToDtoModel(c));
            });

            return res;
        }

        public CharterItemDto GetItemById(long id, long charterItemId)
        {
            var entity = _charterOutRepository.GetById(id).CharterItems;
            return _charterItemToDtoMapper.MapToDtoModel(entity.Single(c => c.Id == charterItemId));
        }

        public void AddItem(CharterItemDto dto)
        {
            _charterOutApplicationService.AddItem(dto.Id, dto.CharterId, dto.Rob, dto.Fee, dto.FeeOffhire, dto.Good.Id, dto.TankDto.Id, dto.Good.Unit.Id);


        }

        public void UpdateItem(CharterItemDto dto)
        {
            _charterOutApplicationService.UpdateItem(dto.Id, dto.CharterId, dto.Rob, dto.Fee, dto.FeeOffhire, dto.Good.Id, dto.TankDto.Id, dto.Good.Unit.Id);
        }

        public void DeleteItem(long id, long charterItemId)
        {
            _charterOutApplicationService.DeleteItem(charterItemId, id);
        }

        //================================================================================

        private long getFuelUserId()
        {
            var currentUserId = SecurityApplicationService.GetCurrentUserId();
            var currentCompanyId = SecurityApplicationService.GetCurrentUserCompanyId();

            return fuelUserRepository.Single(fu => fu.IdentityId == currentUserId && fu.CompanyId == currentCompanyId).Id;
        }


        //================================================================================
        #endregion

        public CharterDto GetCharterStart(long id,long vesselInCompanyId, long chartererId)
        {
            return _charterOutToDtoMapper.MapToDtoModel(_charterOutRepository.GetCharterStart(id,vesselInCompanyId, chartererId));
        }
       
    }
}
