using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.ACL.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.Mapper
{
    public class GoodAntiCorruptionMapper : IGoodAntiCorruptionMapper
    {
        public IEnumerable<Good> MapToEntity(IEnumerable<GoodDto> models)
        {
            var res = new List<Good>();
            models.ToList().ForEach(p => res.Add(MapToEntity(p)));

            return res;
        }

        public Good MapToEntity(GoodDto model)
        {


//            var Units = new Collection<GoodUnit>();
//            model.Units.ForEach(p =>
//                                    {
//                                        var u = new UnitX(p.Id,p.Name);
//                                        Units.Add(u);
//                                    });
////
////            model.Brands.ForEach(p =>
////                                    {
////                                        var b = new Brand(p.Id,p.Name);
////                                        Brands.Add(b);
////                                    });
//            var res = new Good(model.Id, model.Name, model.Code,Units);
//
//            return res;
            return null;
        }

        public IEnumerable<GoodDto> MapToModel(IEnumerable<Good> entities)
        {
            throw new NotImplementedException();
        }

        public GoodDto MapToModel(Good entity)
        {
            throw new NotImplementedException();
        }

        public GoodDto RemapModel(GoodDto model)
        {
            throw new NotImplementedException();
        }
    }
}