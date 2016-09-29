using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Services.Facade;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface IFuelReportDetailToFuelReportDetailDtoMapper : IFacadeMapper<FuelReportDetail, FuelReportDetailDto>
    {
        TransferTypeEnum MapEntityTransferTypeToDtoTransferType(
            TransferTypes? transferType);

        //TransferTypes? MapDtoTransferTypeTypeToEntityTransferTypeType(
        //    string transferType);

        ReceiveTypeEnum MapEntityReceiveTypeToDtoReceiveType(
            ReceiveTypes? receiveType);

        //ReceiveTypes? MapDtoReceiveTypeTypeToEntityReceiveTypeType(
        //    string receiveType);


        CorrectionTypeEnum MapEntityCorrectionTypeToDtoCorrectionType(Domain.Model.Enums.CorrectionTypes? correctionType);

        //CorrectionTypes? MapDtoCorrectionTypeToEntityCorrectionType(string correctionType);
    }
}
