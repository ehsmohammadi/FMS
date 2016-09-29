using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Services.Facade;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
{
    public class FuelReportDetailToFuelReportDetailDtoMapper : BaseFacadeMapper<FuelReportDetail, FuelReportDetailDto>, IFuelReportDetailToFuelReportDetailDtoMapper
    {
        private readonly IGoodToGoodDtoMapper goodMapper;
        private readonly ICurrencyToCurrencyDtoMapper currencyMapper;
        private readonly ITankToTankDtoMapper tankDtoMapper;

        public FuelReportDetailToFuelReportDetailDtoMapper(
            IGoodToGoodDtoMapper goodMapper,
            ICurrencyToCurrencyDtoMapper currencyMapper,
            ITankToTankDtoMapper tankDtoMapper)
        {
            this.goodMapper = goodMapper;

            this.currencyMapper = currencyMapper;
            this.tankDtoMapper = tankDtoMapper;
        }

        public override FuelReportDetailDto MapToModel(FuelReportDetail entity)
        {
            GoodDto goodDto = this.goodMapper.MapEntityToDtoWithUnits(entity.Good);

            goodDto.Unit = new GoodUnitDto
                               {
                                   Id = entity.MeasuringUnitId,
                                   Name = entity.MeasuringUnit.Name
                               };

            CurrencyDto currencyDto = this.currencyMapper.MapEntityToDto(entity.CorrectionPriceCurrency);

            
            var dto = new FuelReportDetailDto()
                          {
                              Id = entity.Id,
                              Consumption = entity.Consumption,
                              Correction = /*entity.Correction == null ? 0 :*/ entity.Correction,
                              CorrectionPrice = /*entity.CorrectionPrice == null ? 0 :*/ (double?)entity.CorrectionPrice,
                              CorrectionType = MapEntityCorrectionTypeToDtoCorrectionType(entity.CorrectionType),
                              FuelReportId = entity.FuelReportId,

                              ReceiveType = MapEntityReceiveTypeToDtoReceiveType(entity.ReceiveType),
                              Recieve = /*entity.Receive == null ? 0 :*/ entity.Receive,
                              ROB = entity.ROB,
                              Transfer = /*entity.Transfer == null ? 0 :*/ entity.Transfer,

                              TransferType = MapEntityTransferTypeToDtoTransferType(entity.TransferType),

                              Good = goodDto,
                              CurrencyDto = currencyDto,
                              GoodId = entity.GoodId,
                              GoodUnitId = entity.MeasuringUnitId,
                              TankId = entity.TankId,
                              Tank = tankDtoMapper.MapToModel( entity.Tank)
                          };

            dto.FuelReportCorrectionReferenceNoDto = new FuelReportCorrectionReferenceNoDto();
            if (entity.CorrectionReference != null && entity.CorrectionReference.ReferenceId.HasValue)
            {
                dto.FuelReportCorrectionReferenceNoDto.Id = entity.CorrectionReference.ReferenceId.Value;
                dto.FuelReportCorrectionReferenceNoDto.Code = entity.CorrectionReference.Code;
            }

            dto.FuelReportReciveReferenceNoDto = new FuelReportReciveReferenceNoDto();
            if (entity.ReceiveReference != null && entity.ReceiveReference.ReferenceId.HasValue)
            {
                dto.FuelReportReciveReferenceNoDto.Id = entity.ReceiveReference.ReferenceId.Value;
                dto.FuelReportReciveReferenceNoDto.Code = entity.ReceiveReference.Code;
            }

            dto.FuelReportTransferReferenceNoDto = new FuelReportTransferReferenceNoDto();
            if (entity.TransferReference != null && entity.TransferReference.ReferenceId.HasValue)
            {
                dto.FuelReportTransferReferenceNoDto.Id = entity.TransferReference.ReferenceId.Value;
                dto.FuelReportTransferReferenceNoDto.Code = entity.TransferReference.Code;
            }

            return dto;
        }

        public TransferTypeEnum MapEntityTransferTypeToDtoTransferType(Domain.Model.Enums.TransferTypes? transferType)
        {
            switch (transferType)
            {
                case MITD.Fuel.Domain.Model.Enums.TransferTypes.Rejected:
                    return TransferTypeEnum.Rejected;

                case MITD.Fuel.Domain.Model.Enums.TransferTypes.InternalTransfer:
                    return TransferTypeEnum.InternalTransfer;

                case MITD.Fuel.Domain.Model.Enums.TransferTypes.TransferSale:
                    return TransferTypeEnum.TransferSale;

                case null:
                default:
                    return TransferTypeEnum.NotDefined;
            }

            //return transferType.HasValue ? transferType.ToString() : null;
        }

        public Domain.Model.Enums.TransferTypes? MapDtoTransferTypeTypeToEntityTransferTypeType(TransferTypeEnum? transferType)
        {
            switch (transferType)
            {
                case TransferTypeEnum.Rejected:
                    return MITD.Fuel.Domain.Model.Enums.TransferTypes.Rejected;

                case TransferTypeEnum.InternalTransfer:
                    return MITD.Fuel.Domain.Model.Enums.TransferTypes.InternalTransfer;

                case TransferTypeEnum.TransferSale:
                    return MITD.Fuel.Domain.Model.Enums.TransferTypes.TransferSale;

                case null:
                default:
                    return null;

            }
        }

        public ReceiveTypeEnum MapEntityReceiveTypeToDtoReceiveType(Domain.Model.Enums.ReceiveTypes? receiveType)
        {
            switch (receiveType)
            {
                case MITD.Fuel.Domain.Model.Enums.ReceiveTypes.Trust:
                    return ReceiveTypeEnum.Trust;

                case MITD.Fuel.Domain.Model.Enums.ReceiveTypes.InternalTransfer:
                    return ReceiveTypeEnum.InternalTransfer;

                case MITD.Fuel.Domain.Model.Enums.ReceiveTypes.TransferPurchase:
                    return ReceiveTypeEnum.TransferPurchase;

                case MITD.Fuel.Domain.Model.Enums.ReceiveTypes.Purchase:
                    return ReceiveTypeEnum.Purchase;
                case null:
                default:
                    return ReceiveTypeEnum.NotDefined;
            }

            //return receiveType.HasValue ? receiveType.ToString() : null;
        }

        public Domain.Model.Enums.ReceiveTypes? MapDtoReceiveTypeTypeToEntityReceiveTypeType(ReceiveTypeEnum? receiveType)
        {
            switch (receiveType)
            {
                case ReceiveTypeEnum.Trust:
                    return MITD.Fuel.Domain.Model.Enums.ReceiveTypes.Trust;

                case ReceiveTypeEnum.InternalTransfer:
                    return MITD.Fuel.Domain.Model.Enums.ReceiveTypes.InternalTransfer;

                case ReceiveTypeEnum.TransferPurchase:
                    return MITD.Fuel.Domain.Model.Enums.ReceiveTypes.TransferPurchase;

                case ReceiveTypeEnum.Purchase:
                    return MITD.Fuel.Domain.Model.Enums.ReceiveTypes.Purchase;

                case null:
                default:
                    return null;
            }
        }


        public CorrectionTypeEnum MapEntityCorrectionTypeToDtoCorrectionType(CorrectionTypes? correctionType)
        {
            switch (correctionType)
            {
                case CorrectionTypes.Minus: return CorrectionTypeEnum.Minus;
                case CorrectionTypes.Plus: return CorrectionTypeEnum.Plus;
                case null:
                default: return CorrectionTypeEnum.NotDefined;
            }

            //return correctionType.HasValue ? correctionType.ToString() : null;
        }

        public CorrectionTypes? MapDtoCorrectionTypeToEntityCorrectionType(CorrectionTypeEnum? correctionType)
        {
            switch (correctionType)
            {
                case CorrectionTypeEnum.Minus: return CorrectionTypes.Minus;
                case CorrectionTypeEnum.Plus: return CorrectionTypes.Plus;
                case null:
                default:
                    return null;
            }
        }
    }
}