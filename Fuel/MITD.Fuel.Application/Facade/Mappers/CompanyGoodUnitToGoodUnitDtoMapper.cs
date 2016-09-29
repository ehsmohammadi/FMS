using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Services.Facade;
using Omu.ValueInjecter;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class CompanyGoodUnitToGoodUnitDtoMapper : BaseFacadeMapper<GoodUnit, GoodUnitDto>, ICompanyGoodUnitToGoodUnitDtoMapper
    {
        public override GoodUnitDto MapToModel(GoodUnit good)
        {

            var model = base.MapToModel(good);
            model.Name = good.Name;
            return model;
        }
    }
}