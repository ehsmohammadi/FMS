using System.Linq;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Specifications;
namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class OffhireDetail
    {
        //================================================================================

        private readonly IsOffhireSubmitRejected isOffhireSubmitRejected;
        private const long NOT_REGISTERED_ID = -1;

        public long Id { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal FeeInVoucherCurrency { get; private set; }
        public decimal FeeInMainCurrency { get; private set; }
        public long GoodId { get; private set; }
        public virtual Good Good { get; private set; }
        public long UnitId { get; private set; }
        public virtual GoodUnit Unit { get; private set; }
        public long? TankId { get; private set; }
        public virtual Tank Tank { get; private set; }

        public long OffhireId { get; private set; }
        public virtual Offhire Offhire { get; private set; }

        public byte[] TimeStamp { get; set; }

        //================================================================================

        private OffhireDetail(
            decimal quantity,
            decimal feeInVoucherCurrency,
            decimal feeInMainCurrency,
            Good good,
            GoodUnit unit,
            Tank tank,
            Offhire offhire)
            : this()
        {
            //This constructor added to be used for insert into DB.
            this.Quantity = quantity;
            this.FeeInVoucherCurrency = feeInVoucherCurrency;
            this.FeeInMainCurrency = feeInMainCurrency;
            this.Good = good;
            this.Unit = unit;
            this.Tank = tank;
            this.Offhire = offhire;
        }

        public OffhireDetail()
        {
            this.Id = NOT_REGISTERED_ID;
            this.isOffhireSubmitRejected = new IsOffhireSubmitRejected();

            //this.InventoryOperations = new List<InventoryOperation>();
        }

        internal OffhireDetail(decimal quantity, decimal feeInVoucherCurrency, decimal feeInMainCurrency, Good good, GoodUnit unit, Tank tank, Offhire offhire,
            IOffhireDomainService offhireDomainService, ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
            : this()
        {
            this.validateOffhire(offhire, offhireDomainService);

            this.validateValues(quantity, feeInVoucherCurrency, feeInMainCurrency, good, unit, tank, offhire, offhireDomainService,
                tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);

            this.Quantity = quantity;
            this.FeeInVoucherCurrency = feeInVoucherCurrency;
            this.FeeInMainCurrency = feeInMainCurrency;
            this.Good = good;
            this.Unit = unit;
            this.Tank = tank;
            this.Offhire = offhire;
        }

        //================================================================================

        private void validateValues(decimal quantity, decimal feeInVoucherCurrency, decimal feeInMainCurrency, Good good, GoodUnit unit, Tank tank, Offhire offhire,
            IOffhireDomainService offhireDomainService, ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            this.validateTank(tank, offhire, tankDomainService);

            this.validateGood(good, goodDomainService);
            this.validateGoodInTank(tank, good, offhire);
            this.validateGoodToBeUniqueInDetails(good, offhire);

            this.validateQuantity(quantity);

            this.validateFeeInMainCurrency(feeInMainCurrency);

            this.validateUnit(unit, goodUnitDomainService);
            this.validateGoodUnitInCompany(offhire.Introducer, good, unit);

            this.validateFeeInVoucherCurrency(feeInVoucherCurrency, offhire, good, offhireDomainService, currencyDomainService);
        }

        //================================================================================

        private void validateGoodUnitInCompany(Company company, DomainObjects.Good good, GoodUnit unit)
        {
            if (!(good.CompanyId == company.Id && good.GoodUnits.Count(gu => gu.Id == unit.Id) == 1))
                throw new BusinessRuleException("", string.Format("Selected Good '{0}' and Unit is not defined for Company.", good.Name));
        }

        //================================================================================

        private void validateFeeInVoucherCurrency(decimal feeInVoucherCurrency, Offhire offhire, Good good, IOffhireDomainService offhireDomainService, ICurrencyDomainService currencyDomainService)
        {
            if (feeInVoucherCurrency < 0)
                throw new BusinessRuleException("", string.Format("Fee In Voucher Currency is negative for '{0}'.", good.Name));

            var pricingValue = offhireDomainService.GetPricingValue(offhire.Introducer, offhire.VesselInCompany, offhire.StartDateTime, good);

            switch (offhire.PricingReference.Type)
            {
                case OffHirePricingType.CharterPartyBase:

                    if (pricingValue != null &&
                        pricingValue.Fee.HasValue && pricingValue.Currency != null)
                    {
                        var charterPartyConvertedOffhireFee = currencyDomainService.ConvertPrice(pricingValue.Fee.Value, pricingValue.Currency.Id, offhire.VoucherCurrencyId, offhire.VoucherDate);

                        if (feeInVoucherCurrency != charterPartyConvertedOffhireFee)
                            throw new BusinessRuleException("", string.Format("The Offhire Fee for Good '{0}' is invlaid.", good.Name));
                    }

                    break;

                case OffHirePricingType.IssueBase:

                    if (pricingValue == null || !pricingValue.Fee.HasValue || pricingValue.Currency == null)
                        throw new BusinessRuleException("", string.Format("IssueBased Pricing for Good '{0}' not found.", good.Name));

                    var issueBaseConvertedOffhireFee = currencyDomainService.ConvertPrice(pricingValue.Fee.Value, pricingValue.Currency.Id, offhire.VoucherCurrencyId, offhire.VoucherDate);

                    if (feeInVoucherCurrency != issueBaseConvertedOffhireFee)
                        throw new BusinessRuleException("", string.Format("The Offhire Fee for Good '{0}' is invlaid.", good.Name));

                    break;

                default:
                    throw new InvalidArgument("PricingReferenceType");
            }
        }

        //================================================================================

        private void validateOffhire(Offhire offhire, IOffhireDomainService offhireDomainService)
        {
            if (offhire == null)
                throw new InvalidArgument("Offhire");

            //Commented intentionally due to registration of Details along with the Offhire.
            //if (offhireDomainService.Get(offhire.Id) == null)
            //    throw new ObjectNotFound("Offhire", offhire.Id);
        }

        //================================================================================

        private void validateGoodInTank(Tank tank, Good good, Offhire offhire)
        {
            if (good == null)
                throw new InvalidArgument("Good");

            if (tank != null)
            {
                var registeredTanksForGoodCount = offhire.OffhireDetails.Count(od =>
                    (od.Id != this.Id || (od.Id == NOT_REGISTERED_ID && this.Id == NOT_REGISTERED_ID)) &&
                    od.Tank != null && od.Tank.Id == tank.Id && od.Good.Id == good.Id);

                if (registeredTanksForGoodCount > 0)
                    throw new BusinessRuleException("", string.Format("The same Good '{0}' in Tank is already defined.", good.Name));
            }
        }

        //================================================================================

        private void validateGoodToBeUniqueInDetails(Good good, Offhire offhire)
        {
            if (offhire.OffhireDetails.Count(od =>
                (od.Id != this.Id || (od.Id == NOT_REGISTERED_ID && this.Id == NOT_REGISTERED_ID)) &&
                od.Good.Id == good.Id) != 0)
                throw new BusinessRuleException("", string.Format("The Good '{0}' has other records in Offhire Details.", good.Name));
        }

        //================================================================================

        private void validateGood(Good good, IGoodDomainService goodDomainService)
        {
            if (good == null)
                throw new BusinessRuleException("", "Good must be selected.");

            if (goodDomainService.Get(good.Id) == null)
                throw new ObjectNotFound("Good", good.Id);
        }

        //================================================================================

        private void validateUnit(GoodUnit unit, IGoodUnitDomainService goodUnitDomainService)
        {
            if (unit == null)
                throw new BusinessRuleException("", "Unit must be selected.");

            if (goodUnitDomainService.Get(unit.Id) == null)
                throw new ObjectNotFound("Unit", unit.Id);
        }

        //================================================================================

        private void validateFeeInMainCurrency(decimal feeInMainCurrency)
        {
            if (feeInMainCurrency < 0)
                throw new BusinessRuleException("", "Rial Fee is negative.");
        }

        //================================================================================

        private void validateQuantity(decimal quantity)
        {
            if (quantity < 0)
                throw new BusinessRuleException("", "Quantity is negative.");
        }

        //================================================================================

        private void validateTank(Tank tank, Offhire offhire, ITankDomainService tankDomainService)
        {
            this.validateTankExistance(tank, tankDomainService);

            //validateTankForVesselWithTanks(tank, offhire);

            //validateTankForVesselWithoutTanks(tank, offhire);

            //validateTankToBeInVesselTanks(tank, offhire);
        }

        //================================================================================

        private void validateTankToBeInVesselTanks(Tank tank, Offhire offhire)
        {
            if (offhire.VesselInCompany.Tanks.Count > 0 && tank != null && offhire.VesselInCompany.Tanks.Count(t => t.Id == tank.Id) == 0)
                throw new BusinessRuleException("", "Selected Tank is not defined for Vessel.");
        }

        //================================================================================

        private void validateTankForVesselWithoutTanks(Tank tank, Offhire offhire)
        {
            if (offhire.VesselInCompany.Tanks.Count == 0 && tank != null)
                throw new BusinessRuleException("", "Tank should not be selected for Vessel.");
        }

        //================================================================================

        private void validateTankForVesselWithTanks(Tank tank, Offhire offhire)
        {
            if (offhire.VesselInCompany.Tanks.Count > 0 && tank == null)
                throw new BusinessRuleException("", "Tank must be selected.");
        }

        //================================================================================

        private void validateTankExistance(Tank tank, ITankDomainService tankDomainService)
        {
            if (tank != null && tankDomainService.Get(tank.Id) == null)
                throw new ObjectNotFound("Tank", tank.Id);
        }

        //================================================================================

        internal void Update(decimal feeInVoucherCurrency, decimal feeInMainCurrency,
            IOffhireDomainService offhireDomainService, ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            this.validateValues(this.Quantity, feeInVoucherCurrency, feeInMainCurrency, this.Good, this.Unit, this.Tank, this.Offhire,
                offhireDomainService, tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);

            this.FeeInVoucherCurrency = feeInVoucherCurrency;
            this.FeeInMainCurrency = feeInMainCurrency;
        }

        //================================================================================

        public void Validate(IOffhireDomainService offhireDomainService, ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            this.validateValues(this.Quantity, this.FeeInVoucherCurrency, this.FeeInMainCurrency, this.Good, this.Unit, this.Tank, this.Offhire,
                 offhireDomainService, tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);
        }

        //================================================================================
    }
}