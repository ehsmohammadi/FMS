using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.CharterTypes;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.DomainServices;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Events;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Specifications;
using MITD.Fuel.Domain.Model.DomainServices;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Extensions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.FuelSecurity.Domain.Model;


namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate
{

    public class CharterIn : Charter
    {

        #region Prop


        private CharterTypeBase<CharterIn> charterTypeBase;
        public OffHirePricingType OffHirePricingType { get; set; }

        private IGoodRepository _goodRepository;

        #endregion

        #region Ctor

        public CharterIn()
        {

        }

        internal CharterIn(long id, long chartererId, long ownerId, long vesselInCompanyId, long CurrencyId,
                       DateTime actionDate, List<CharterItem> charterItems,
                       List<InventoryOperation> inventoryOperationItems,
                       CharterType charterType, CharterEndType charterEndType,
                       OffHirePricingType offHirePricingType,
            ICharterInDomainService charterInDomainService,
            ICharterOutDomainService charterOutDomainService,
            IEventPublisher eventPublisher
            )
            : base(id, chartererId, ownerId, vesselInCompanyId, CurrencyId,
                  actionDate, charterItems, inventoryOperationItems,
                  charterType, charterEndType, charterInDomainService, charterOutDomainService, eventPublisher)
        {

            this.OffHirePricingType = offHirePricingType;
            this._eventPublisher.RegisterHandler<CharterInApproveArg>(new InventorySubscriber());
            this._eventPublisher.RegisterHandler<CharterInFinalApproveArg>(new InventorySubscriber());
            this._eventPublisher.RegisterHandler<CharterInDisApproveArg>(new InventorySubscriber());



            #region BZ

            if (CharterType == CharterType.Start)
            {

                //B74
                IsNotSameChatererOwner(ownerId, chartererId);
                //B4
                IsValidateCharterInStartProp();
                //B17
                HasVesselCharterInStart();
                //B2
                IsValidCharterInStartDate();
                //B59
                CompareOffhirePricing(offHirePricingType);
                //90
                IsEqualDateCharterOutStart(0, actionDate);

            }
            else
            {
                //B45
                IsValidEndDate();

                //B48
                ExistFinalApprove();

                //B58
                HasVesselCharterInEnd();
                //B55
                IsVesselCharterOut();

                //B49

                //B56
                IsEndDateGreaterThanLastCharterOutEndDate();

                //91
                IsEqualDateCharterOutEnd(actionDate);

            }
            #endregion




        }

        #endregion

        #region Opert

        internal void SetCharterType(CharterTypeBase<CharterIn> charterTypeBase)
        {
            this.charterTypeBase = charterTypeBase;
        }


        public void Update(long id, long chartererId, long ownerId, long vesselInCompanyId, long currencyId,
                       DateTime actionDate, List<CharterItem> charterItems,
                       List<InventoryOperation> inventoryOperationItems,
                       CharterType charterType, CharterEndType charterEndType, OffHirePricingType offHirePricingType)
        {
            #region bz

            if (CharterType == CharterType.Start)
            {

                //B74
                IsNotSameChatererOwner(ownerId, chartererId);
                //B4
                IsValidateCharterInStartProp();
                //B13
                IsChangePropRejectSubmitted(ownerId, vesselInCompanyId, currencyId, actionDate);
                //B1
                IsNotFinalApprove();


                //B2
                IsValidCharterInStartDate();
                //B18
                IsValidChangeOffhirePricing(offHirePricingType);
                //B59
                CompareOffhirePricing(offHirePricingType);
                //90
                IsEqualDateCharterOutStart(0, actionDate);
            }
            else
            {
                //B32
                IsValidateCharterEnd(actionDate, currencyId, charterEndType);

                //B45
                IsValidEndDate();

                //B46
                IsNotFinalApprove();

                //B48
                ExistFinalApprove();

                //B53
                IsChangeValidForSubmitRejectedCharter(actionDate, currencyId, charterEndType);
                //B55
                IsVesselCharterOut();

                //B56
                IsEndDateGreaterThanLastCharterOutEndDate();
                //91
                IsEqualDateCharterOutEnd(actionDate);


            }

            #endregion


            this.OffHirePricingType = offHirePricingType;
            this.UpdateprivateProp(id, chartererId, ownerId, vesselInCompanyId, currencyId,
                        actionDate, charterItems,
                       inventoryOperationItems,
                        charterType, charterEndType);

        }

        public void DeleteCheckRule()
        {

            #region bz

            //B1
            IsNotFinalApprove();
            //B10
            HasCharterItem();
            if (this.CharterType == CharterType.Start)
            {

                //B11
                HasCharterEnd();
            }
            else
            {
                //B57 
                IsLastCharterInEnd();
            }

            #endregion
        }


        public void AddItem(CharterItem charterItem)
        {

            #region bz

            if (this.CharterType == CharterType.Start)
            {
                //B1
                IsNotFinalApprove();

                //B5
                IsNotDuplicateItem(charterItem.TankId.Value, charterItem.GoodId);

                //12
                HasTank(this.VesselInCompanyId.Value, charterItem.TankId.Value);

                //B8,B47
                HasCharterInHeader(this.Id);

                //B9
                IsGoodAndUnitValidInCompany(charterItem.GoodId, charterItem.GoodUnitId);


                //B60
                CompareOffhireValueItem(charterItem.GoodId, charterItem.OffhireFee);

                //B74
                IsEqualValueOfFuelBetweenCharterInOut(0, charterItem.GoodId, charterItem.Rob);
            }
            else
            {
                //B5
                IsNotDuplicateItem(charterItem.TankId.Value, charterItem.GoodId);

                //12
                HasTank(this.VesselInCompanyId.Value, charterItem.TankId.Value);

                //B47
                HasCharterInHeader(this.Id);

                //B92
                IsEqualValueOfFuelBetweenCharterInOutEnd(charterItem.GoodId, charterItem.Rob)
                ;

                //B1
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
                //B8
                HasCharterInHeader(this.Id);

                //B9
                IsGoodAndUnitValidInCompany(goodId, unitId);

                //B5
                IsNotDuplicateItemUpdate(id, tankId, goodId);

                //B1
                IsNotFinalApprove();

                //B12
                HasTank(this.VesselInCompanyId.Value, tankId);

                //B16
                IsChangeValidRejectSubmited(id, feeOffhire, goodId, tankId)
                ;

                //B60
                CompareOffhireValueItem(goodId, feeOffhire);

                //B74
                IsEqualValueOfFuelBetweenCharterInOut(0, goodId, rob);

            }
            else
            {

                //B5
                IsNotDuplicateItemUpdate(id, tankId, goodId);

                //B9
                IsGoodAndUnitValidInCompany(goodId, unitId);


                //B12
                HasTank(this.VesselInCompanyId.Value, tankId);

                //B16
                IsChangeValidRejectSubmited(id, feeOffhire, goodId, tankId);
                //B46
                IsNotFinalApprove();
                //B92
                IsEqualValueOfFuelBetweenCharterInOutEnd(goodId, rob)
                ;
                //B47
                HasCharterInHeader(this.Id);

            }
            #endregion

            //B6,7 in charteritem
            var item = this.CharterItems.SingleOrDefault(c => c.Id == id);

            if (item != null)
            {

                _charterInDomainService.AddCharterItemHistory(new CharterItemHistory(0, item.CharterId, item.Rob, item.Fee, item.OffhireFee, item.GoodId, item.TankId.Value, item.GoodUnitId, DateTime.Now, item.Id, (int)this.CurrentState));

                item.Update(id, charterId, rob, fee,
                   feeOffhire, goodId, tankId, unitId);
            }
            else
            {
                throw new ObjectNotFound("Charter Item Start", id);
            }

        }

        public void DeleteItem(long id)
        {
            #region bz
            if (this.CharterType == CharterType.Start)
            {
                //B1
                IsNotFinalApprove();

                //B14
                IsNotRejectFinalApprove();

                //B15
                //UI
            }
            else
            {
                //B1
                IsNotFinalApprove();

                //B54
                IsNotRejectFinalApprove();
            }
            #endregion


            var item = this.CharterItems.SingleOrDefault(c => c.Id == id);
            if (item != null)
            {
                _charterInDomainService.DeleteCharterItem(id);
            }
            else
            {
                throw new ObjectNotFound("Charter Item Start", id);
            }


        }


        #endregion

        #region WorkFlow

        public override void Approve(long approverId)
        {
            if (this.CharterType == CharterType.Start)
            {
                //B1
                IsNotFinalApprove();
                //B14
                IsNotRejectFinalApprove();
                //B61
                HasAtleastCharterItem();
                //B62
                IsInactiveVessel();

            }
            else
            {
                //B46
                IsNotFinalApprove();
                //B61
                HasAtleastCharterItem();
                //B55
                IsVesselCharterOut();
                //B56 
                IsEndDateGreaterThanLastCharterOutEndDate();
                //B54
                IsNotRejectFinalApprove();

                //B66
                IsVesselCharterIn();
            }
            _eventPublisher.Publish<CharterInApproveArg>(new CharterInApproveArg() { SendObject = this });

        }

        public override void Submited(long approverId)
        {
            if (this.CurrentState == States.Open)
            {
                if (this.CharterType == CharterType.Start)
                {
                    //B1
                    IsNotFinalApprove();
                    //B62
                    IsInactiveVessel();
                    //64
                    CanIssuance();
                    //65
                    CanRecipt();

                }
                else
                {
                    //B46
                    IsNotFinalApprove();
                    //B55
                    IsVesselCharterOut();
                    //B56 
                    IsEndDateGreaterThanLastCharterOutEndDate();
                    //64
                    CanIssuance();
                    //65
                    CanRecipt();
                    //B66
                    IsVesselCharterIn();
                    //B67
                    IsSubmitedCharterInStart();

                    //B68
                    IssuanceEndOfVoyageIsSubmitedForVessel();
                }

                _eventPublisher.Publish<CharterInFinalApproveArg>(new CharterInFinalApproveArg() { SendObject = this });

                var inventoryOperationResult = new List<InventoryOperation>();

                if (this.CharterType == CharterType.Start)
                {
                    if (_charterInDomainService.GetCharterState(this.Id) == States.Open)  //<A.H>
                        VesselInCompanyDomainService.BeginCharteringInVessel(this.VesselInCompanyId.Value, this.ActionDate, approverId);

                    var operationResult = InventoryOperationNotifier.NotifySubmittingCharterInStart(
                            _charterInDomainService.GetVoyageCharterInStart(this.ChartererId.Value, this.VesselInCompanyId.Value, this.ActionDate),
                            this,
                            approverId);

                    this.InventoryOperationItems.MergeInventoryOperationResult(operationResult);






                    //foreach (var item in this.CharterItems)
                    //{
                    //    _charterInDomainService.AddCharterItemHistory(new CharterItemHistory(0, item.CharterId, item.Rob, item.Fee, item.OffhireFee, item.GoodId, item.TankId.Value, item.GoodUnitId, DateTime.Now, item.Id, 1));
                    //}
                }
                else
                {
                    if (_charterInDomainService.GetCharterState(this.Id) == States.Open)  //<A.H>
                        VesselInCompanyDomainService.FinishCharteringInVessel(this.VesselInCompanyId.Value);

                    var operationResult = InventoryOperationNotifier.NotifySubmittingCharterInEnd(
                            this._charterInDomainService.GetVoyageCharterInEnd(this.ChartererId.Value, this.VesselInCompanyId.Value, this.ActionDate),
                            this,
                            approverId);

                    this.InventoryOperationItems.MergeInventoryOperationResult(operationResult);
                }
            }
            else if (this.CurrentState == States.SubmitRejected)
            {
                #region Resubmit Scenario By A.Hatefi
                if (this.CharterType == CharterType.Start)
                {
                    bool vesselShouldBeDeactivated = this.detectLastVesselActivationStatusForCharterInStart();

                    Charter nextCharterContract = getNextCharterContract();

                    var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();
                    fuelReportDomainService.MoveFuelReportsToCompany(this.VesselInCompany.Code, this.VesselInCompany.CompanyId, this.ActionDate.AddSeconds(1), nextCharterContract == null ? null : (DateTime?)nextCharterContract.ActionDate, approverId);

                    var operationResult = InventoryOperationNotifier.NotifyCharterInStartResubmit(
                            this._charterInDomainService.GetVoyageCharterInEnd(this.ChartererId.Value, this.VesselInCompanyId.Value, this.ActionDate),
                            this,
                            approverId,
                            vesselShouldBeDeactivated);

                    this.InventoryOperationItems.MergeInventoryOperationResult(operationResult);
                }
                else
                {
                    //B56 
                    IsEndDateGreaterThanLastCharterOutEndDate();
                    //B67  TODO : Must be improved to check with relevant Chart Start
                    //IsSubmitedCharterInStart();

                    bool vesselShouldBeDeactivated = this.detectLastVesselActivationStatusForCharterInEnd();

                    var operationResult = InventoryOperationNotifier.NotifyCharterInEndResubmit(
                            this._charterInDomainService.GetVoyageCharterInEnd(this.ChartererId.Value, this.VesselInCompanyId.Value, this.ActionDate),
                            this,
                            approverId,
                            vesselShouldBeDeactivated);

                    this.InventoryOperationItems.MergeInventoryOperationResult(operationResult);
                }

                #endregion
                //    //todo comment for server
                //    var historylist = _charterInDomainService.GetCharterItemHistories(this.Id, this.CurrentState);
                //    var charterInEnd = _charterInDomainService.GetCharterEnd(this.Id);
                //    var listChangeQuantity = new List<Tuple<CharterItem, decimal,decimal?>>();
                //    var listChangePrice = new List<Tuple<CharterItem, decimal>>();
                //    foreach (var charterItem in this.CharterItems)
                //    {
                //        var res =
                //            historylist.Where(c => c.CharterItemId == charterItem.Id).OrderBy(c => c.Id).LastOrDefault();

                //        if ((res != null && (res.Rob != charterItem.Rob)) && (res.Fee != charterItem.Fee))
                //        {
                //            //3 parameter old fee
                //            listChangeQuantity.Add(new Tuple<CharterItem, decimal, decimal?>(charterItem, Math.Abs(charterItem.Rob - res.Rob), res.Fee));
                //            //3 parameter new Quantity
                //            listChangePrice.Add(new Tuple<CharterItem, decimal>(charterItem, Math.Abs(charterItem.Fee - res.Fee)));//, charterItem.Rob));
                //        }
                //        else if (res != null && (res.Rob != charterItem.Rob))
                //        {
                //            listChangeQuantity.Add(new Tuple<CharterItem, decimal, decimal?>(charterItem, Math.Abs(charterItem.Rob - res.Rob),null));
                //        }
                //        else if (res != null && (res.Fee != charterItem.Fee))
                //        {
                //            listChangePrice.Add(new Tuple<CharterItem, decimal>(charterItem, Math.Abs(charterItem.Fee - res.Fee)));//, null)); 
                //        }
                //    }

                //    foreach (var l in listChangeQuantity)
                //    {

                //        InventoryOperationNotifier.NotifySubmittingRevertQuantityCharterInStart(
                //        this,
                //       _charterInDomainService.GetVoyageCharterInStart(this.ChartererId.Value, this.VesselInCompanyId.Value, this.ActionDate),
                //         (charterInEnd != null) ? charterInEnd.ActionDate : (DateTime?)null,
                //         (DateTime?)null,
                //         approverId,
                //        _goodRepository,
                //          l.Item2,
                //          l.Item1,
                //          l.Item3
                //         );

                //        _charterInDomainService.AddCharterItemHistory(new CharterItemHistory(0, l.Item1.CharterId, l.Item1.Rob, l.Item1.Fee, l.Item1.OffhireFee, l.Item1.GoodId, l.Item1.TankId.Value, l.Item1.GoodUnitId, DateTime.Now, l.Item1.Id, 1));

                //    }


                //    foreach (var l in listChangePrice)
                //    {

                //        InventoryOperationNotifier.NotifySubmittingRevertPriceCharterInStart(
                //        this,
                //       _charterInDomainService.GetVoyageCharterInStart(this.ChartererId.Value, this.VesselInCompanyId.Value, this.ActionDate),
                //         (charterInEnd != null) ? charterInEnd.ActionDate : (DateTime?)null,
                //         (DateTime?)null,
                //         approverId,
                //        _goodRepository,
                //          l.Item2,
                //          l.Item1
                //         );

                //        _charterInDomainService.AddCharterItemHistory(new CharterItemHistory(0, l.Item1.CharterId, l.Item1.Rob, l.Item1.Fee, l.Item1.OffhireFee, l.Item1.GoodId, l.Item1.TankId.Value, l.Item1.GoodUnitId, DateTime.Now, l.Item1.Id, 1));

                //    }



                //    //    _charterInDomainService.AddCharterItemHistory(new CharterItemHistory(0, l.Item1.CharterId, l.Item1.Rob, l.Item1.Fee, l.Item1.OffhireFee, l.Item1.GoodId, l.Item1.TankId.Value, l.Item1.GoodUnitId, DateTime.Now, l.Item1.Id, 1));

                //    //}

            }




        }

        private Charter getNextCharterContract()
        {
            var charteringDomainService = ServiceLocator.Current.GetInstance<ICharteringDomainService>();

            try
            {
                return charteringDomainService.GetNextCharterContractForCompany(this.VesselInCompany.Code, this.ChartererId.Value, this.ActionDate, this.Id, true);
            }
            finally
            {
                ServiceLocator.Current.Release(charteringDomainService);
            }
        }

        private bool detectLastVesselActivationStatusForCharterInStart()
        {
            var charteringDomainService = ServiceLocator.Current.GetInstance<ICharteringDomainService>();
            try
            {
                var lastCharterContract = charteringDomainService.GetLastCharterContractForCompany(this.VesselInCompany.Code, this.ChartererId.Value, DateTime.Now);

                var vesselShouldBeDeactivated = false;

                if (lastCharterContract != null)
                {
                    if (lastCharterContract.ActionDate < this.ActionDate)
                    {
                        if (!(lastCharterContract is CharterIn && lastCharterContract.CharterType == CharterType.End))
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

        private bool detectLastVesselActivationStatusForCharterInEnd()
        {
            var charteringDomainService = ServiceLocator.Current.GetInstance<ICharteringDomainService>();

            try
            {
                var lastCharterContract = charteringDomainService.GetLastCharterContractForCompany(this.VesselInCompany.Code, this.ChartererId.Value, DateTime.Now);

                var vesselShouldBeDeactivated = false;

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

                var operationResult = InventoryOperationNotifier.RevertCharterInStartInventoryOperations(this, (int)approverId);

                this.InventoryOperationItems.MergeInventoryOperationResult(operationResult);
            }
            else
            {
                //B63
                IsFinalApprove();

                var operationResult = InventoryOperationNotifier.RevertCharterInEndInventoryOperations(this, (int)approverId);

                this.InventoryOperationItems.MergeInventoryOperationResult(operationResult);
            }
            //_eventPublisher.Publish<CharterInDisApproveArg>(new CharterInDisApproveArg() { SendObject = this });
        }

        #endregion

        #region BusinessRule

        #region CharterIn

        //B02 DisApprove
        void IsLastCharterInStartWithOutChaterInEnd(long charterId, long companyId, long vesselInCompanyId)
        {
            if (!_charterInDomainService.IsLastCharterInStartWithOutChaterInEnd(charterId, companyId, vesselInCompanyId))
                throw new BusinessRuleException("B02 DisApprove", "Charter In Start dosent last or has charter In End");
        }


        //B1,B46
        void IsNotFinalApprove()
        {
            if (_charterInDomainService.GetCharterState(this.Id) == States.Submitted)
                throw new BusinessRuleException("B1,B46", "Charter is final Approve");
        }

        //B2 
        void IsValidCharterInStartDate()
        {
            if (!(_charterInDomainService.
                IsCharterStartDateGreaterThanPrevCharterEndDate(this.VesselInCompanyId.Value, this.ActionDate)))
                throw new BusinessRuleException("B2", "Charter In Start date must  Greater than Previous Charter In End");

        }

        //B4
        void IsValidateCharterInStartProp()
        {
            if (!(this.VesselInCompanyId > 0
                && this.ActionDate != null
                && this.OwnerId > 0))
                throw new BusinessRuleException("B4", "Charter In Start is Not Valid");
        }

        //B8,B47
        void HasCharterInHeader(long charterId)
        {

            if (!(_charterInDomainService.ExistCharterInHeader(charterId)))
                throw new BusinessRuleException("B8", "CharterItem Has Not Header");
        }

        //B9 chrter in 
        public override void IsGoodAndUnitValidInCompany(long goodId, long goodUnitId)
        {
            IGoodDomainService goodDomainService = ServiceLocator.Current.GetInstance<IGoodDomainService>();
            var good = goodDomainService.GetGoodWithUnit(this.ChartererId.Value, goodId);
            if (!(good != null && good.GoodUnits.Count(gu => gu.Id == goodUnitId) == 1))
                throw new BusinessRuleException("B9", "Good Or Unit Not Define For Company");

        }

        //B10
        void HasCharterItem()
        {
            var res = _charterInDomainService.HasCharterItem(this.Id);

            if (res)
                throw new BusinessRuleException("B10", "CharterIn Has Items");
        }

        //B11
        void HasCharterEnd()
        {
            if (_charterInDomainService.HasCharterEnd(this.Id))
                throw new BusinessRuleException("B11", "Charter Is End");
        }





        //B13
        void IsChangePropRejectSubmitted(long ownerId, long vesselInCompanyId, long currencyId,
                       DateTime actionDate)
        {
            if (this.CurrentState == States.SubmitRejected)
                if (!(this.OwnerId.Value == ownerId
                    && this.VesselInCompanyId.Value == vesselInCompanyId
                    && this.CurrencyId == currencyId
                    && this.ActionDate == actionDate))
                    throw new BusinessRuleException("B13", "Owner & Vessel & Currency & StartDate Shoulde'nt change");
        }

        //B14,B54
        void IsNotRejectFinalApprove()
        {
            if (_charterInDomainService.GetCharterState(this.Id) == States.SubmitRejected)
                throw new BusinessRuleException("B54,B14", "Charter is reject final Approve");
        }



        //B17
        void HasVesselCharterInStart()
        {
            if (_charterInDomainService.HasVesselCharterInStart(this.VesselInCompanyId.Value))
                throw new BusinessRuleException("B54,B14", "Vessel Has  Charter In start ");
        }

        //B16
        void IsChangeValidRejectSubmited(long id, decimal feeOffhire, long goodId, long tankId)
        {
            if (_charterInDomainService.GetCharterState(this.Id) == States.SubmitRejected)
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

            if (_charterInDomainService.GetCharterState(this.Id) == States.SubmitRejected)
            {
                IOffhireDomainService offhireDomainService = ServiceLocator.Current.GetInstance<IOffhireDomainService>();
                var res = _charterInDomainService.GetCharterEnd(this.Id);
                if (res != null)
                    if (offhireDomainService.
                             IsOffhireRegisteredForVessel(this.VesselInCompanyId.Value, this.ActionDate, res.ActionDate))
                        if (this.OffHirePricingType != offHirePricingType)
                            throw new BusinessRuleException("B18", "Offhire Registerd for this charter in");
            }
        }



        //B45
        void IsValidEndDate()
        {
            var startDate = _charterInDomainService.GetCharterStartDate(this.VesselInCompanyId.Value, this.ChartererId.Value);
            if (!(this.ActionDate > startDate))
                throw new BusinessRuleException("B45", "End Date Must Greater than Start Date");
        }

        //B48
        void ExistFinalApprove()
        {
            if (!(_charterInDomainService.HasVesselCharterInStart(this.VesselInCompanyId.Value, this.ChartererId.Value)))
                throw new BusinessRuleException("B48", "Charter In Start Submited Not Exist");

        }

        //B53
        void IsChangeValidForSubmitRejectedCharter(DateTime actionDate, long currencyId, CharterEndType charterEndType)
        {
            if (_charterInDomainService.GetCharterState(this.Id) == States.SubmitRejected)
                if (!(actionDate == this.ActionDate
                    && currencyId == this.CurrencyId
                    && charterEndType == this.CharterEndType))
                    throw new BusinessRuleException("B53", "Charter End Invalid");

        }

        //B55
        void IsVesselCharterOut()
        {
            IVesselInCompanyDomainService vesselDomainService = ServiceLocator.Current.GetInstance<IVesselInCompanyDomainService>();
            var vessel = vesselDomainService.Get(this.VesselInCompanyId.Value);
            if (vessel.VesselStateCode == VesselStates.CharterOut)
                throw new BusinessRuleException("B55", "Vessel is charter Out");

        }

        //B56 
        void IsEndDateGreaterThanLastCharterOutEndDate()
        {
            var res = _charterOutDomainService.GetDateEndLast(this.VesselInCompanyId.Value);

            if (res != null)
                if (!(this.ActionDate > res.ActionDate))
                    throw new BusinessRuleException("B56 ", "");

        }

        //B57
        void IsLastCharterInEnd()
        {
            if (!(_charterInDomainService.IsLastCharter(this.VesselInCompanyId.Value, this.Id)))
                throw new BusinessRuleException("B57", "Charter In has another last charter end ");

        }

        //B58
        void HasVesselCharterInEnd()
        {
            if (_charterInDomainService.HasVesselCharterInEnd(this.VesselInCompanyId.Value))
                throw new BusinessRuleException("B54,B14", "Vessel Has  Charter In end ");
        }

        //B59
        void CompareOffhirePricing(OffHirePricingType offHirePricingType)
        {
            var res = _charterOutDomainService.GetCharterOutStart(this.VesselInCompanyId.Value, this.OwnerId.Value,
                                                                 this.ActionDate);
            if (res != null)
                if (!(res.OffHirePricingType == offHirePricingType))
                    throw new BusinessRuleException("B59", "Offhire Registerd Has not same type");


        }

        //B60
        void CompareOffhireValueItem(long goodID, decimal offhireFee)
        {

            var res = _charterOutDomainService.GetCharterOutStart(this.VesselInCompanyId.Value, this.OwnerId.Value,
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



        //B61
        void HasAtleastCharterItem()
        {
            var res = _charterInDomainService.GetCharterInStart(this.Id);
            if (!(res.CharterItems.Count > 0))
                throw new BusinessRuleException("B61", "Charter has not Charter Item");

        }

        //B62
        void IsInactiveVessel()
        {
            IVesselInCompanyDomainService vesselDomainService = ServiceLocator.Current.GetInstance<IVesselInCompanyDomainService>();
            var res = vesselDomainService.Get(this.VesselInCompanyId.Value);
            if (!(res.VesselStateCode == VesselStates.Inactive))
                throw new BusinessRuleException("B62", "Vessel is not idle");
        }

        //B63
        void IsFinalApprove()
        {
            if (_charterInDomainService.GetCharterState(this.Id) != States.Submitted)
                throw new BusinessRuleException("B63", "Charter is not final Approve");
        }



        //B66
        void IsVesselCharterIn()
        {
            IVesselInCompanyDomainService vesselDomainService = ServiceLocator.Current.GetInstance<IVesselInCompanyDomainService>();
            var vessel = vesselDomainService.Get(this.VesselInCompanyId.Value);
            if (!(vessel.VesselStateCode == VesselStates.CharterIn))
                throw new BusinessRuleException("B55", "Vessel is not charter In");

        }

        //B67
        void IsSubmitedCharterInStart()
        {
            var res = _charterInDomainService.GetCharterStartState(this.VesselInCompanyId.Value, this.ChartererId.Value);
            if (!(res == States.Submitted))
                throw new BusinessRuleException("B67", "Charter In start is submited");
        }

        //B68
        void IssuanceEndOfVoyageIsSubmitedForVessel()
        {
            IInventoryManagementDomainService inventoryManagementDomainService
                = ServiceLocator.Current.GetInstance<IInventoryManagementDomainService>();
            //if(!(inventoryManagementDomainService.IssuanceEndOfVoyageIsSubmitedForVessel(this.VesselInCompanyId.Value)))
            //    throw new BusinessRuleException("B68","All Issue for vessel is not valid");

        }

        //B74
        void IsEqualValueOfFuelBetweenCharterInOut(long id, long goodId, decimal rob)
        {

            var newGoodId = GetGoodIdOtherCompany(goodId);
            var res = _charterOutDomainService.GetCharterStartDate(id, this.VesselInCompanyId.Value, this.OwnerId.Value);
            if (res != null)
            {
                var itm = res.CharterItems.SingleOrDefault(c => c.GoodId == newGoodId);

                if (itm.Rob != rob)
                    throw new BusinessRuleException("B74", "Value of Rob diffrent with CharterOut");
            }


        }

        //B90
        void IsEqualDateCharterOutStart(long id, DateTime dateTime)
        {

            var res = _charterOutDomainService.GetCharterStartDate(id, this.VesselInCompanyId.Value, this.OwnerId.Value);
            if (res != null)
            {
                if (res.ActionDate != dateTime)
                    throw new BusinessRuleException("B90", "Value of Date diffrent with CharterOut");
            }
        }


        //B91
        void IsEqualDateCharterOutEnd(DateTime dateTime)
        {

            var res = _charterOutDomainService.GetCharterEnd(this.Id);
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
            var res = _charterOutDomainService.GetCharterEnd(this.Id);
            if (res != null)
            {
                var itm = res.CharterItems.SingleOrDefault(c => c.GoodId == newGoodId);

                if (itm.Rob != rob)
                    throw new BusinessRuleException("B92", "Value of Rob diffrent with CharterOut");
            }


        }

        #endregion





        #endregion

        long GetGoodIdOtherCompany(long goodID)
        {
            this._goodRepository = ServiceLocator.Current.GetInstance<IGoodRepository>();
            var shardGoodId = _goodRepository.FindByKey(goodID).SharedGood.Id;
            var newGoodId =
                _goodRepository.Single(c => c.SharedGood.Id == shardGoodId && c.Company.Id == this.OwnerId.Value).Id;
            return newGoodId;
        }

        List<CharterItem> GetChangeQuantityListCharterItems()
        {
            var result = new List<CharterItem>();
            var historylist = _charterInDomainService.GetCharterItemHistories(this.Id, this.CurrentState);

            foreach (var charterItem in this.CharterItems)
            {
                var res =
                    historylist.Where(c => c.CharterItemId == charterItem.Id).OrderBy(c => c.Id).LastOrDefault();
                if (res != null && (res.Rob != charterItem.Rob))
                    result.Add(charterItem);
            }
            return result;
        }


    }

}
