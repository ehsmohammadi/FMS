using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class VoucherSetingDetailToDtoMapper : IVoucherSetingDetailToDtoMapper
    {
        
        
        public VoucherSetingDetailDto MapToDtoModel(VoucherSetingDetail voucherSeting)
        {
            var res= new VoucherSetingDetailDto()
                   {
                       Id = voucherSeting.Id,
                       VoucherSetingId = voucherSeting.VoucherSetingId,
                       VoucherCeditRefDescription = voucherSeting.VoucherCeditRefDescription,
                       VoucherCreditDescription = voucherSeting.VoucherCreditDescription,
                       VoucherDebitDescription = voucherSeting.VoucherDebitDescription,
                       VoucherDebitRefDescription = voucherSeting.VoucherDebitRefDescription,
                       GoodDto = new GoodDto  { Id=voucherSeting.Good.Id,Name =voucherSeting.Good.Name},

                       
                      
                   };

            res.CreditSegmentTypes = new List<int>();
            voucherSeting.CreditSegmentTypes.ForEach(c =>
                                                     {   
                                                         res.CreditSegmentTypes.Add(c.SegmentTypeId);
                                                     });
            res.DebitSegmentTypes = new List<int>();
            voucherSeting.DebitSegmentTypes.ForEach(c =>
                                                    {

                                                        res.DebitSegmentTypes.Add(c.SegmentTypeId);
                                                    });

            voucherSeting.AsgnVoucherAconts.ForEach(c =>
                                                    {
                                                      var acc =new AccountDto()
                                                                                 {
                                                                                     Id = c.Account.Id,
                                                                                     Name = c.Account.Name,
                                                                                     Code = c.Account.Code
                                                                                 };

                                                        if (c.IsCredit)
                                                            res.CreditAccountDto = acc;
                                                        else
                                                            res.DebitAccountDto = acc;
                                                    }
            
        );


            return res;


        }

        public List<VoucherSetingDetailDto> MapToDtoModel(List<VoucherSetingDetail> voucherSetings)
        {
            var res = new List<VoucherSetingDetailDto>();
            
            voucherSetings.ForEach(c=>res.Add(MapToDtoModel(c)));
            return res;
        }

        public IEnumerable<VoucherSetingDetail> MapToEntity(IEnumerable<VoucherSetingDetailDto> models)
        {
            throw new NotImplementedException();
        }

        public VoucherSetingDetail MapToEntity(VoucherSetingDetailDto model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VoucherSetingDetailDto> MapToModel(IEnumerable<VoucherSetingDetail> entities)
        {
            throw new NotImplementedException();
        }

        public VoucherSetingDetailDto MapToModel(VoucherSetingDetail entity)
        {
            throw new NotImplementedException();
        }

        public VoucherSetingDetailDto RemapModel(VoucherSetingDetailDto model)
        {
            throw new NotImplementedException();
        }
    }
}
