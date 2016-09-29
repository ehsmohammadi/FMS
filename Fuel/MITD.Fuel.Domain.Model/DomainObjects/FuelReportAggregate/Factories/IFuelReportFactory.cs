using System;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Factories;

namespace MITD.Fuel.Domain.Model.DomainObjects.Factories
{
    public interface IFuelReportFactory : IFactory
    {
        FuelReport CreateFuelReport(string code, string description, DateTime eventDate, DateTime reportDate, long vesselInCompanyId, long? voyageId, FuelReportTypes fuelReportType, long userId, bool isVesselInActiveState);

        FuelReportDetail CreateFuelReportDetail(long fuelReportId, decimal rob, string robUOM, decimal consumption, decimal? receive, ReceiveTypes? receiveType, decimal? transfer, TransferTypes? transferType, decimal? correction, CorrectionTypes? correctionType, CorrectionPricingTypes? correctionPricingType, decimal? correctionPrice, string correctionPriceCurrencyISOCode, long? correctionPriceCurrencyId, long fuelTypeId, long measuringUnitId, long tankId);
    }
}