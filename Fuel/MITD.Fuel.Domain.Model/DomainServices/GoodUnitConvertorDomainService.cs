using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainServices
{
    public class GoodUnitConvertorDomainService:IGoodUnitConvertorDomainService
    {
        private readonly IGoodDomainService _goodDomainService;
        private readonly IGoodUnitDomainService goodUnitDomainService;

        public GoodUnitConvertorDomainService(IGoodDomainService goodDomainService,IGoodUnitDomainService goodUnitDomainService)
        {
            _goodDomainService = goodDomainService;
            this.goodUnitDomainService = goodUnitDomainService;
        }

        public MainUnitValue GetUnitValueInMainUnit(long goodId, long goodUnitId, decimal value)
        {
            var good = _goodDomainService.GetGoodWithUnitAndMainUnit(goodId, goodUnitId);
            var goodUnit = good.GoodUnits.Single(c => c.Id == goodUnitId);
            var result = ConvertUnitValueToMainUnitValue(goodUnit, value);
            return new MainUnitValue {Id = good.SharedGood.MainUnit.Id, Name = good.SharedGood.MainUnit.Name, Value = result};
        }

        public decimal ConvertUnitValueToMainUnitValue(GoodUnit goodUnit, decimal value)
        {
            if (goodUnit.Parent == null)
                return value;

            return goodUnit.Coefficient * value;
        }

        public decimal ConvertUnitValueToMainUnitValue(long goodUnitId, decimal value)
        {
            var goodUnit = goodUnitDomainService.Get(goodUnitId);
            return ConvertUnitValueToMainUnitValue(goodUnit, value);
        }

        public decimal ConvertMainUnitValueToUnitValue(GoodUnit goodUnit, decimal value)
        {
            if (goodUnit.Parent == null)
                return value;

            return value / goodUnit.Coefficient;
        }

        public decimal ConvertMainUnitValueToUnitValue(long goodUnitId, decimal value)
        {
            var goodUnit = goodUnitDomainService.Get(goodUnitId);
            return ConvertMainUnitValueToUnitValue(goodUnit, value);
        }

        public decimal ConvertUnits(long goodId, long fromUnitId, long toUnitId, decimal quantity)
        {
            var units = goodUnitDomainService.GetGoodUnits(goodId);
            var fromUnit = units.Single(c => c.Id == fromUnitId);
            var toUnit = units.Single(c => c.Id == toUnitId);
            var result = ConvertUnitValueToMainUnitValue(fromUnit, quantity);
              result = ConvertMainUnitValueToUnitValue(toUnit, result);
            return result;
        }

        public List<GoodUnit> Get(List<long> IDs)
        {
            throw new NotImplementedException();
        }

        public GoodUnit Get(long id)
        {
            throw new NotImplementedException();
        }

        public List<GoodUnit> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}