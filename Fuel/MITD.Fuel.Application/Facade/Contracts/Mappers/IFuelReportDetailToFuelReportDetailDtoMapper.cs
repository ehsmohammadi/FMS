using System;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IFuelReportDetailToFuelReportDetailDtoMapper : IFacadeMapper<FuelReportDetail, FuelReportDetailDto>
    {
        FuelReportDetailDto MapToModel(FuelReportDetail entity, Action<FuelReportDetail, FuelReportDetailDto> action);

        TransferTypeEnum MapEntityTransferTypeToDtoTransferType(
            TransferTypes? transferType);

        TransferTypes? MapDtoTransferTypeTypeToEntityTransferTypeType(
            TransferTypeEnum? transferType);

        ReceiveTypeEnum MapEntityReceiveTypeToDtoReceiveType(
            ReceiveTypes? receiveType);

        ReceiveTypes? MapDtoReceiveTypeTypeToEntityReceiveTypeType(
            ReceiveTypeEnum? receiveType);


        CorrectionTypeEnum MapEntityCorrectionTypeToDtoCorrectionType(Domain.Model.Enums.CorrectionTypes? correctionType);
        CorrectionPricingTypeEnum MapEntityCorrectionPricingTypeToDtoCorrectionPricingType(CorrectionPricingTypes? correctionPricingType);

        CorrectionTypes? MapDtoCorrectionTypeToEntityCorrectionType(CorrectionTypeEnum? correctionType);
        CorrectionPricingTypes? MapDtoCorrectionPricingTypeToEntityCorrectionPricingType(CorrectionPricingTypeEnum? correctionPricingType);

        IEnumerable<FuelReportDetailDto> MapToModel(IEnumerable<FuelReportDetail> list, Action<FuelReportDetail, FuelReportDetailDto> action);
    }
}
