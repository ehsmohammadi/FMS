using System;
using MITD.Fuel.Presentation.Contracts.Enums;
using System.Collections.Generic;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class FuelReportCommandDto
    {
        public string VesselReportReference { get; set; }

        public string VesselCode { get; set; }

        public FuelReportTypeEnum FuelReportType { get; set; }

        public DateTime ReportDate { get; set; }

        public DateTime EventDate { get; set; }

        public string VoyageNumber { get; set; }

        public bool IsActive { get; set; }

        public IList<FuelReportCommandDetailDto> FuelReportDetails { get; set; }

        public string Remark { get; set; }
    }

    public partial class FuelReportCommandDetailDto
    {
        public string FuelType { get; set; }

        public decimal Consumption { get; set; }

        public decimal ROB { get; set; }

        public decimal? Transfer { get; set; }

        public decimal? Receive { get; set; }

        public decimal? Correction { get; set; }

        public CorrectionTypeEnum ? CorrectionType { get; set; }

        public decimal? CorrectionPrice { get; set; }

        public string CorrectionCurrencyCode { get; set; }

        public string TankCode { get; set; }

        public string MeasuringUnitCode { get; set; }
    }
}
