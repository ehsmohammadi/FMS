using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using MITD.AutomaticVoucher.FinancialService;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.FuelSecurity.Domain.Model.Repository;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class VoucherFacadeService : IVoucherFacadeService
    {
        private IVoucherRepository _voucherRepository;
        private IVoucherToDtoMapper _voucherToDtoMapper;
        private IJournalEntryToDtoMapper _journalEntryToDtoMapper;
        private readonly IUserRepository _userRepository;
        private readonly IInventoryOperationRepository _inventoryOperationRepository;
        private readonly IOffhireRepository _offhireRepository;

        public VoucherFacadeService(IVoucherRepository voucherRepository
            , IVoucherToDtoMapper voucherToDtoMapper, 
            IJournalEntryToDtoMapper journalEntryToDtoMapper,
            IUserRepository userRepository,
           IInventoryOperationRepository inventoryOperationRepository,
            IOffhireRepository offhireRepository

            )
        {
            _voucherRepository = voucherRepository;
            _voucherToDtoMapper = voucherToDtoMapper;
            _journalEntryToDtoMapper = journalEntryToDtoMapper;
            _userRepository = userRepository;
            _inventoryOperationRepository = inventoryOperationRepository;
            _offhireRepository = offhireRepository;
        }

        public PageResultDto<VoucherDto> GetAll(long companyId, DateTime? fromDate, DateTime? toDate, int voucherTyp, string refNo, string state, int pageIndex, int pageSize)
        {
            var fetchstartegy = new ListFetchStrategy<Voucher>(Enums.FetchInUnitOfWorkOption.NoTracking);

            var stat = long.Parse(state);
            IQueryable<Voucher> query = _voucherRepository.Find(c =>
                (c.CompanyId == companyId)
                && (c.FinancialVoucherState == stat || state == "0")
                && (c.LocalVoucherDate <= toDate || toDate == null)
                && (c.LocalVoucherDate > fromDate || fromDate == null)
                && (c.ReferenceNo.ToLower() == refNo.ToLower() || string.IsNullOrEmpty(refNo))
                && (c.VoucherDetailTypeId == voucherTyp || voucherTyp == 0), fetchstartegy).AsQueryable();


            var voucher = query.
                    Skip(pageSize * (pageIndex - 1))
                   .Take(pageSize).ToList();

            var totalCount = query.Count();

            
            return new PageResultDto<VoucherDto>()
                   {
                       Result = _voucherToDtoMapper.MapToDtoModel(voucher)
                       ,
                       TotalCount = totalCount
                       ,
                       CurrentPage = pageIndex//+1
                       ,
                       PageSize = pageSize,
                       TotalPages = Convert.ToInt32(Math.Ceiling(decimal.Divide(totalCount, pageSize)))
                   };
        }

        
        public VoucherDto GetById(long id)
        {
            var fetchstartegy = new ListFetchStrategy<Voucher>(Enums.FetchInUnitOfWorkOption.NoTracking).Include(j => j.JournalEntrieses)
                .Include(c => c.JournalEntrieses.SelectMany(d => d.Segments)); ;
            var voucher = _voucherRepository.Find(c => c.Id == id, fetchstartegy).FirstOrDefault();
            var dto = _voucherToDtoMapper.MapToDtoModel(voucher);
            dto.VoucherTransferLogDto = new List<VoucherTransferLogDto>();
            var serviceLog = new VoucherTransferLogService();
            var logs = serviceLog.GetLogByVoucherId(id);
            logs.ForEach(c =>
            {
                dto.VoucherTransferLogDto.Add(new VoucherTransferLogDto()
                {
                    Id = c.Id,
                    ConfigCode = c.ConfigCode,
                    ConfigDate = c.ConfigDate,
                    FinancialExceptionMessage = c.FinancialExceptionMessage,
                    SendDate = c.SendDate,
                    UserId = c.UserId,
                    VoucherIds = c.VoucherIds,
                    UserName = _userRepository.FindByKey(c.UserId).PartyName
                });
            });
            
            dto.JournalEntryDtos = _journalEntryToDtoMapper.MapToDtoModels(voucher.JournalEntrieses);
            return dto;
        }





        public VoucherEntityDto GetEntityId(string refNo)
        {
            var res = new VoucherEntityDto();

            var result = _inventoryOperationRepository.Single(c => c.ActionNumber.ToLower() == refNo.ToLower());
            var vouch = _voucherRepository.Single(c => c.ReferenceNo.ToLower() == refNo.ToLower());
            if (result.Scrap_Id != null)
            {
                res.Id = result.Scrap_Id.Value;
                res.EntityTypeName = "Scrap";
                return res;
            }
            if (result.FuelReport_Id != null)
            {
                res.Id = result.FuelReport_Id.Value;
                res.EntityTypeName = "FuelReport";
                return res;
            }
            if (result.CharterId != null)
            {
                res.Id = result.CharterId.Value;
                if (vouch.ReferenceTypeId==1)
                {
                    res.EntityTypeName = "CharterIn"; 
                }
                else if (vouch.ReferenceTypeId == 2)
                {
                    res.EntityTypeName = "CharterOut";
                }
               
                return res;
            }
            if (result.FuelReportDetailId != null)
            {
                res.Id = result.FuelReportDetailId.Value;
                res.EntityTypeName = "FuelReportDetail";
                return res;
            }
            var res1 = _offhireRepository.Single(c => c.ReferenceNumber.ToString().ToLower() == refNo.ToLower());
            res.EntityTypeName = "Offhire";
            res.Id = res1.Id;
            return res;
        }
    }
}
