using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public   interface IGoodUnitConvertorDomainService : IDomainService<GoodUnit>
    {
        MainUnitValue GetUnitValueInMainUnit(long goodId, long goodUnitId, decimal value);
        
        decimal ConvertUnitValueToMainUnitValue(GoodUnit goodUnit, decimal value);
        decimal ConvertUnitValueToMainUnitValue(long goodUnitId, decimal value);
        
        decimal ConvertMainUnitValueToUnitValue(GoodUnit goodUnit, decimal value);
        decimal ConvertMainUnitValueToUnitValue(long goodUnitId, decimal value);

        decimal ConvertUnits(long goodId, long fromUnitId, long toUnitId, decimal quantity);
    }
}