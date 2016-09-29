using System;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.Enums.Inventory;

namespace MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations
{
    public interface IInventoryOperationNotifier : IEventNotifier
    {
        InventoryOperationResult NotifySubmittingFuelReportConsumption(FuelReport fuelReport, long userId);

        InventoryOperationResult NotifySubmittingFuelReportDetail(FuelReportDetail source, IFuelReportDomainService fuelReportDomainService, long userId);

        void NotifySubmittingFuelReportDetailByFinance(FuelReportDetail source, IFuelReportDomainService fuelReportDomainService, long userId);

        List<InventoryOperation> NotifySubmittingScrap(Scrap source, long approverId);

        InventoryOperation NotifySubmittingOrderItemBalance(OrderItemBalance orderItemBalance, long userId);
        void RevertOrderItemBalancePricing(OrderItemBalance orderItemBalance, long userId);
        void RevertFuelReportDetailCorrectionPricing(FuelReportDetail fuelReportDetail, long userId);
        InventoryOperationResult NotifySubmittingCharterInStart(Voyage voyage, CharterIn charterInStart, long userId);
        InventoryOperationResult NotifyCharterInStartResubmit(Voyage voyage, CharterIn charterInStart, long userId, bool vesselShouldBeDeactivated);

        InventoryOperationResult NotifySubmittingCharterInEnd(Voyage voyage, CharterIn charterInEnd, long userId);
        InventoryOperationResult NotifyCharterInEndResubmit(Voyage voyage, CharterIn charterInEnd, long userId, bool vesselShouldBeDeactivated);

        InventoryOperationResult NotifySubmittingCharterOutStart(Voyage voyage, CharterOut charterOutStart, long userId);
        InventoryOperationResult NotifyCharterOutStartResubmit(Voyage voyage, CharterOut charterOutStart, long userId, bool vesselShouldBeDeactivated);

        InventoryOperationResult NotifySubmittingCharterOutEnd(Voyage voyage, CharterOut charterOutEnd, long userId);
        InventoryOperationResult NotifyCharterOutEndResubmit(Voyage voyage, CharterOut charterOutEnd, long userId, bool vesselShouldBeDeactivated);

        InventoryOperationResult RevertFuelReportConsumptionInventoryOperations(FuelReport fuelReport, int userId);
        InventoryOperationResult RevertFuelRpeortDetailReceiveInventoryOperations(FuelReportDetail fuelReportDetail, int userId);
        InventoryOperationResult RevertFuelRpeortDetailTransferInventoryOperations(FuelReportDetail fuelReportDetail, int userId);
        InventoryOperationResult RevertFuelRpeortDetailCorrectionInventoryOperations(FuelReportDetail fuelReportDetail, int userId);

        InventoryOperationResult RevertCharterInStartInventoryOperations(CharterIn charterInStart, int userId);
        InventoryOperationResult RevertCharterInEndInventoryOperations(CharterIn charterInEnd, int userId);
        InventoryOperationResult RevertCharterOutStartInventoryOperations(CharterOut charterOutStart, int userId);
        InventoryOperationResult RevertCharterOutEndInventoryOperations(CharterOut charterOutEnd, int userId);
    }
}