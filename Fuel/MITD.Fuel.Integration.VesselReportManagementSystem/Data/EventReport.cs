//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MITD.Fuel.Integration.VesselReportManagementSystem.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class EventReport
    {
        public int ID { get; set; }
        public Nullable<int> DraftID { get; set; }
        public string ShipCode { get; set; }
        public string ConsNo { get; set; }
        public string ShipName { get; set; }
        public string VoyageNo { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> Day { get; set; }
        public string PortName { get; set; }
        public Nullable<double> PortTime { get; set; }
        public Nullable<double> AtSeaLatitudeDegree { get; set; }
        public Nullable<double> AtSeaLatitudeMinute { get; set; }
        public Nullable<double> AtSeaLongitudeDegree { get; set; }
        public Nullable<double> AtSeaLongitudeMinute { get; set; }
        public Nullable<double> ObsDist { get; set; }
        public Nullable<double> EngDist { get; set; }
        public Nullable<double> SteamTime { get; set; }
        public Nullable<decimal> AvObsSpeed { get; set; }
        public Nullable<double> AvEngSpeed { get; set; }
        public Nullable<int> RPM { get; set; }
        public Nullable<double> Slip { get; set; }
        public Nullable<double> WindDir { get; set; }
        public Nullable<double> WindForce { get; set; }
        public Nullable<double> SeaDir { get; set; }
        public Nullable<double> SeaForce { get; set; }
        public Nullable<decimal> ROBHO { get; set; }
        public Nullable<decimal> ROBDO { get; set; }
        public Nullable<decimal> ROBMGO { get; set; }
        public Nullable<decimal> ROBFW { get; set; }
        public Nullable<decimal> ConsInPortHO { get; set; }
        public Nullable<decimal> ConsInPortDO { get; set; }
        public Nullable<decimal> ConsInPortMGO { get; set; }
        public Nullable<decimal> ConsInPortFW { get; set; }
        public Nullable<decimal> ConsAtSeaHO { get; set; }
        public Nullable<decimal> ConsAtSeaDO { get; set; }
        public Nullable<decimal> ConsAtSeaMGO { get; set; }
        public Nullable<decimal> ConsAtSeaFW { get; set; }
        public Nullable<decimal> ReceivedHO { get; set; }
        public Nullable<decimal> ReceivedDO { get; set; }
        public Nullable<decimal> ReceivedMGO { get; set; }
        public Nullable<decimal> ReceivedFW { get; set; }
        public string ETAPort { get; set; }
        public string ETADate { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> DateIn { get; set; }
        public Nullable<double> DailyFuelCons { get; set; }
        public Nullable<decimal> Speed { get; set; }
        public Nullable<bool> IsSM { get; set; }
        public string InPortOrAtSea { get; set; }
        public string ImportDate { get; set; }
        public Nullable<decimal> TransferHo { get; set; }
        public Nullable<decimal> TransferDo { get; set; }
        public Nullable<decimal> TransferFW { get; set; }
        public Nullable<decimal> TransferMGOLS { get; set; }
        public Nullable<decimal> CorrectionHo { get; set; }
        public Nullable<decimal> CorrectionDo { get; set; }
        public Nullable<decimal> CorrectionFW { get; set; }
        public Nullable<decimal> CorrectionMGOLS { get; set; }
        public string CorrectionTypeHo { get; set; }
        public string CorrectionTypeDo { get; set; }
        public string CorrectionTypeFW { get; set; }
        public string CorrectionTypeMGOLS { get; set; }
        public Nullable<System.TimeSpan> Time { get; set; }
        public Nullable<byte> FuelReportType { get; set; }
        public Nullable<byte> State { get; set; }
        public Nullable<decimal> ROBatNoonIfo { get; set; }
        public Nullable<decimal> ROBatNoonDoLs { get; set; }
        public Nullable<decimal> ROBatNoonHoLs { get; set; }
        public Nullable<decimal> ROBatNoonIfoLs { get; set; }
        public Nullable<decimal> ConsumpAtSeaIfo { get; set; }
        public Nullable<decimal> ConsumpAtSeaHoLs { get; set; }
        public Nullable<decimal> ConsumpInPortHoLs { get; set; }
        public Nullable<decimal> ConsumpInPortDoLs { get; set; }
        public Nullable<decimal> ConsumpAtSeaIfoLs { get; set; }
        public Nullable<decimal> ConsumpInPortIfo { get; set; }
        public Nullable<decimal> ConsumpAtSeaDoLs { get; set; }
        public Nullable<decimal> ConsumpInPortIfoLs { get; set; }
        public Nullable<decimal> RecivedAtPortIFO { get; set; }
        public Nullable<decimal> RecivedAtPortHoLs { get; set; }
        public Nullable<decimal> TransferIfo { get; set; }
        public Nullable<decimal> RecivedAtPortDoLs { get; set; }
        public Nullable<decimal> RecivedAtPortIfoLs { get; set; }
        public Nullable<decimal> TransferHoLs { get; set; }
        public Nullable<decimal> TransferDoLs { get; set; }
        public Nullable<decimal> TransferIfoLs { get; set; }
        public Nullable<decimal> CorrectionIfo { get; set; }
        public Nullable<decimal> CorrectionHoLs { get; set; }
        public Nullable<decimal> CorrectionDoLs { get; set; }
        public Nullable<decimal> CorrectionIfoLs { get; set; }
        public string CorrectionTypeIfo { get; set; }
        public string CorrectionTypeHoLs { get; set; }
        public string CorrectionTypeDoLs { get; set; }
        public string CorrectionTypeIfoLs { get; set; }
        public string ETATime { get; set; }
        public string ErrorMessage { get; set; }
    }
}
