using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Facade;
using Omu.ValueInjecter;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class EffectiveFactorDtoMapper : BaseFacadeMapper<EffectiveFactor, EffectiveFactorDto>, IEffectiveFactorMapper
    {
        public EffectiveFactorDtoMapper()
        {
            
        }
        public override EffectiveFactorDto MapToModel(EffectiveFactor entity)
        {
            var model = base.MapToModel(entity);
            model.EffectiveFactorType = MapEffectiveFactorTypeEnum(entity.EffectiveFactorType);
            return model;
        }

        private EffectiveFactorTypeEnum MapEffectiveFactorTypeEnum(EffectiveFactorTypes effectiveFactorType)
        {
            switch (effectiveFactorType)
            {
                case EffectiveFactorTypes.Decrease:
                    return EffectiveFactorTypeEnum.Decrease;
                    break;
                case EffectiveFactorTypes.Increase:
                    return EffectiveFactorTypeEnum.InCrease;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("effectiveFactorType");
            }
        }
    }
}