using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Presentation.Contracts;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Services.Facade;
using OffHirePricingType = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.OffHirePricingType;


namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
{
    public class CharterInToDtoMapper : BaseFacadeMapper<CharterIn, CharterDto>, ICharterInToDtoMapper
    {
        private readonly IVesselToVesselDtoMapper vesselToVesselDtoMapper;

        public CharterInToDtoMapper(IVesselToVesselDtoMapper vesselToVesselDtoMapper)
        {
            this.vesselToVesselDtoMapper = vesselToVesselDtoMapper;
        }

        public CharterDto MapToDtoModel(CharterIn charterIn)
        {
            var res = new CharterDto()
                          {
                              Id = charterIn.Id,
                              Owner = (charterIn.Owner != null) ? base.Map(new CompanyDto(), charterIn.Owner) as CompanyDto : null,
                              Charterer = (charterIn.Charterer != null) ? base.Map(new CompanyDto(), charterIn.Charterer) as CompanyDto : null,
                              Vessel = (charterIn.VesselInCompany != null) ? vesselToVesselDtoMapper.MapToModel(charterIn.VesselInCompany) : null,
                              Currency = (charterIn.Currency != null) ? base.Map(new CurrencyDto(), charterIn.Currency) as CurrencyDto : null,
                              OffHirePricingType = OffhireToDtoConvertor(charterIn.OffHirePricingType),
                              CurrentStateName = GetStateName(charterIn.CurrentState),
                              IsFinalApproveVisiblity = (charterIn.CurrentState != States.Submitted)


                          };

            if (charterIn.CharterType == MITD.Fuel.Domain.Model.Enums.CharterType.Start)
            {
                res.StartDate = charterIn.ActionDate;

            }
            else
            {
                res.EndDate = charterIn.ActionDate;
                res.CharterEndType = CharterEndTypeConvertor(charterIn.CharterEndType);
            }

            res.CharterItems = new ObservableCollection<CharterItemDto>();
            res.InventoryOperationDtos = new ObservableCollection<FuelReportInventoryOperationDto>();

            return res;
        }

        public PageResultDto<CharterDto> MapToDtoModels(PageResult<CharterIn> charterIns)
        {
            var dtos = new PageResultDto<CharterDto>();
            dtos.Result = new List<CharterDto>();
            charterIns.Result.ToList().ForEach(c => dtos.Result.Add(MapToDtoModel(c)));

            dtos.TotalCount = charterIns.TotalCount;
            dtos.TotalPages = charterIns.TotalPages;
            dtos.CurrentPage = charterIns.CurrentPage;
            return dtos;

        }


        public string GetStateName(States states)
        {
            var res = "ثبت";
            switch (states)
            {
                case States.Open:
                    break;
                case States.Submitted:
                    res = "تایید نهایی";
                    break;
                case States.Closed:
                    break;
                case States.Cancelled:
                    break;
                case States.SubmitRejected:
                    res = "برگشت از تایید نهایی";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("states");
            }
            return res;
        }

        public CharterEndTypeEnum CharterEndTypeConvertor(CharterEndType charterEndType)
        {
            var res = CharterEndTypeEnum.None;
            switch (charterEndType)
            {
                case CharterEndType.None:
                    res = CharterEndTypeEnum.None;
                    break;
                case CharterEndType.DryDock:
                    res = CharterEndTypeEnum.DryDock;
                    break;
                case CharterEndType.CharterInEnd:
                    res = CharterEndTypeEnum.CharterInEnd;
                    break;

                case CharterEndType.LayUp:
                    res = CharterEndTypeEnum.LayUp;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("charterEndType");
            }
            return res;

            //return charterEndType.ToString();
        }




        public OffHirePricingType OffhireToDtoConvertor(Domain.Model.Enums.OffHirePricingType pricingType)
        {
            if (pricingType == MITD.Fuel.Domain.Model.Enums.OffHirePricingType.CharterPartyBase)
            {
                return OffHirePricingType.CharterPartyBase;
            }
            else
            {
                return OffHirePricingType.IssueBase;
            }
            //return pricingType.ToString();
        }


        public Domain.Model.Enums.OffHirePricingType DtoToOfHreConvertor(OffHirePricingType pricingType)
        {
            if (pricingType == OffHirePricingType.CharterPartyBase)
            {
                return Domain.Model.Enums.OffHirePricingType.CharterPartyBase;
            }
            else
            {
                return Domain.Model.Enums.OffHirePricingType.IssueBase;
            }
        }


        //public CharterEndType CharterEndTypeEnumConvertor(CharterEndTypeEnum charterEndTypeEnum)
        //{
        //    var res = CharterEndType.None;
        //    switch (charterEndTypeEnum)
        //    {
        //        case CharterEndTypeEnum.None:
        //            res = CharterEndType.None;
        //            break;
        //        case CharterEndTypeEnum.DryDock:
        //            res = CharterEndType.DryDock;
        //            break;
        //        case CharterEndTypeEnum.CharterInEnd:
        //            res = CharterEndType.CharterInEnd;
        //            break;

        //        case CharterEndTypeEnum.LayUp:
        //            res = CharterEndType.LayUp;
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException("charterEndTypeEnum");
        //    }
        //    return res;
        //}

        public PageResultDto<FuelReportInventoryOperationDto> MapToInvDtoModels(PageResult<InventoryOperation> entities)
        {
            var dtos = new PageResultDto<FuelReportInventoryOperationDto>();
            dtos.Result = entities.Result.Select(MapToInvDtoModel).ToList();

            return dtos;
        }

        public FuelReportInventoryOperationDto MapToInvDtoModel(InventoryOperation entity)
        {
            var dto = new FuelReportInventoryOperationDto()
            {
                Id = entity.Id,
                Code = entity.ActionNumber,
                ActionType = entity.ActionType.ToString(),
                ActionDate = entity.ActionDate,

            };

            return dto;
        }

    }
}
