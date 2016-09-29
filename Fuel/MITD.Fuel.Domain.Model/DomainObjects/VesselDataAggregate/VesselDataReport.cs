using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.DomainObjects.VesselDataAggregate
{
    public class VesselDataReport
    {
        public long Id { get; set; }
        public string VesselDataReferenceCode { get; set; }

        public string VesselCode { get; set; }

        public FuelReportTypes FuelReportType { get; set; }

        public DateTime ReportDate { get; set; }

        public DateTime EventDate { get; set; }

        public string VoyageNumber { get; set; }

        public bool IsActive { get; set; }

        public IList<FuelReportCommandDetailDto>[] FuelReportDetails { get; set; }
    }

    public partial class FuelReportCommandDetailDto
    {
        public string FuelType { get; set; }

        public decimal Consumption { get; set; }

        public decimal ROB { get; set; }

        public decimal? Transfer { get; set; }

        public decimal? Recieve { get; set; }

        public decimal? Correction { get; set; }

        public decimal? CorrectionPrice { get; set; }

        public string CorrectionCurrencyCode { get; set; }

        public string TankCode { get; set; }

        public string MeasuringUnitCode { get; set; }

    }

}
