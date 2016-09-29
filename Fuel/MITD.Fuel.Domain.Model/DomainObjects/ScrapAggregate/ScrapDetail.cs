using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Specifications;
namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class ScrapDetail
    {
        //================================================================================

        private readonly IsScrapSubmitRejected isScrapSubmitRejected;

        public long Id { get; private set; }
        public double ROB { get; private set; }
        public double Price { get; private set; }
        public long CurrencyId { get; private set; }
        public virtual Currency Currency { get; private set; }
        public long GoodId { get; private set; }
        public virtual Good Good { get; private set; }
        public long UnitId { get; private set; }
        public virtual GoodUnit Unit { get; private set; }
        public long? TankId { get; private set; }
        public virtual Tank Tank { get; private set; }

        public long ScrapId { get; private set; }
        public virtual Scrap Scrap { get; private set; }
        //public virtual List<InventoryOperation> InventoryOperations { get; private set; }

        public byte[] TimeStamp { get; set; }

        //================================================================================

        private ScrapDetail(
            double rob,
            double price,
            Currency currency,
            Good good,
            GoodUnit unit,
            Tank tank,
            Scrap scrap)
        {
            //This constructor added to be used for insert into DB.
            this.ROB = rob;
            this.Price = price;
            this.Currency = currency;
            this.Good = good;
            this.Unit = unit;
            this.Tank = tank;
            this.Scrap = scrap;

            this.isScrapSubmitRejected = new IsScrapSubmitRejected();

            //this.InventoryOperations = new List<InventoryOperation>();
        }

        public ScrapDetail()
        {
            this.isScrapSubmitRejected = new IsScrapSubmitRejected();

            //this.InventoryOperations = new List<InventoryOperation>();
        }

        internal ScrapDetail(double rob, double price, Currency currency, Good good, GoodUnit unit, Tank tank, Scrap scrap,
            IScrapDomainService scrapDomainService, ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
            : this()
        {
            this.validateScrap(scrap, scrapDomainService);

            this.validateValues(rob, price, currency, good, unit, tank, scrap,
                tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);

            this.ROB = rob;
            this.Price = price;
            this.Currency = currency;
            this.Good = good;
            this.Unit = unit;
            this.Tank = tank;
            this.Scrap = scrap;
        }

        //================================================================================

        private void validateValues(double rob, double price, Currency currency, Good good, GoodUnit unit, Tank tank, Scrap scrap, ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService, IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            this.validateTank(tank, scrap, tankDomainService);

            this.validateGood(good, goodDomainService);
            this.validateGoodInTank(tank, good, scrap);
            this.validateGoodToBeUniqueInDetails(good, scrap);

            this.validateROB(rob);
            this.validatePrice(price);
            this.validateCurrency(currency, currencyDomainService);

            this.validateUnit(unit, goodUnitDomainService);
            this.validateGoodUnitInCompany(scrap.VesselInCompany.Company, good, unit);
        }

        //================================================================================

        private void validateScrap(Scrap scrap, IScrapDomainService scrapDomainService)
        {
            if (scrap == null)
                throw new InvalidArgument("Scrap");

            if (scrapDomainService.Get(scrap.Id) == null)
                throw new ObjectNotFound("Scrap", scrap.Id);
        }

        //================================================================================

        private void validateGoodInTank(Tank tank, Good good, Scrap scrap)
        {
            if (good == null)
                throw new InvalidArgument("Good");

            if (tank != null)
            {
                var registeredTanksForGoodCount = scrap.ScrapDetails.Count(sd => sd.Id != this.Id && sd.Tank != null && sd.Tank.Id == tank.Id && sd.Good.Id == good.Id);

                if (registeredTanksForGoodCount > 0)
                    throw new BusinessRuleException("", string.Format("The same Good '{0}' in Tank is already defined.", good.Name));
            }
        }

        //================================================================================

        private void validateGoodToBeUniqueInDetails(Good good, Scrap scrap)
        {
            if (scrap.ScrapDetails.Count(sd => sd.Good.Id == good.Id && sd.Id != this.Id) != 0)
                throw new BusinessRuleException("", string.Format("The Good '{0}' has other records in Scrap Details.", good.Name));
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

        private void validateCurrency(Currency currency, ICurrencyDomainService currencyDomainService)
        {
            if (currency == null)
                throw new BusinessRuleException("", "Currency must be selected.");

            if (currencyDomainService.Get(currency.Id) == null)
                throw new ObjectNotFound("Currency", currency.Id);
        }

        //================================================================================

        private void validatePrice(double price)
        {
            if (price < 0)
                throw new BusinessRuleException("", "Price is negative.");
        }

        //================================================================================

        private void validateROB(double rob)
        {
            if (rob < 0)
                throw new BusinessRuleException("", "ROB is negative.");
        }

        //================================================================================

        private void validateTank(Tank tank, Scrap scrap, ITankDomainService tankDomainService)
        {
            this.validateTankExistance(tank, tankDomainService);

            validateTankForVesselWithTanks(tank, scrap);

            validateTankForVesselWithoutTanks(tank, scrap);

            validateTankToBeInVesselTanks(tank, scrap);
        }

        //================================================================================

        private void validateTankToBeInVesselTanks(Tank tank, Scrap scrap)
        {
            if (scrap.VesselInCompany.Tanks.Count > 0 && tank != null && scrap.VesselInCompany.Tanks.Count(t => t.Id == tank.Id) == 0)
                throw new BusinessRuleException("", "Selected Tank is not defined for Vessel.");
        }

        //================================================================================

        private void validateTankForVesselWithoutTanks(Tank tank, Scrap scrap)
        {
            if (scrap.VesselInCompany.Tanks.Count == 0 && tank != null)
                throw new BusinessRuleException("", "Tank should not be selected for Vessel.");
        }

        //================================================================================

        private void validateTankForVesselWithTanks(Tank tank, Scrap scrap)
        {
            if (scrap.VesselInCompany.Tanks.Count > 0 && tank == null)
                throw new BusinessRuleException("", "Tank must be selected.");
        }

        //================================================================================

        private void validateTankExistance(Tank tank, ITankDomainService tankDomainService)
        {
            if (tank != null && tankDomainService.Get(tank.Id) == null)
                throw new ObjectNotFound("Tank", tank.Id);
        }

        //================================================================================

        private void validateGoodUnitInCompany(Company company, Good good, GoodUnit unit)
        {
            if (!(good.CompanyId == company.Id && good.GoodUnits.Count(gu => gu.Id == unit.Id) == 1))
                throw new BusinessRuleException("", string.Format("Selected Good '{0}' and Unit is not defined for Company.", good.Name));
        }

        //================================================================================

        internal void Update(double rob, double price, Currency currency, Good good, GoodUnit unit, Tank tank,
            ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            this.validateValues(rob, price, currency, good, unit, tank, this.Scrap,
                 tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);

            this.ROB = rob;
            this.Price = price;
            this.Currency = currency;
            this.Unit = unit;

            if (!this.isScrapSubmitRejected.IsSatisfiedBy(this.Scrap))
            {
                this.Good = good;
                this.Tank = tank;
            }
        }

        //================================================================================

        public void Validate(ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            this.validateValues(this.ROB, this.Price, this.Currency, this.Good, this.Unit, this.Tank, this.Scrap,
                 tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);
        }

        //================================================================================
    }
}