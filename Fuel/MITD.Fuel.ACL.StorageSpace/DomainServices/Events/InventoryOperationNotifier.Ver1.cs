using MITD.Fuel.ACL.StorageSpace.InventoryServiceReference;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;
using MITD.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CharterType = MITD.Fuel.ACL.StorageSpace.InventoryServiceReference.CharterType;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events
{
    public class InventoryOperationNotifier : IInventoryOperationNotifier
    {
        private FuelServiceClient svc;

        private readonly ICharterInToDtoMapper charterInToDtoMapper;
        private readonly ICharterOutToDtoMapper charterOutToDtoMapper;
        private readonly ICharterItemToDtoMapper charterItemToDtoMapper;
        private readonly IScrapToScrapDtoMapper scrapToScrapDtoMapper;
        private readonly IFuelReportDetailToFuelReportDetailDtoMapper fuelReportDetailToFuelReportDetailDtoMapper;
        private readonly IInvoiceToDtoMapper invoiceToDtoMapper;
        private readonly IFuelReportFuelReportDtoMapper fuelReportToFuelReportDtoMapper;

        public InventoryOperationNotifier(
            ICharterInToDtoMapper charterInToDtoMapper
            , ICharterOutToDtoMapper charterOutToDtoMapper
            , ICharterItemToDtoMapper charterItemToDtoMapper
            , IScrapToScrapDtoMapper scrapToScrapDtoMapper
            , IFuelReportFuelReportDtoMapper fuelReportToFuelReportDtoMapper
            , IFuelReportDetailToFuelReportDetailDtoMapper fuelReportDetailToFuelReportDetailDtoMapper
            , IInvoiceToDtoMapper invoiceToDtoMapper)
        {
            //svc = new FuelServiceClient();
            //svc.ClientCredentials.UserName.UserName = "OnlineInquireOnlineUser";
            //svc.ClientCredentials.UserName.Password = "A@ty&90Ty^Vv";

            this.charterInToDtoMapper = charterInToDtoMapper;
            this.charterOutToDtoMapper = charterOutToDtoMapper;
            this.charterItemToDtoMapper = charterItemToDtoMapper;
            this.scrapToScrapDtoMapper = scrapToScrapDtoMapper;
            this.fuelReportDetailToFuelReportDetailDtoMapper = fuelReportDetailToFuelReportDetailDtoMapper;
            this.invoiceToDtoMapper = invoiceToDtoMapper;
            this.fuelReportToFuelReportDtoMapper = fuelReportToFuelReportDtoMapper;
        }


        public List<InventoryOperation> NotifySubmittingFuelReportDetail(FuelReportDetail source)
        {
            //var fuelReportDetailDto = fuelReportDetailToFuelReportDetailDtoMapper.MapToModel(source);

            //fuelReportDetailDto.FuelReport = fuelReportToFuelReportDtoMapper.MapToModel(source.FuelReport);

            //var inventoryOperationDtosResult = svc.NotifySubmittingFuelReportDetail(fuelReportDetailDto).ToList();

            //var result = inventoryOperationDtosResult.Select(mapInventoryOperationDtoToInventoryOperation);

            //return result.ToList();

            return null;
        }

        public List<InventoryOperation> NotifySubmittingScrap(Scrap source)
        {
            //var scrapDto = scrapToScrapDtoMapper.MapToModel(source);


            ////TODO: svc.NotifySubmittingScrapDetail must be renamed to NotifySubmittingScrap
            ////TODO: The argument Type must be changed from ScrapDetailDto to ScrapDto.
            //var inventoryOperationDtosResult = svc.NotifySubmittingScrapDetail(scrapDto).ToList();

            //var result = inventoryOperationDtosResult.Select(mapInventoryOperationDtoToInventoryOperation);

            //return result.ToList();

            return null;
        }

        public List<InventoryOperation> NotifySubmittingInvoice(Invoice source)
        {
            //var invoiceDto = invoiceToDtoMapper.MapToModel(source);

            //var inventoryOperationDtosResult = svc.NotifySubmittingInvoice(invoiceDto).ToList();

            //var result = inventoryOperationDtosResult.Select(mapInventoryOperationDtoToInventoryOperation);

            //return result.ToList();

            return null;
        }

        public List<InventoryOperation> NotifySubmittingCharterInStart(CharterIn charterInStart)
        {
            var charterInDto = charterInToDtoMapper.MapToDtoModel(charterInStart);

            charterInDto.CharterType = CharterType.In;

            charterInDto.CharterItems = charterInStart.CharterItems.Select(charterItemToDtoMapper.MapToDtoModel).ToList();

            //var inventoryOperationDtosResult = svc.NotifySubmittingCharterInStart(charterInDto).ToList();
            var inventoryOperationDtosResult = new List<FuelReportInventoryOperationDto>();

            ClientHelper.Post<List<FuelReportInventoryOperationDto>, CharterDto>
                (new Uri("http://localhost:65234/api/fuelevents", UriKind.Absolute),
                (result, exp) =>
                {
                    inventoryOperationDtosResult = result;
                    var ex = exp;
                },
                charterInDto, ClientHelper.MessageFormat.Json, new Dictionary<string, string>(), "CharterDto");



            return inventoryOperationDtosResult.Select(mapInventoryOperationDtoToInventoryOperation).ToList();

            //return result.ToList();
        }

        private InventoryOperation mapInventoryOperationDtoToInventoryOperation(FuelReportInventoryOperationDto arg)
        {
            var result = new InventoryOperation(arg.Code, arg.ActionDate, mapInventoryActionType(arg.ActionType), null, null);
            return result;
        }

        private Domain.Model.Enums.InventoryActionType mapInventoryActionType(string actionType)
        {
            return (InventoryActionType)Enum.Parse(typeof(InventoryActionType), actionType);
        }

        public List<InventoryOperation> NotifySubmittingCharterInEnd(CharterIn charterInEnd)
        {
            //var charterInDto = charterInToDtoMapper.MapToDtoModel(charterInEnd);

            //charterInDto.CharterType = "In";

            //charterInDto.CharterItems = charterInEnd.CharterItems.Select(charterItemToDtoMapper.MapToDtoModel).ToList();

            //var inventoryOperationDtosResult = svc.NotifySubmittingCharterInEnd(charterInDto).ToList();

            //var result = inventoryOperationDtosResult.Select(mapInventoryOperationDtoToInventoryOperation);

            //return result.ToList();

            return null;
        }


        public List<InventoryOperation> NotifySubmittingCharterOutStart(CharterOut charterOutStart)
        {
            //var charterOutDto = charterOutToDtoMapper.MapToDtoModel(charterOutStart);

            //charterOutDto.CharterType = "Out";

            //charterOutDto.CharterItems = charterOutStart.CharterItems.Select(charterItemToDtoMapper.MapToDtoModel).ToList();

            //var inventoryOperationDtosResult = svc.NotifySubmittingCharterOutStart(charterOutDto).ToList();

            //var result = inventoryOperationDtosResult.Select(mapInventoryOperationDtoToInventoryOperation);

            //return result.ToList();

            return null;
        }

        public List<InventoryOperation> NotifySubmittingCharterOutEnd(CharterOut charterOutEnd)
        {
            //var charterOutDto = charterOutToDtoMapper.MapToDtoModel(charterOutEnd);

            //charterOutDto.CharterType = "Out";

            //charterOutDto.CharterItems = charterOutEnd.CharterItems.Select(charterItemToDtoMapper.MapToDtoModel).ToList();

            //var inventoryOperationDtosResult = svc.NotifySubmittingCharterOutEnd(charterOutDto).ToList();

            //var result = inventoryOperationDtosResult.Select(mapInventoryOperationDtoToInventoryOperation);

            //return result.ToList();

            return null;
        }

        public string test(string s)
        {
            //return svc.test(s);

            return null;
        }
    }
}