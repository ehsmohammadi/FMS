using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
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