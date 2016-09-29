using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.CharterTypes;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.DomainServices;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Events;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Specifications;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Extensions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;


namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate
{
    public class CharterOut : Charter
    {
        #region Prop


        private CharterTypeBase<CharterOut> charterTypeBase;
        public OffHirePricingType OffHirePricingType { get; set; }
        private IGoodRepository _goodRepository;

        #endregion

        #region ctor

        public CharterOut()
        {

        }

        public CharterOut(long id, long chartererId, long ownerId, long vesselInCompanyId, long CurrencyId,
                          DateTime actionDate, List<CharterItem> charterItems,
                          List<InventoryOperation> inventoryOperationItems,
                          CharterType charterType, CharterEndType charterEndType,
                          OffHirePricingType offHirePricingType, ICharterInDomainService charterInDomainService
            , ICharterOutDomainService charterOutDomainService, IEventPublisher eventPublisher)
            : base(id, chartererId, ownerId, vesselInCompanyId, CurrencyId,
                  actionDate, charterItems, inventoryOperationItems,
                  charterType, charterEndType, charterInDomainService, charterOutDomainService, eventPublisher)
        {

            this.OffHirePricingType = offHirePricingType;
            this._eventPublisher.RegisterHandler<CharterOutApproveArg>(new InventorySubscriber());
            this._eventPublisher.RegisterHandler<CharterOutFinalApproveArg>(new InventorySubscriber());
            this._eventPublisher.RegisterHandler<CharterOutDisApproveArg>(new InventorySubscriber());

            #region bz

            if (this.CharterType == CharterType.Start)
            {

                //B74
                IsNotSameChatererOwner(ownerId, chartererId);
                //B29
                IsValidateCharterOutStartProp();
                //B23
                IsValidStartDate();
                //B2 
                IsValidCharterInStartDate();
                //B22
                IsVesselOwnedOrCharterIn();
                //B28
                HasVesselCharterOutStart(vesselInCompanyId);
                //B59
                CompareOffhirePricing(offHirePricingType);
                //B90
                IsEqualDateCharterInStart(actionDate)
                ;

            }
            else
            {
                //B36
                CheckPeriodCharterInValid(this.Id);
                //B37
                IsNextCharterStartDateGreaterThanActionDate(this.Id);
                //B31
                ExistFinalApprove(this.Id);
                //B35
                IsNotVesselCharterIn(VesselInCompanyId.Value);
                //B43
                HasVesselCharterOutEnd(VesselInCompanyId.Value);
                //B45
                IsValidEndDate(this.Id);
                //51
                IsValidateCharterEnd(actionDate, CurrencyId, charterEndType);

                //B34

                //91
                IsEqualDateCharterInEnd(actionDate);

            }
            #endregion


        }

        #endregion

        #region Opert

        internal void SetCharterType(CharterTypeBase<CharterOut> charterTypeBase)
        {
            this.charterTypeBase = charterTypeBase;
        }


        public void Update(long id, long chartererId, long ownerId, long vesselInCompanyId, long CurrencyId,
                       DateTime actionDate, List<CharterItem> charterItems,
                       List<InventoryOperation> inventoryOperationItems,
                       CharterType charterType, CharterEndType charterEndType, OffHirePricingType offHirePricingType)
        {


            #region bz
            if (CharterType == CharterType.Start)
            {

                //B74
                IsNotSameChatererOwner(ownerId, chartererId);
                //B18
                IsValidChangeOffhirePricing(offHirePricingType);

                //B19
                IsNotFinalApprove();

                //B22
                IsVesselOwnedOrCharterIn();

                //B23
                IsValidStartDate();

                //B29
                IsValidateCharterOutStartProp();

                //B2 
                IsValidCharterInStartDate();

                //B59
                CompareOffhirePricing(offHirePricingType);

                //B30
                IsChangePropRejectSubmitted(chartererId, vesselInCompanyId, CurrencyId,
                                            actionDate);
                //B90
                IsEqualDateCharterInStart(actionDate)
                ;

            }
            else
            {
                //B31
                ExistFinalApprove(this.Id);
                //B35
                IsNotVesselCharterIn(VesselInCompanyId.Value);
                //B36
                CheckPeriodCharterInValid(this.Id);
                //B37
                IsNextCharterStartDateGreaterThanActionDate(this.Id);
                //B45
                IsValidEndDate(this.Id);
                //B51
                IsValidateCharterEnd(actionDate, CurrencyId, charterEndType);
                //B33
                IsNotFinalApprove();

                //B44
                IsChangeValidForSubmitRejectedCharter(actionDate, CurrencyId, charterEndType);

                //B34

                //91
                IsEqualDateCharterInEnd(actionDate);

            }

            #endregion
            this.OffHirePricingType = offHirePricingType;
            this.UpdateprivateProp(id, chartererId, ownerId, vesselInCompanyId, CurrencyId,
                          actionDate, charterItems,
                         inventoryOperationItems,
                          charterType, charterEndType);

        }

        public void DeleteCheckRule()
        {
            if (this.CharterType == CharterType.Start)
            {
                //B19
                IsNotFinalApprove();
                //B21 
                HasCharterItem();
                //B26
                HasCharterEnd();
            }
            else
            {
                //B33
                IsNotFinalApprove();
                //B41
                IsLastCharterInEnd();
                //B39 
                HasCharterItem();

            }
        }


        public void AddItem(CharterItem charterItem)
        {
            #region bz

            if (this.CharterType == CharterType.Start)
            {
                //B5
                IsNotDuplicateItem(charterItem.TankId.Value, charterItem.GoodId);

                //B12
                HasTank(this.VesselInCompanyId.Value, charterItem.TankId.Value);

                //B60
                CompareOffhireValueItem(charterItem.GoodId, charterItem.OffhireFee);

                //B19
                IsNotFinalApprove();

                //B20
                HasCharterOutHeader(charterItem.CharterId);

                //B9 chrter out 
                IsGoodAndUnitValidInCompany(charterItem.GoodId, charterItem.GoodUnitId);

                //B74
                IsEqualValueOfFuelBetweenCharterInOut(charterItem.GoodId, charterItem.Rob)
                ;

            }
            else
            {
                //B5
                IsNotDuplicateItem(charterItem.TankId.Value, charterItem.GoodId);
                //B9 chrter out 
                IsGoodAndUnitValidInCompany(charterItem.GoodId, charterItem.GoodUnitId);
                //B12
                HasTank(this.VesselInCompanyId.Value, charterItem.TankId.Value);
                //B20
                HasCharterOutHeader(charterItem.CharterId);
                //B33
                //B92
                IsEqualValueOfFuelBetweenCharterInOutEnd(charterItem.GoodId, charterItem.Rob)
                       ;
                IsNotFinalApprove();
            }

            IsCharterItemValid isCharterItemValid = new IsCharterItemValid();
            //B6,7
            if (!(isCharterItemValid.IsSatisfiedBy(charterItem)))
                throw new BusinessRuleException("B6,7", "Invalid Charter Item ");
            #endregion

            this.CharterItems.Add(charterItem);
        }

        public void UpdateItem(long id, long charterId, decimal rob, decimal fee,
                     decimal feeOffhire, long goodId, long tankId, long unitId)
        {
            #region bz
            if (this.CharterType == CharterType.Start)
            {
                //B9 chrter out 
                IsGoodAndUnitValidInCompany(goodId, unitId);

                //B19
                IsNotFinalApprove();

                //B20
                HasCharterOutHeader(charterId);

                //B16

                //B5
                IsNotDuplicateItemUpdate(id, tankId, goodId);

                //B12
                HasTank(this.VesselInCompanyId.Value, tankId);

                //B60
                CompareOffhireValueItem(goodId, feeOffhire);
                //B74
                IsEqualValueOfFuelBetweenCharterInOut(goodId, rob)
                ;

            }
            else
            {
                //B9 chrter out 
                IsGoodAndUnitValidInCompany(goodId, unitId);

                //B16

                //B5
                IsNotDuplicateItemUpdate(id, tankId, goodId);

                //B33
                IsNotFinalApprove();

                //B20
                HasCharterOutHeader(charterId);

                //B92
                IsEqualValueOfFuelBetweenCharterInOutEnd(goodId, rob)
                       ;
            }

            #endregion

            //B6,7
            var item = this.CharterItems.SingleOrDefault(c => c.Id == id);
            if (item != null)
            {
                item.Update(id, charterId, rob, fee,
                   feeOffhire, goodId, tankId, unitId);
            }
            else
            {
                throw new ObjectNotFound("Charter Item ", id);
            }


        }

        public void DeleteItem(long id)
        {

            #region bz
            if (this.CharterType == CharterType.Start)
            {
                //B24
                IsNotRejectFinalApprove();
                //B25
                //UI

            }
            else
            {
                //B33
                IsNotFinalApprove();
                //B42
                IsNotRejectFinalApprove();
                //B40
            }
            #endregion
            var item = this.CharterItems.SingleOrDefault(c => c.Id == id);
            if (item != null)
            {
                this._charterOutDomainService.DeleteCharterItem(id);
            }
            else
            {
                throw new ObjectNotFound("Charter Item End", id);
            }
        }


        #endregion

        #region WorkFlow

        public override void Approve(long approverId)
        {
            if (this.CharterType == CharterType.Start)
            {
                //B70
                IsNotFinalApprove();
                //B42
                IsNotRejectFinalApprove();
                //B71
                HasAtleastCharterItem();
                //B69
                IsVesselCharterInOrOwned();
            }
            else
            {
                //B70
                IsNotFinalApprove();
                //B42
                IsNotRejectFinalApprove();
                //B71
                HasAtleastCharterItem();
                //B55
                IsVesselCharterOut();
            }
            _eventPublisher.Publish<CharterOutApproveArg>(new CharterOutApproveArg() { SendObject = this });

        }

        public override void Submited(long approverId)
        {
            if (this.CurrentState == States.Open)
            {
            if (this.CharterType == CharterType.Start)
            {
                //64
                CanIssuance();
                //65
                CanRecipt();
                //B69
                IsVesselCharterInOrOwned();
                //B70
                IsNotFinalApprove();
                ////72
                //this.IsSubmitedCharterOutStart();
            }
            else
            {
                //B70
                IsNotFinalApprove();
                //64
                CanIssuance();
                //65
                CanRecipt();
                //B55
                IsVesselCharterOut();
                //72
                this.IsSubmitedCharterOutStart(this.Id);


            }

            _eventPublisher.Publish<CharterOutFinalApproveArg>(new CharterOutFinalApproveArg() { SendObject = this });

            if (this.CharterType == CharterType.Start)
            {
                //<A.H>
                if (_charterOutDomainService.GetCharterState(this.Id) == States.Open)
                        VesselInCompanyDomainService.BeginCharteringOutVessel(this.VesselInCompanyId.Value, this.ActionDate, approverId);

                    //Change FR states moved to BeginCharterOut method.

                    var operationResult = InventoryOperationNotifier.NotifySubmittingCharterOutStart(
                    this._charterOutDomainService.GetVoyageCharterInStart(this.OwnerId.Value, this.VesselInCompanyId.Value, this.ActionDate),
                    this,
                    approverId);

                    this.InventoryOperationItems.MergeInventoryOperationResult(operationResult);
            }
            else
            {
                //<A.H>
                if (_charterOutDomainService.GetCharterState(this.Id) == States.Open)
                        VesselInCompanyDomainService.FinishCharteringOutVessel(this.VesselInCompanyId.Value, this.ActionDate, approverId);


                    var operationResult = InventoryOperationNotifier.NotifySubmittingCharterOutEnd(
                    _charterOutDomainService.GetVoyageCharterInEnd(this.OwnerId.Value, this.VesselInCompanyId.Value, this.ActionDate),
                    this,
                    approverId);

                    this.InventoryOperationItems.MergeInventoryOperationResult(operationResult);

                }
            }
            else if (this.CurrentState == States.SubmitRejected)
            {
                if (this.CharterType == CharterType.Start)
                {
                    Charter nextCharterContract = getNextCharterContract();

                    var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();
                    fuelReportDomainService.ChangeFuelReportsStateForCharteredOutVessel(this.VesselInCompany, this.ActionDate, nextCharterContract == null ? null : (DateTime?)nextCharterContract.ActionDate, approverId);

                    bool vesselShouldBeDeactivated = detectLastVesselActivationStatusForCharterOutStart();

                    var operationResult = InventoryOperationNotifier.NotifyCharterOutStartResubmit(
                            this._charterInDomainService.GetVoyageCharterInEnd(this.OwnerId.Value, this.VesselInCompanyId.Value, this.ActionDate),
                            this,
                            approverId,
                            vesselShouldBeDeactivated);

                    this.InventoryOperationItems.MergeInventoryOperationResult(operationResult);
                }
                else
                {
                    bool vesselShouldBeDeactivated = detectLastVesselActivationStatusForCharterOutEnd();

                    Charter nextCharterContract = getNextCharterContract();

                    var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();
                    fuelReportDomainService.MoveFuelReportsToCompany(this.VesselInCompany.Code, this.VesselInCompany.CompanyId, this.ActionDate.AddSeconds(1), nextCharterContract == null ? null : (DateTime?)nextCharterContract.ActionDate, approverId);

                    var operationResult = InventoryOperationNotifier.NotifyCharterOutEndResubmit(
                            this._charterInDomainService.GetVoyageCharterInEnd(this.OwnerId.Value, this.VesselInCompanyId.Value, this.ActionDate),
                            this,
                            approverId,
                            vesselShouldBeDeactivated);

                    this.InventoryOperationItems.MergeInventoryOperationResult(operationResult);
                }
            }

            //this.InventoryOperationItems.MergeInventoryOperationResult(); AddRange(inventoryOperationResult);
        }

        private Charter getNextCharterContract()
        {
            var charteringDomainService = ServiceLocator.Current.GetInstance<ICharteringDomainService>();

            try
            {
                return charteringDomainService.GetNextCharterContractForCompany(this.VesselInCompany.Code, this.OwnerId.Value, this.ActionDate, this.Id, true);
            }
            finally
            {
                ServiceLocator.Current.Release(charteringDomainService);
            }
        }

        private bool detectLastVesselActivationStatusForCharterOutEnd()
        {
            var charteringDomainService = ServiceLocator.Current.GetInstance<ICharteringDomainService>();

            try
            {
                var lastCharterContract = charteringDomainService.GetLastCharterContractForCompany(this.VesselInCompany.Code, this.OwnerId.Value, DateTime.Now);

                bool vesselShouldBeDeactivated = false;

                if (lastCharterContract != null)
                {
                    if (lastCharterContract.ActionDate < this.ActionDate)
                    {
                        if (!(lastCharterContract is CharterOut && lastCharterContract.CharterType == CharterType.Start))
                            throw new BusinessRuleException("", "Last active Charter contract is invalid.");

                        vesselShouldBeDeactivated = false;
                    }
                    else
                    {
                        if (
                            (lastCharterContract is CharterIn && lastCharterContract.CharterType == CharterType.End) ||
                            (lastCharterContract is CharterOut && lastCharterContract.CharterType == CharterType.Start)
                            )
                            vesselShouldBeDeactivated = true;
                        else
                            vesselShouldBeDeactivated = false;
                    }
                }

                return vesselShouldBeDeactivated;
            }
            finally
            {
                ServiceLocator.Current.Release(charteringDomainService);
            }
        }

        private bool detectLastVesselActivationStatusForCharterOutStart()
        {
            var charteringDomainService = ServiceLocator.Current.GetInstance<ICharteringDomainService>();

            try
            {
                var lastCharterContract = charteringDomainService.GetLastCharterContractForCompany(this.VesselInCompany.Code, this.OwnerId.Value, DateTime.Now);

                bool vesselShouldBeDeactivated = false;

                if (lastCharterContract != null)
                {
                    if (lastCharterContract.ActionDate < this.ActionDate)
                    {
                        if (!((lastCharterContract is CharterIn && lastCharterContract.CharterType == CharterType.Start) ||
                            (lastCharterContract is CharterOut && lastCharterContract.CharterType == CharterType.End))
                            )
                            throw new BusinessRuleException("", "Last active Charter contract is invalid.");

                        vesselShouldBeDeactivated = true;
                    }
                    else
                    {
                        if (
                            (lastCharterContract is CharterIn && lastCharterContract.CharterType == CharterType.Start) ||
                            (lastCharterContract is CharterOut && lastCharterContract.CharterType == CharterType.End)
                            )
                            vesselShouldBeDeactivated = false;
                        else
                            vesselShouldBeDeactivated = true;
                    }
                }

                return vesselShouldBeDeactivated;
            }
            finally
            {
                ServiceLocator.Current.Release(charteringDomainService);
            }
        }

        public override void RejectSubmited(long approverId)
        {
            if (this.CharterType == CharterType.Start)
            {
                //B63
                IsFinalApprove();

                var operationResult = InventoryOperationNotifier.RevertCharterOutStartInventoryOperations(this, (int)approverId);

                this.InventoryOperationItems.MergeInventoryOperationResult(operationResult);

                Charter nextCharterContract = getNextCharterContract();

                var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();
                fuelReportDomainService.MoveFuelReportsToCompany(this.VesselInCompany.Code, this.VesselInCompany.CompanyId, this.ActionDate.AddSeconds(1), nextCharterContract == null ? null : (DateTime?)nextCharterContract.ActionDate, approverId);
            }
            else
            {
                //B63
                IsFinalApprove();

                var operationResult = InventoryOperationNotifier.RevertCharterOutEndInventoryOperations(this, (int)approverId);

                this.InventoryOperationItems.MergeInventoryOperationResult(operationResult);
            }
            _eventPublisher.Publish<CharterOutDisApproveArg>(new CharterOutDisApproveArg() { SendObject = this });
        }

        #endregion

        #region Business Rule


        //B2 
        void IsValidCharterInStartDate()
        {
            if (!(_charterOutDomainService.IsCharterStartDateGreaterThanPrevCharterEndDate(this.VesselInCompanyId.Value, this.ActionDate)))
                throw new BusinessRuleException("B2", "Charter In Start date must  Greater than Previous Charter In End");
        }

        //B9 chrter out 
        public override void IsGoodAndUnitValidInCompany(long goodId, long goodUnitId)
        {
            IGoodDomainService goodDomainService = ServiceLocator.Current.GetInstance<IGoodDomainService>();
            var good = goodDomainService.GetGoodWithUnit(this.OwnerId.Value, goodId);
            if (!(good != null && good.GoodUnits.Count(gu => gu.Id == goodUnitId) == 1))
                throw new BusinessRuleException("B9", "Good Or Unit Not Define For Company");

        }

        //B30
        void IsChangePropRejectSubmitted(long chartererId, long vesselInCompanyId, long currencyId,
                       DateTime actionDate)
        {
            if (this.CurrentState == States.SubmitRejected)
                if (!(this.ChartererId.Value == chartererId
                    && this.VesselInCompanyId.Value == vesselInCompanyId
                    && this.CurrencyId == currencyId
                    && this.ActionDate == actionDate))
                    throw new BusinessRuleException("B30", "Charterer & Vessel & Currency & StartDate Shoulde'nt change");
        }





        //B16
        void IsChangeValidRejectSubmited(long id, decimal feeOffhire, long goodId, long tankId)
        {
            if (_charterOutDomainService.GetCharterState(this.Id) == States.SubmitRejected)
            {
                var item = this.CharterItems.SingleOrDefault(c => c.Id == id);
                if (item != null)
                    if (!(item.OffhireFee == feeOffhire
                        && item.GoodId == goodId
                        && item.TankId == tankId))
                        throw new BusinessRuleException("B16", "Charter Is RejectedSubmited");

            }
        }


        //B18
        void IsValidChangeOffhirePricing(OffHirePricingType offHirePricingType)
        {

            if (_charterOutDomainService.GetCharterState(this.Id) == States.SubmitRejected)
            {
                IOffhireDomainService offhireDomainService = ServiceLocator.Current.GetInstance<IOffhireDomainService>();
                var res = _charterOutDomainService.GetCharterEnd(this.Id);
                if (res != null)
                    if (offhireDomainService.
                             IsOffhireRegisteredForVessel(this.VesselInCompanyId.Value, this.ActionDate, res.ActionDate))
                        if (this.OffHirePricingType != offHirePricingType)
                            throw new BusinessRuleException("B18", "Offhire Registerd for this charter in");
            }
        }



        //B33,B19
        void IsNotFinalApprove()
        {
            if (_charterOutDomainService.GetCharterState(this.Id) == States.Submitted)
                throw new BusinessRuleException("B1,B46", "Charter is final Approve");
        }

        //B20
        void HasCharterOutHeader(long charterId)
        {

            if (!(_charterOutDomainService.ExistCharterOutHeader(charterId)))
                throw new BusinessRuleException("B8", "CharterItem Has Not Header");
        }

        //B21,B39 
        void HasCharterItem()
        {
            var res = _charterOutDomainService.HasCharterItem(this.Id);

            if (res)
                throw new BusinessRuleException("B21", "CharterIn Has Items");
        }

        //B22
        void IsVesselOwnedOrCharterIn()
        {
            IVesselInCompanyDomainService vesselDomainService = ServiceLocator.Current.GetInstance<IVesselInCompanyDomainService>();
            var res = vesselDomainService.Get(this.VesselInCompanyId.Value);
            if (!(res.VesselStateCode == VesselStates.CharterIn || res.VesselStateCode == VesselStates.Owned))
                throw new BusinessRuleException("B22", "Vessel is not charterin or owned");

        }

        //B23
        void IsValidStartDate()
        {
            IVesselInCompanyDomainService vesselDomainService = ServiceLocator.Current.GetInstance<IVesselInCompanyDomainService>();

            var res = vesselDomainService.Get(this.VesselInCompanyId.Value);
            if (res.VesselStateCode == VesselStates.CharterIn)
            {
                var charterInStart = _charterInDomainService.GetCharterStartDate(this.VesselInCompanyId.Value,
                                                                                this.OwnerId.Value);
                if (this.ActionDate < charterInStart)
                    throw new BusinessRuleException("B23", "Vessel is charter in ,invalid start date ");
            }
        }

        //B42,B24
        void IsNotRejectFinalApprove()
        {
            if (_charterOutDomainService.GetCharterState(this.Id) == States.SubmitRejected)
                throw new BusinessRuleException("B42,B24", "Charter is reject final Approve");
        }

        //B26
        void HasCharterEnd()
        {
            if (_charterOutDomainService.HasCharterEnd(this.Id))
                throw new BusinessRuleException("B11", "Charter Is End");
        }

        //B28
        void HasVesselCharterOutStart(long vesselInCompanyId)
        {
            if (_charterOutDomainService.HasVesselCharterOutStart(vesselInCompanyId))
                throw new BusinessRuleException("B54,B14", "Vessel Has  Charter Out start ");
        }

        //B29
        void IsValidateCharterOutStartProp()
        {
            if (!(this.VesselInCompanyId > 0
                && this.ActionDate != null
                && this.ChartererId > 0)
                && this.CurrencyId > 0)
                throw new BusinessRuleException("B29", "Charter Out Start is Not Valid");
        }

        //B31
        void ExistFinalApprove(long id)
        {
            if (!(_charterOutDomainService.HasVesselCharterOutStart(id, this.VesselInCompanyId.Value, this.OwnerId.Value)))
                throw new BusinessRuleException("B31", "Charter In Start Submited Not Exist");

        }



        //B35
        void IsNotVesselCharterIn(long vesselInCompanyId)
        {
            IVesselInCompanyDomainService vesselDomainService = ServiceLocator.Current.GetInstance<IVesselInCompanyDomainService>();
            var res = vesselDomainService.Get(vesselInCompanyId);
            if (res.VesselStateCode == VesselStates.CharterIn)
                throw new BusinessRuleException("B22", "Vessel is  charterin ");
        }

        //B36
        void CheckPeriodCharterInValid(long id)
        {
            //var charteroutstartdate = _charterOutDomainService.GetCharterStartDateInternal(id, this.VesselInCompanyId.Value, this.OwnerId.Value);
            //if (charteroutstartdate != null)
            //{
            //    IVesselInCompanyDomainService vesselInCompanyDomainService = ServiceLocator.Current.GetInstance<IVesselInCompanyDomainService>();

            //    ICompanyDomainService companyDomainService = ServiceLocator.Current.GetInstance<ICompanyDomainService>();

            //    var charterer = companyDomainService.Get(this.ChartererId.Value);

            //    if (!charterer.IsMemberOfHolding)
            //    {
            //        var code =
            //     vesselInCompanyDomainService.GetAll().Single(c => c.Id == this.VesselInCompanyId.Value).Code;
                    
            //        var vessel = vesselInCompanyDomainService.GetVesselInCompany(this.ChartererId.Value, code);
            //        var res = _charterInDomainService.GetCharterInPrevCharterOut(vessel.Id,
            //                                                                charteroutstartdate.ActionDate);
            //        if (res != null)
            //            if ((this.ActionDate < res.ActionDate))
            //                throw new BusinessRuleException("B36", "Charter Out end date must Previous Charter In end date");
            //    }
               
            //}
        }

        //B37
        void IsNextCharterStartDateGreaterThanActionDate(long id)
        {
            if (!_charterOutDomainService.CheckNextCharterStartDate(id, this.VesselInCompanyId.Value, this.OwnerId.Value, this.ActionDate))
                throw new BusinessRuleException("B37", "Next Start Date Must Greater than End Date");


        }

        //B41
        void IsLastCharterInEnd()
        {
            if (!(_charterOutDomainService.IsLastCharter(this.VesselInCompanyId.Value, this.Id)))
                throw new BusinessRuleException("B41", "Charter out has another last charter end ");

        }

        //B43
        void HasVesselCharterOutEnd(long vesselInCompanyId)
        {
            if (_charterOutDomainService.HasVesselCharterOutEnd(vesselInCompanyId))
                throw new BusinessRuleException("B54,B14", "Vessel Has  Charter Out end ");
        }

        //B44
        void IsChangeValidForSubmitRejectedCharter(DateTime actionDate, long currencyId, CharterEndType charterEndType)
        {
            if (_charterOutDomainService.GetCharterState(this.Id) == States.SubmitRejected)
                if (!(actionDate == this.ActionDate
                    && currencyId == this.CurrencyId
                    && charterEndType == this.CharterEndType))
                    throw new BusinessRuleException("B53", "Charter End Invalid");

        }


        //B45
        void IsValidEndDate(long id)
        {
            var startDate = _charterOutDomainService.GetCharterStartDate(id, this.VesselInCompanyId.Value, this.OwnerId.Value);
            if (startDate != null)
                if (!(this.ActionDate > startDate.ActionDate))
                    throw new BusinessRuleException("B45", "End Date Must Greater than Start Date");
        }
        //B55
        void IsVesselCharterOut()
        {
            IVesselInCompanyDomainService vesselDomainService = ServiceLocator.Current.GetInstance<IVesselInCompanyDomainService>();
            var vessel = vesselDomainService.Get(this.VesselInCompanyId.Value);
            if (vessel.VesselStateCode != VesselStates.CharterOut)
                throw new BusinessRuleException("B55", "Vessel is not chartered Out");

        }

        //B59
        void CompareOffhirePricing(OffHirePricingType offHirePricingType)
        {
            var res = _charterInDomainService.GetCharterInStart(this.VesselInCompanyId.Value, this.OwnerId.Value,
                                                                 this.ActionDate);
            if (res != null)
                if (!(res.OffHirePricingType == offHirePricingType))
                    throw new BusinessRuleException("B59", "Offhire Registerd Has not same type");


        }

        //B60
        void CompareOffhireValueItem(long goodID, decimal offhireFee)
        {
            var res = _charterInDomainService.GetCharterInStart(this.VesselInCompanyId.Value, this.OwnerId.Value,
                                                                this.ActionDate);
            var newGoodId = GetGoodIdOtherCompany(goodID);
            if (res != null)
                foreach (CharterItem item in res.CharterItems)
                {
                    if (item.GoodId == newGoodId)
                        if (item.OffhireFee != offhireFee)
                            throw new BusinessRuleException("B60", "Offhire Registerd Has not same value");

                }
        }



        //B69
        void IsVesselCharterInOrOwned()
        {
            IVesselInCompanyDomainService vesselDomainService = ServiceLocator.Current.GetInstance<IVesselInCompanyDomainService>();
            var vessel = vesselDomainService.Get(this.VesselInCompanyId.Value);
            if (!(vessel.VesselStateCode == VesselStates.CharterIn || vessel.VesselStateCode == VesselStates.Owned))
                throw new BusinessRuleException("B55", "Vessel is not charter In");

        }

        //B70
        void IsFinalApprove()
        {
            if (_charterOutDomainService.GetCharterState(this.Id) != States.Submitted)
                throw new BusinessRuleException("B1,B46", "Charter is not final Approve");
        }

        //B71
        void HasAtleastCharterItem()
        {
            var res = _charterOutDomainService.GetCharterInStart(this.Id);
            if (!(res.CharterItems.Count > 0))
                throw new BusinessRuleException("B71", "Charter has not Charter Item");

        }
        //B72
        void IsSubmitedCharterOutStart(long id)
        {
            var res = _charterOutDomainService.GetCharterStartState(id, this.VesselInCompanyId.Value, this.OwnerId.Value);
            if (!(res == States.Submitted))
                throw new BusinessRuleException("B67", "Charter Out start is submited");
        }

        //B74
        void IsEqualValueOfFuelBetweenCharterInOut(long goodId, decimal rob)
        {

            var newGoodId = GetGoodIdOtherCompany(goodId);
            var res = _charterInDomainService.GetCharterInStart(this.VesselInCompanyId.Value, this.OwnerId.Value, this.ActionDate);
            if (res != null)
            {
                var itm = res.CharterItems.SingleOrDefault(c => c.GoodId == newGoodId);

                if (itm.Rob != rob)
                    throw new BusinessRuleException("B74", "Value of Rob diffrent with CharterOut");
            }


        }

        //B90
        void IsEqualDateCharterInStart(DateTime dateTime)
        {

            var res = _charterInDomainService.GetCharterInStart(this.VesselInCompanyId.Value, this.OwnerId.Value, this.ActionDate);
            if (res != null)
            {
                if (res.ActionDate != dateTime)
                    throw new BusinessRuleException("B90", "Value of Date diffrent with CharterIn");
            }
        }


        //B91
        void IsEqualDateCharterInEnd(DateTime dateTime)
        {

            var res = _charterInDomainService.GetCharterEnd(this.Id);
            if (res != null)
            {
                if (res.ActionDate != dateTime)
                    throw new BusinessRuleException("B91", "Value of Date diffrent with CharterOut");
            }
        }

        //B92
        void IsEqualValueOfFuelBetweenCharterInOutEnd(long goodId, decimal rob)
        {

            var newGoodId = GetGoodIdOtherCompany(goodId);
            var res = _charterInDomainService.GetCharterEnd(this.Id);
            if (res != null)
            {
                var itm = res.CharterItems.SingleOrDefault(c => c.GoodId == newGoodId);

                if (itm.Rob != rob)
                    throw new BusinessRuleException("B92", "Value of Rob diffrent with CharterOut");
            }


        }

        #endregion
        long GetGoodIdOtherCompany(long goodID)
        {
            this._goodRepository = ServiceLocator.Current.GetInstance<IGoodRepository>();
            var shardGoodId = _goodRepository.FindByKey(goodID).SharedGood.Id;
            var newGoodId =
                _goodRepository.Single(c => c.SharedGood.Id == shardGoodId && c.Company.Id == this.OwnerId.Value).Id;
            return newGoodId;
        }
    }
}
