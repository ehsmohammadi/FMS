using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;

namespace MITD.Fuel.Application.Service
{
    public class CharterOutApplicationService : ICharterOutApplicationService
    {
         #region Prop

        private ICharterOutRepository _charterOutRepository;
        private CharterFactory _charterFactory;
        private ICharterOutToDtoMapper _charterOutToDtoMapper;
        private IUnitOfWorkScope _unitOfWorkScope;
        private readonly IVesselInCompanyDomainService vesselInCompanyDomainService;

        #endregion


        #region ctor

        public CharterOutApplicationService(ICharterOutRepository charterOutRepository,
            ICharterOutToDtoMapper charterOutToDtoMapper, IUnitOfWorkScope unitOfWorkScope
            , IWorkflowStepRepository _workflowStepRepository,ICharterOutDomainService charterOutDomainService,
            ICharterInDomainService charterInDomainService, IEventPublisher eventPublisher, IVesselInCompanyDomainService vesselInCompanyDomainService, IInventoryOperationNotifier inventoryOperationNotifier)
        {
            this._charterOutRepository = charterOutRepository;
            this._charterOutToDtoMapper = charterOutToDtoMapper;
            this._unitOfWorkScope = unitOfWorkScope;
            this.vesselInCompanyDomainService = vesselInCompanyDomainService;

            _charterFactory = new CharterFactory(_workflowStepRepository,
                charterInDomainService, charterOutDomainService, eventPublisher, vesselInCompanyDomainService, inventoryOperationNotifier);
        }

        #endregion


        #region Method

        public void AddStart( long chartererId, long ownerId, long vesselInCompanyId, long currencyId,
                           DateTime actionDate,
                           OffHirePricingType offHirePricingType, long userId)
        {
          var entity=  _charterFactory.CreateCharterOut(0 ,chartererId, ownerId, vesselInCompanyId, currencyId, actionDate, null, null,
                                                        CharterType.Start, CharterEndType.None, offHirePricingType, userId);
          _charterOutRepository.Add(entity);
            _unitOfWorkScope.Commit();

        }

        public void AddEnd( long chartererId, long ownerId, long vesselInCompanyId, long currencyId,
                         DateTime actionDate
                        , CharterEndType charterEndType, OffHirePricingType offHirePricingType, long userId)
        {
            var entity = _charterFactory.CreateCharterOut( 0,chartererId, ownerId, vesselInCompanyId, currencyId, actionDate, null, null,
                                              CharterType.End, charterEndType, offHirePricingType, userId);
            _charterOutRepository.Add(entity);
            _unitOfWorkScope.Commit();

        }

        public void UpdateStart(long id, long chartererId, long ownerId, long vesselInCompanyId, long currencyId,
                            DateTime actionDate,
                            OffHirePricingType offHirePricingType)
        {
            var entity =_charterFactory.ReCreateCharterOut(_charterOutRepository.GetCharterStartById(id));
            if (entity == null)
                throw new ObjectNotFound("Charter Out", id);
            entity.Update(id,  chartererId,  ownerId,  vesselInCompanyId,  currencyId,
                             actionDate,null,null,CharterType.Start, CharterEndType.None,
                             offHirePricingType);

            _charterOutRepository.Update(entity);
            try
            {
              _unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException exception)
            {

                throw new ConcurencyException("Charter Out Start"); 
            }
            
            

        }

        public void UpdateEnd(long id, long chartererId, long ownerId, long vesselInCompanyId, long currencyId,
                           DateTime actionDate,
                           CharterEndType charterEndType)
        {
            var entity = _charterFactory.ReCreateCharterOut(_charterOutRepository.GetCharterStartById(id));
            if (entity == null)
                throw new ObjectNotFound("Charter Out", id);
            entity.Update(id, chartererId, ownerId, vesselInCompanyId, currencyId,
                             actionDate, null, null, CharterType.End, charterEndType,OffHirePricingType.IssueBase);

            _charterOutRepository.Update(entity);
            try
            {
                _unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException exception)
            {

                throw new ConcurencyException("Charter Out Start");
            }



        }


        public void Delete(long id)
        {
            var entity = _charterFactory.ResolveCharterOut(_charterOutRepository.GetCharterStartById(id));
            if (entity==null)
                throw new ObjectNotFound("Charter Out", id);
            
            entity.DeleteCheckRule();
            _charterOutRepository.Delete(entity);

            try
            {
                _unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException exception)
            {

                throw new ConcurencyException("Charter Out ");
            }

        }

        public void AddItem(long id, long charterId, decimal rob, decimal fee, decimal feeOffhire,
                         long goodId, long tankId, long unitId)
        {
            var entity = _charterFactory.ReCreateCharterOut(_charterOutRepository.GetById(charterId));
            if (entity == null)
                throw new ObjectNotFound("Charter Out", charterId);
            var item = _charterFactory.CraeteCharterItem(id, charterId, rob, fee, feeOffhire,
                                                         goodId, tankId, unitId);

           
            entity.AddItem(item);
           _unitOfWorkScope.Commit();
        }

        public void UpdateItem(long id, long charterId, decimal rob, decimal fee, decimal feeOffhire, long goodId, long tankId, long unitId)
        {
            var entity = _charterFactory.ReCreateCharterOut(_charterOutRepository.GetById(charterId));
            if (entity == null)
                throw new ObjectNotFound("Charter In", charterId);
            entity.UpdateItem( id,  charterId,  rob,  fee,  feeOffhire,  goodId,  tankId,  unitId);
            try
            {
                _unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException exception)
            {

                throw new ConcurencyException("Charter In Item");
            }

        }

        #endregion


        public void DeleteItem(long id, long charterId)
        {
            var entity =_charterFactory.ReCreateCharterOut( _charterOutRepository.GetById(charterId));
            if (entity == null)
                throw new ObjectNotFound("Charter In", charterId);

            entity.DeleteItem(id);
            try
            {
                _unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("Charter In Item");
            }
        }
    }
}
