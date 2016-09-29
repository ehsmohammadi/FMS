using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.CharterStates;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.CharterTypes;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate
{
    public class Charter
    {

        #region Prop

        public States CurrentState { get; private set; }

        public long Id { get; private set; }
        public DateTime ActionDate { get; private set; }

        public CharterType CharterType { get; private set; }
        public CharterEndType CharterEndType { get; private set; }

        public CharterState CharterState { get; set; }

        public long? ChartererId { get; private set; }
        public virtual Company Charterer { get; private set; }

        public long? OwnerId { get; private set; }
        public virtual Company Owner { get; private set; }

        public long? VesselInCompanyId { get; private set; }
        public virtual VesselInCompany VesselInCompany { get; private set; }

        public long? CurrencyId { get; private set; }
        public virtual Currency Currency { get; private set; }

        public virtual List<CharterItem> CharterItems { get; private set; }

        public virtual List<Attachment> Attachments { get; private set; }

        public virtual List<InventoryOperation> InventoryOperationItems { get; private set; }
        public virtual List<CharterWorkflowLog> ApproveWorkflows { get; private set; }
        public byte[] TimeStamp { get; set; }

        protected ICharterInDomainService _charterInDomainService;
        protected ICharterOutDomainService _charterOutDomainService;
        protected IEventPublisher _eventPublisher;

        protected IInventoryOperationNotifier InventoryOperationNotifier;
        protected IVesselInCompanyDomainService VesselInCompanyDomainService;

        #endregion

        #region Ctor

        public Charter()
        {
            ApproveWorkflows = new List<CharterWorkflowLog>();
            InventoryOperationItems = new List<InventoryOperation>();
        }

        public Charter(long id, long chartererId, long ownerId, long vesselInCompanyId, long CurrencyId,
                           DateTime actionDate, List<CharterItem> charterItems,
                           List<InventoryOperation> inventoryOperationItems,
                           CharterType charterType, CharterEndType charterEndType,
            ICharterInDomainService charterInDomainService, ICharterOutDomainService charterOutDomainService
            , IEventPublisher eventPublisher)
            : this()
        {



            this.Id = id;
            this.ChartererId = chartererId;
            this.OwnerId = ownerId;
            this.VesselInCompanyId = vesselInCompanyId;
            this.CurrencyId = CurrencyId;
            this.ActionDate = actionDate;
            this.CharterItems = charterItems;
            this.InventoryOperationItems = inventoryOperationItems;
            this.CharterType = charterType;
            this.CharterEndType = charterEndType;
            _charterInDomainService = charterInDomainService;
            _charterOutDomainService = charterOutDomainService;
            _eventPublisher = eventPublisher;
        }


        #endregion

        #region Method

        public void Resolve(ICharterInDomainService charterInDomainService, ICharterOutDomainService charterOutDomainService
            , IEventPublisher eventPublisher, IVesselInCompanyDomainService vesselInCompanyDomainService, IInventoryOperationNotifier inventoryOperationNotifier)
        {
            _charterInDomainService = charterInDomainService;
            _charterOutDomainService = charterOutDomainService;
            _eventPublisher = eventPublisher;
            InventoryOperationNotifier = inventoryOperationNotifier;
            VesselInCompanyDomainService = vesselInCompanyDomainService;
        }

        //B5
        internal void IsNotDuplicateItem(long tankId, long goodId)
        {
            if (this.CharterItems.Exists(c => c.TankId == tankId && c.GoodId == goodId))
                throw new BusinessRuleException("B5", "CharterItem Is Duplicate");
        }

        //B5
        internal void IsNotDuplicateItemUpdate(long id, long tankId, long goodId)
        {
            var origin = this.CharterItems.SingleOrDefault(c => c.Id == id);

            if (origin != null)
                if (origin.TankId != tankId || origin.GoodId != goodId)
                    if (this.CharterItems.Exists(c => c.TankId == tankId && c.GoodId == goodId))
                        throw new BusinessRuleException("B5", "CharterItem Is Duplicate");
        }

        //B9 chrter 
        public virtual void IsGoodAndUnitValidInCompany(long goodId, long goodUnitId)
        {


        }


        //B12
        protected void HasTank(long vesselInCompanyId, long tankId)
        {
            IVesselInCompanyDomainService vesselDomainService = ServiceLocator.Current.GetInstance<IVesselInCompanyDomainService>();

            var vessel = vesselDomainService.Get(vesselInCompanyId);
            if (vessel.Tanks.Count > 0)
                if (!(tankId > 0))
                    throw new BusinessRuleException("B12", "Tank Should Select");
        }




        //B32,B51
        protected void IsValidateCharterEnd(DateTime actionDate, long currencyId, CharterEndType charterEndType)
        {
            if (!(actionDate != null
                && currencyId > 0
                && charterEndType != CharterEndType.None))
                throw new BusinessRuleException("B32", "Charter End Invalid");
        }

        //B64
        protected void CanIssuance()
        {
            IInventoryManagementDomainService inventoryManagementDomainService
                = ServiceLocator.Current.GetInstance<IInventoryManagementDomainService>();
            if (!(inventoryManagementDomainService.CanIssuance(this.VesselInCompanyId.Value)))
                throw new BusinessRuleException("B64", "Can not Issue");

        }

        //B65
        protected void CanRecipt()
        {
            IInventoryManagementDomainService inventoryManagementDomainService
                = ServiceLocator.Current.GetInstance<IInventoryManagementDomainService>();
            if (!(inventoryManagementDomainService.CanRecipt(this.VesselInCompanyId.Value)))
                throw new BusinessRuleException("B64", "Can not Recipt");

        }

        //B74 
        protected void IsNotSameChatererOwner(long OwnerId, long ChartererId)
        {
            if (OwnerId == ChartererId)
                throw new BusinessRuleException("B74", "Charterer and Owner should not be the same");
        }

        protected void UpdateprivateProp(long id, long chartererId, long ownerId, long vesselInCompanyId, long CurrencyId,
                         DateTime actionDate, List<CharterItem> charterItems,
                         List<InventoryOperation> inventoryOperationItems,
                         CharterType charterType, CharterEndType charterEndType
                         )
        {
            this.Id = id;
            this.ChartererId = chartererId;
            this.CurrencyId = CurrencyId;

            this.OwnerId = ownerId;
            this.VesselInCompanyId = vesselInCompanyId;
            this.ActionDate = actionDate;
            this.CharterType = charterType;
            this.CharterEndType = charterEndType;


        }

        public void SetStateType(CharterState state)
        {
            CharterState = state;
        }

        public void SetStateType(States state)
        {
            CurrentState = state;
        }


        public virtual void Approve(long approverId)
        {


        }
        public virtual void Submited(long approverId)
        {


        }
        public virtual void RejectSubmited(long approverId) { }




        #endregion


    }
}
