using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Application.Facade
{
   [Interceptor(typeof(SecurityInterception))]
    public class VoucherSetingFacadeService : IVoucherSetingFacadeService
    {

        #region Prop

        private IVoucherSetingRepository _voucherSetingRepository;
        private IVoucherSetingToDtoMapper _voucherSetingToDtoMapper;
        private IVoucherSetingDetailToDtoMapper _voucherSetingDetailToDtoMapper;
        private IVoucherSetingApplicationService _voucherSetingApplicationService;


        #endregion


        public VoucherSetingFacadeService(IVoucherSetingRepository voucherSetingRepository
                                          , IVoucherSetingToDtoMapper voucherSetingToDtoMapper
                                          , IVoucherSetingDetailToDtoMapper voucherSetingDetailToDtoMapper
                                          ,IVoucherSetingApplicationService voucherSetingApplicationService
                                          )
        {
            _voucherSetingRepository = voucherSetingRepository;
            _voucherSetingToDtoMapper = voucherSetingToDtoMapper;
            _voucherSetingApplicationService = voucherSetingApplicationService;
            _voucherSetingDetailToDtoMapper = voucherSetingDetailToDtoMapper;
        }

        public PageResultDto<VoucherSetingDto> GetByFilter(long companyId, int voucherTypeId, int voucherDetailTypeId, int pageIndex, int pageSize)
        {

            var query = _voucherSetingRepository.Find(c =>
                (c.CompanyId == companyId )
                && (c.VoucherDetailTypeId == voucherDetailTypeId || voucherDetailTypeId==0)
                && (c.VoucherTypeId == voucherTypeId || voucherTypeId==0)
                , new ListFetchStrategy<VoucherSeting>(Enums.FetchInUnitOfWorkOption.NoTracking)).AsQueryable();

            var totalCount = query.Count();
            var voucherSetings = query.OrderBy(c => c.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

            var res = new List<VoucherSetingDto>();

            voucherSetings.ForEach(c =>
                                   {
                                       var item=new VoucherSetingDto();
                                       item = _voucherSetingToDtoMapper.MapToDtoModel(c);
                                       item.VoucherSetingDetails = _voucherSetingDetailToDtoMapper.MapToDtoModel(c.VoucherSetingDetails.Where(e=>!e.IsDelete).Select(d=>d).ToList());
                                       item.HistoryVoucherSetingDetails = _voucherSetingDetailToDtoMapper.MapToDtoModel(c.VoucherSetingDetails.Where(e =>e.IsDelete).Select(d => d).ToList());
                                       res.Add(item);
                                   });
            
            return new PageResultDto<VoucherSetingDto>()
                   {
                       TotalCount = totalCount,
                       PageSize = pageSize,
                       Result = res,
                       CurrentPage = pageIndex,
                       TotalPages = Convert.ToInt32(Math.Ceiling(decimal.Divide(totalCount, pageSize)))

                   };
        }

        public VoucherSetingDto GetById(long id)
        {
            var res = new VoucherSetingDto();
            var voucherSeting = _voucherSetingRepository.Find(c => c.Id == id,
                new ListFetchStrategy<VoucherSeting>(Enums.FetchInUnitOfWorkOption.NoTracking)
                .Include(d => d.VoucherSetingDetails).Include(x=>x.VoucherSetingDetails.Select(d=>d.Good))
                .Include(y => y.VoucherSetingDetails.SelectMany(s => s.AsgnSegmentTypeVoucherSetingDetails))).FirstOrDefault();
           
            if (voucherSeting != null)
            {
               
                res = _voucherSetingToDtoMapper.MapToDtoModel(voucherSeting);
                res.VoucherSetingDetails =
                    _voucherSetingDetailToDtoMapper.MapToDtoModel(voucherSeting.VoucherSetingDetails.Where(c=>!c.IsDelete).Select(d=>d).ToList());
            }
            return res;
        }


        public VoucherSetingDetailDto GetDetailById(long id, long detailId)
        {
            var res = new VoucherSetingDetailDto();
            var voucherSeting = _voucherSetingRepository.Find(c => c.Id == id,
                new ListFetchStrategy<VoucherSeting>(Enums.FetchInUnitOfWorkOption.NoTracking)
                .Include(d => d.VoucherSetingDetails).Include(x => x.VoucherSetingDetails.Select(d => d.Good))
                .Include(y => y.VoucherSetingDetails.SelectMany(s => s.AsgnSegmentTypeVoucherSetingDetails))).FirstOrDefault();

            if (voucherSeting != null)
            {

                res = _voucherSetingDetailToDtoMapper.MapToDtoModel(voucherSeting.VoucherSetingDetails.Single(c=>c.Id==detailId));
            }
            return res;
        }


        public void AddVoucherSeting(VoucherSetingDto voucherSetingDto)
        {

            _voucherSetingApplicationService.AddVoucherSeting(voucherSetingDto.Company.Id,voucherSetingDto.VoucherDetailTypeId,  voucherSetingDto.VoucherTypeId,voucherSetingDto.VoucherMainRefDescription ,voucherSetingDto.VoucherMainDescription);

        }


        public void AddVoucherSetingDetail(VoucherSetingDetailDto voucherSetingDto)
        {
            _voucherSetingApplicationService.AddVoucherSetingDetail(voucherSetingDto.GoodDto.Id,
                                                                    voucherSetingDto.VoucherSetingId,
                                                                    voucherSetingDto.VoucherDebitDescription,
                                                                    voucherSetingDto.VoucherDebitRefDescription,
                                                                    voucherSetingDto.VoucherCreditDescription,                                                            voucherSetingDto.VoucherCeditRefDescription,
                                                                    voucherSetingDto.DebitSegmentTypes,
                                                                    voucherSetingDto.DebitAccountDto.Id,
                                                                    voucherSetingDto.CreditSegmentTypes,
                                                                    voucherSetingDto.CreditAccountDto.Id);
        }


        public void UpdateVoucherSeting(VoucherSetingDto voucherSetingDto)
        {
            _voucherSetingApplicationService.UpdateVoucherSeting(voucherSetingDto.Id,voucherSetingDto.Company.Id,voucherSetingDto.VoucherDetailTypeId,voucherSetingDto.VoucherTypeId,voucherSetingDto.VoucherMainRefDescription,voucherSetingDto.VoucherMainDescription);
            
        }

        public void UpdateVoucherSetingDetail(VoucherSetingDetailDto voucherSetingDto)
        {
            _voucherSetingApplicationService.UpdateVoucherSetingDetail(
                voucherSetingDto.Id,
                voucherSetingDto.GoodDto.Id,
                voucherSetingDto.VoucherSetingId,
                voucherSetingDto.VoucherDebitDescription,
                voucherSetingDto.VoucherDebitRefDescription,
                voucherSetingDto.VoucherCreditDescription,
                voucherSetingDto.VoucherCeditRefDescription,
                voucherSetingDto.DebitSegmentTypes,
                voucherSetingDto.DebitAccountDto.Id,
                voucherSetingDto.CreditSegmentTypes,
                voucherSetingDto.CreditAccountDto.Id
                
                );
        }


        public void UpdateDelete(long voucherSetingId, long voucherSetingDetailId)
        {
            _voucherSetingApplicationService.UpdateDelete(voucherSetingId, voucherSetingDetailId);
        }
    }
}
