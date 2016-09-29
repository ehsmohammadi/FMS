using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Presentation.Contracts.DTOs.Report
{
    public partial class VesselEventReportViewDto
    {
        public int Id { get; set; } // ID

        public int? DraftId { get; set; } // DraftID

        public string ShipCode { get; set; } // ShipCode

        public string ConsNo { get; set; } // ConsNo

        public string ShipName { get; set; } // ShipName

        public string VoyageNo { get; set; } // VoyageNo

        public int? Year { get; set; } // Year

        public int? Month { get; set; } // Month

        public int? Day { get; set; } // Day

        public string PortName { get; set; } // PortName

        public double? PortTime { get; set; } // PortTime

        public double? AtSeaLatitudeDegree { get; set; } // AtSeaLatitudeDegree

        public double? AtSeaLatitudeMinute { get; set; } // AtSeaLatitudeMinute

        public double? AtSeaLongitudeDegree { get; set; } // AtSeaLongitudeDegree

        public double? AtSeaLongitudeMinute { get; set; } // AtSeaLongitudeMinute

        public double? ObsDist { get; set; } // ObsDist

        public double? EngDist { get; set; } // EngDist

        public double? SteamTime { get; set; } // SteamTime

        public decimal? AvObsSpeed { get; set; } // AvObsSpeed

        public double? AvEngSpeed { get; set; } // AvEngSpeed

        public int? Rpm { get; set; } // RPM

        public double? Slip { get; set; } // Slip

        public double? WindDir { get; set; } // WindDir

        public double? WindForce { get; set; } // WindForce

        public double? SeaDir { get; set; } // SeaDir

        public double? SeaForce { get; set; } // SeaForce

        public decimal? Robho { get; set; } // ROBHO

        public decimal? Robdo { get; set; } // ROBDO

        public decimal? Robmgo { get; set; } // ROBMGO

        public decimal? Robfw { get; set; } // ROBFW

        public decimal? ConsInPortHo { get; set; } // ConsInPortHO

        public decimal? ConsInPortDo { get; set; } // ConsInPortDO

        public decimal? ConsInPortMgo { get; set; } // ConsInPortMGO

        public decimal? ConsInPortFw { get; set; } // ConsInPortFW

        public decimal? ConsAtSeaHo { get; set; } // ConsAtSeaHO

        public decimal? ConsAtSeaDo { get; set; } // ConsAtSeaDO

        public decimal? ConsAtSeaMgo { get; set; } // ConsAtSeaMGO

        public decimal? ConsAtSeaFw { get; set; } // ConsAtSeaFW

        public decimal? ReceivedHo { get; set; } // ReceivedHO

        public decimal? ReceivedDo { get; set; } // ReceivedDO

        public decimal? ReceivedMgo { get; set; } // ReceivedMGO

        public decimal? ReceivedFw { get; set; } // ReceivedFW

        public string EtaPort { get; set; } // ETAPort

        public string EtaDate { get; set; } // ETADate

        public DateTime? Date { get; set; } // Date

        public DateTime? DateIn { get; set; } // DateIn

        public double? DailyFuelCons { get; set; } // DailyFuelCons

        public decimal? Speed { get; set; } // Speed

        public bool? IsSm { get; set; } // IsSM

        public string InPortOrAtSea { get; set; } // InPortOrAtSea

        public string ImportDate { get; set; } // ImportDate

        public decimal? TransferHo { get; set; } // TransferHo

        public decimal? TransferDo { get; set; } // TransferDo

        public decimal? TransferFw { get; set; } // TransferFW

        public decimal? TransferMgols { get; set; } // TransferMGOLS

        public decimal? CorrectionHo { get; set; } // CorrectionHo

        public decimal? CorrectionDo { get; set; } // CorrectionDo

        public decimal? CorrectionFw { get; set; } // CorrectionFW

        public decimal? CorrectionMgols { get; set; } // CorrectionMGOLS

        public string CorrectionTypeHo { get; set; } // CorrectionTypeHo

        public string CorrectionTypeDo { get; set; } // CorrectionTypeDo

        public string CorrectionTypeFw { get; set; } // CorrectionTypeFW

        public string CorrectionTypeMgols { get; set; } // CorrectionTypeMGOLS

        public TimeSpan? Time { get; set; } // Time

        public byte? FuelReportType { get; set; } // FuelReportType

        public byte? State { get; set; } // State

        public string ReportTypeName { get; set; } // ReportTypeName

        public string LocationTypeName { get; set; } // LocationTypeName
    }
}
