using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class VoucherToDtoMapper : IVoucherToDtoMapper
    {
        public VoucherDto MapToDtoModel(Voucher voucher)
        {
            var res= new VoucherDto()
                   {
                       Id=voucher.Id,
                       Company = new CompanyDto() {Id=voucher.Company.Id,Code=voucher.Company.Code },
                       Description=voucher.Description,
                       FinancialVoucherDate = voucher.FinancialVoucherDate,
                       LocalVoucherDate = voucher.LocalVoucherDate,
                       LocalVoucherNo = voucher.LocalVoucherNo,
                       ReferenceNo = voucher.ReferenceNo,
                       //ReferenceType = voucher.ReferenceType.Name,
                       VoucherRef = voucher.VoucherRef,
                       VoucherDetailTypeId = voucher.VoucherDetailTypeId,
                       State = (voucher.FinancialVoucherState == null) ? 2 : voucher.FinancialVoucherState.Value,
                       FinancialVoucherNo = voucher.FinancialVoucherNo,
                      
                      
                   };
            res.FinancialVoucherDate = (res.State == 1) ? voucher.FinancialVoucherDate.Substring(0, 4) + "/" + voucher.FinancialVoucherDate.Substring(4, 2) + "/" + voucher.FinancialVoucherDate.Substring(6, 2) : "";
            return res;
        }
        public List<VoucherDto> MapToDtoModel(List<Voucher> vouchers)
        {
            var res = new List<VoucherDto>();

            vouchers.ForEach(c => res.Add(MapToDtoModel(c)));

            return res;
        }
        public IEnumerable<Voucher> MapToEntity(IEnumerable<VoucherDto> models)
        {
            throw new NotImplementedException();
        }

        public Voucher MapToEntity(VoucherDto model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VoucherDto> MapToModel(IEnumerable<Voucher> entities)
        {
            throw new NotImplementedException();
        }

        public VoucherDto MapToModel(Voucher entity)
        {
            throw new NotImplementedException();
        }

        public VoucherDto RemapModel(VoucherDto model)
        {
            throw new NotImplementedException();
        }
    }
}
