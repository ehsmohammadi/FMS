using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;

namespace MITD.Fuel.Integration.VesselReportManagementSystem.Mapper
{
    public class ReportMapper
    {
        //public static FuelReportCommandDto Map(Data.RPMInfo rpmInfo)
        //{
        //    FuelReportCommandDto res = new FuelReportCommandDto();
        //    res.FuelReportDetails = new List<FuelReportCommandDetailDto>();
        //    res.VesselReportReference = rpmInfo.ID.ToString();

        //    res.VesselCode = rpmInfo.Ship.ShipID;

        //    //res.FuelReportType = GetFuelReportTypeEnum(rpmInfo.FuelReportType);
        //    res.FuelReportType = GetFuelReportTypeEnum(rpmInfo.FuelReportType.Value);

        //    res.EventDate = new DateTime(rpmInfo.Year.Value, rpmInfo.Month.Value, rpmInfo.Day.Value);

        //    //res.EventDate.Add(rpmInfo.Time);
        //    res.EventDate = res.EventDate.Add(rpmInfo.Time.HasValue ? rpmInfo.Time.Value : new TimeSpan(12, 0, 0));

        //    res.ReportDate = DateTime.Now;


        //    res.VoyageNumber = rpmInfo.VoyageNo;

        //    res.Remark = rpmInfo.VoyageNo + " / " + rpmInfo.PortName;

        //    res.IsActive = true;


        //    if (rpmInfo.ROBHO != null)
        //    {
        //        //HFO
        //        var hFoFuelReportCommandDetailDto = new FuelReportCommandDetailDto();
        //        hFoFuelReportCommandDetailDto.Consumption = Convert.ToDecimal(rpmInfo.ConsInPortHO) + Convert.ToDecimal(rpmInfo.ConsAtSeaHO);
        //        hFoFuelReportCommandDetailDto.Transfer = rpmInfo.TransferHo;

        //        hFoFuelReportCommandDetailDto.ROB = Convert.ToDecimal(rpmInfo.ROBHO);

        //        hFoFuelReportCommandDetailDto.Receive = Convert.ToDecimal(rpmInfo.ReceivedHO);

        //        hFoFuelReportCommandDetailDto.Correction = Convert.ToDecimal(rpmInfo.CorrectionHo);
        //        hFoFuelReportCommandDetailDto.CorrectionType =
        //            (rpmInfo.CorrectionTypeHo != null && rpmInfo.CorrectionTypeHo.Value) ? CorrectionTypeEnum.Plus : CorrectionTypeEnum.Minus;

        //        hFoFuelReportCommandDetailDto.MeasuringUnitCode = "TON";
        //        hFoFuelReportCommandDetailDto.FuelType = "HFO";

        //        res.FuelReportDetails.Add(hFoFuelReportCommandDetailDto);

        //    }


        //    if (rpmInfo.ROBDO != null)
        //    {
        //        //MDO
        //        var hFoFuelReportCommandDetailDto = new FuelReportCommandDetailDto();
        //        hFoFuelReportCommandDetailDto.Consumption = Convert.ToDecimal(rpmInfo.ConsInPortDO) + Convert.ToDecimal(rpmInfo.ConsAtSeaDO);
        //        hFoFuelReportCommandDetailDto.Transfer = rpmInfo.TransferDo;

        //        hFoFuelReportCommandDetailDto.ROB = Convert.ToDecimal(rpmInfo.ROBDO);

        //        hFoFuelReportCommandDetailDto.Receive = Convert.ToDecimal(rpmInfo.ReceivedDO);

        //        hFoFuelReportCommandDetailDto.Correction = Convert.ToDecimal(rpmInfo.CorrectionDo);
        //        hFoFuelReportCommandDetailDto.CorrectionType =
        //            (rpmInfo.CorrectionTypeHo != null && rpmInfo.CorrectionTypeDo.Value) ? CorrectionTypeEnum.Plus : CorrectionTypeEnum.Minus;

        //        hFoFuelReportCommandDetailDto.MeasuringUnitCode = "TON";
        //        hFoFuelReportCommandDetailDto.FuelType = "MDO";

        //        res.FuelReportDetails.Add(hFoFuelReportCommandDetailDto);

        //    }

        //    if (rpmInfo.ROBMGO != null)
        //    {
        //        //MGO
        //        var hFoFuelReportCommandDetailDto = new FuelReportCommandDetailDto();
        //        hFoFuelReportCommandDetailDto.Consumption = Convert.ToDecimal(rpmInfo.ConsInPortMGO) + Convert.ToDecimal(rpmInfo.ConsAtSeaMGO);
        //        hFoFuelReportCommandDetailDto.Transfer = rpmInfo.TransferMGOLS;

        //        hFoFuelReportCommandDetailDto.ROB = Convert.ToDecimal(rpmInfo.ROBMGO);

        //        hFoFuelReportCommandDetailDto.Receive = Convert.ToDecimal(rpmInfo.ReceivedMGO);

        //        hFoFuelReportCommandDetailDto.Correction = Convert.ToDecimal(rpmInfo.CorrectionMGOLS);
        //        hFoFuelReportCommandDetailDto.CorrectionType =
        //            (rpmInfo.CorrectionTypeHo != null && rpmInfo.CorrectionTypeMGOLS.Value) ? CorrectionTypeEnum.Plus : CorrectionTypeEnum.Minus;

        //        hFoFuelReportCommandDetailDto.MeasuringUnitCode = "TON";
        //        hFoFuelReportCommandDetailDto.FuelType = "MGO";

        //        res.FuelReportDetails.Add(hFoFuelReportCommandDetailDto);

        //    }


        //    return res;

        //}

        public static FuelReportCommandDto Map(Data.EventReport eventReport)
        {
            FuelReportCommandDto res = new FuelReportCommandDto();
            res.FuelReportDetails = new List<FuelReportCommandDetailDto>();
            res.VesselReportReference = eventReport.ID.ToString();

            res.VesselCode = eventReport.ShipCode.PadLeft(4, '0');

            //res.FuelReportType = GetFuelReportTypeEnum(eventReport.FuelReportType);
            res.FuelReportType = GetFuelReportTypeEnum(eventReport.FuelReportType);

            res.EventDate = new DateTime(eventReport.Year.Value, eventReport.Month.Value, eventReport.Day.Value);

            //res.EventDate.Add(eventReport.Time);
            res.EventDate = res.EventDate.Add(eventReport.Time.HasValue ? eventReport.Time.Value : new TimeSpan(12, 0, 0));

            res.ReportDate = DateTime.Now;


            res.VoyageNumber = eventReport.VoyageNo;

            res.Remark = eventReport.VoyageNo + " / " + eventReport.PortName;

            res.IsActive = true;


            //---------------HFO---------------
            var hFoFuelReportCommandDetailDto = new FuelReportCommandDetailDto();
            hFoFuelReportCommandDetailDto.ROB = Convert.ToDecimal(eventReport.ROBHO.GetValueOrDefault(0));
            hFoFuelReportCommandDetailDto.Consumption = Convert.ToDecimal(eventReport.ConsInPortHO.GetValueOrDefault(0)) + Convert.ToDecimal(eventReport.ConsAtSeaHO.GetValueOrDefault(0));
            hFoFuelReportCommandDetailDto.Transfer = eventReport.TransferHo.GetValueOrDefault(0);
            hFoFuelReportCommandDetailDto.Receive = Convert.ToDecimal(eventReport.ReceivedHO.GetValueOrDefault(0));
            hFoFuelReportCommandDetailDto.Correction = Convert.ToDecimal(eventReport.CorrectionHo.GetValueOrDefault(0));
            hFoFuelReportCommandDetailDto.CorrectionType =
                (!string.IsNullOrWhiteSpace(eventReport.CorrectionTypeHo) && eventReport.CorrectionTypeHo == "+") ? CorrectionTypeEnum.Plus : CorrectionTypeEnum.Minus;

            hFoFuelReportCommandDetailDto.MeasuringUnitCode = "TON";
            hFoFuelReportCommandDetailDto.FuelType = "HFO";

            res.FuelReportDetails.Add(hFoFuelReportCommandDetailDto);



            //---------------MDO---------------
            var mdoFuelReportCommandDetailDto = new FuelReportCommandDetailDto();
            mdoFuelReportCommandDetailDto.ROB = Convert.ToDecimal(eventReport.ROBDO.GetValueOrDefault(0));
            mdoFuelReportCommandDetailDto.Consumption = Convert.ToDecimal(eventReport.ConsInPortDO.GetValueOrDefault(0)) + Convert.ToDecimal(eventReport.ConsAtSeaDO.GetValueOrDefault(0));
            mdoFuelReportCommandDetailDto.Transfer = eventReport.TransferDo.GetValueOrDefault(0);
            mdoFuelReportCommandDetailDto.Receive = Convert.ToDecimal(eventReport.ReceivedDO.GetValueOrDefault(0));
            mdoFuelReportCommandDetailDto.Correction = Convert.ToDecimal(eventReport.CorrectionDo.GetValueOrDefault(0));
            mdoFuelReportCommandDetailDto.CorrectionType =
                (!string.IsNullOrWhiteSpace(eventReport.CorrectionTypeDo) && eventReport.CorrectionTypeDo == "+") ? CorrectionTypeEnum.Plus : CorrectionTypeEnum.Minus;

            mdoFuelReportCommandDetailDto.MeasuringUnitCode = "TON";
            mdoFuelReportCommandDetailDto.FuelType = "MDO";

            res.FuelReportDetails.Add(mdoFuelReportCommandDetailDto);


            //---------------MGO---------------
            var mgoFuelReportCommandDetailDto = new FuelReportCommandDetailDto();
            mgoFuelReportCommandDetailDto.ROB = Convert.ToDecimal(eventReport.ROBMGO.GetValueOrDefault(0));
            mgoFuelReportCommandDetailDto.Consumption = Convert.ToDecimal(eventReport.ConsInPortMGO.GetValueOrDefault(0)) + Convert.ToDecimal(eventReport.ConsAtSeaMGO.GetValueOrDefault(0));
            mgoFuelReportCommandDetailDto.Transfer = eventReport.TransferMGOLS.GetValueOrDefault(0);
            mgoFuelReportCommandDetailDto.Receive = Convert.ToDecimal(eventReport.ReceivedMGO.GetValueOrDefault(0));
            mgoFuelReportCommandDetailDto.Correction = Convert.ToDecimal(eventReport.CorrectionMGOLS.GetValueOrDefault(0));
            mgoFuelReportCommandDetailDto.CorrectionType =
                (!string.IsNullOrWhiteSpace(eventReport.CorrectionTypeMGOLS) && eventReport.CorrectionTypeMGOLS == "+") ? CorrectionTypeEnum.Plus : CorrectionTypeEnum.Minus;

            mgoFuelReportCommandDetailDto.MeasuringUnitCode = "TON";
            mgoFuelReportCommandDetailDto.FuelType = "MGO";

            res.FuelReportDetails.Add(mgoFuelReportCommandDetailDto);


            //---------------IFO---------------
            var ifoFuelReportCommandDetailDto = new FuelReportCommandDetailDto();
            ifoFuelReportCommandDetailDto.ROB = Convert.ToDecimal(eventReport.ROBatNoonIfo.GetValueOrDefault(0));
            ifoFuelReportCommandDetailDto.Consumption = Convert.ToDecimal(eventReport.ConsumpInPortIfo.GetValueOrDefault(0)) + Convert.ToDecimal(eventReport.ConsumpAtSeaIfo.GetValueOrDefault(0));
            ifoFuelReportCommandDetailDto.Transfer = eventReport.TransferIfo.GetValueOrDefault(0);
            ifoFuelReportCommandDetailDto.Receive = Convert.ToDecimal(eventReport.RecivedAtPortIFO.GetValueOrDefault(0));
            ifoFuelReportCommandDetailDto.Correction = Convert.ToDecimal(eventReport.CorrectionIfo.GetValueOrDefault(0));
            ifoFuelReportCommandDetailDto.CorrectionType =
                (!string.IsNullOrWhiteSpace(eventReport.CorrectionTypeIfo) && eventReport.CorrectionTypeIfo == "+") ? CorrectionTypeEnum.Plus : CorrectionTypeEnum.Minus;

            ifoFuelReportCommandDetailDto.MeasuringUnitCode = "TON";
            ifoFuelReportCommandDetailDto.FuelType = "IFO";

            res.FuelReportDetails.Add(ifoFuelReportCommandDetailDto);


            //---------------HO LS---------------
            var hoLSFuelReportCommandDetailDto = new FuelReportCommandDetailDto();
            hoLSFuelReportCommandDetailDto.ROB = Convert.ToDecimal(eventReport.ROBatNoonHoLs.GetValueOrDefault(0));
            hoLSFuelReportCommandDetailDto.Consumption = Convert.ToDecimal(eventReport.ConsumpInPortHoLs.GetValueOrDefault(0)) + Convert.ToDecimal(eventReport.ConsumpAtSeaHoLs.GetValueOrDefault(0));
            hoLSFuelReportCommandDetailDto.Transfer = eventReport.TransferHoLs.GetValueOrDefault(0);
            hoLSFuelReportCommandDetailDto.Receive = Convert.ToDecimal(eventReport.RecivedAtPortHoLs.GetValueOrDefault(0));
            hoLSFuelReportCommandDetailDto.Correction = Convert.ToDecimal(eventReport.CorrectionHoLs.GetValueOrDefault(0));
            hoLSFuelReportCommandDetailDto.CorrectionType =
                (!string.IsNullOrWhiteSpace(eventReport.CorrectionTypeHoLs) && eventReport.CorrectionTypeHoLs == "+") ? CorrectionTypeEnum.Plus : CorrectionTypeEnum.Minus;

            hoLSFuelReportCommandDetailDto.MeasuringUnitCode = "TON";
            hoLSFuelReportCommandDetailDto.FuelType = "HOLS";

            res.FuelReportDetails.Add(hoLSFuelReportCommandDetailDto);

            //---------------DO LS---------------
            var doLSFuelReportCommandDetailDto = new FuelReportCommandDetailDto();
            doLSFuelReportCommandDetailDto.ROB = Convert.ToDecimal(eventReport.ROBatNoonDoLs.GetValueOrDefault(0));
            doLSFuelReportCommandDetailDto.Consumption = Convert.ToDecimal(eventReport.ConsumpInPortDoLs.GetValueOrDefault(0)) + Convert.ToDecimal(eventReport.ConsumpAtSeaDoLs.GetValueOrDefault(0));
            doLSFuelReportCommandDetailDto.Transfer = eventReport.TransferDoLs.GetValueOrDefault(0);
            doLSFuelReportCommandDetailDto.Receive = Convert.ToDecimal(eventReport.RecivedAtPortDoLs.GetValueOrDefault(0));
            doLSFuelReportCommandDetailDto.Correction = Convert.ToDecimal(eventReport.CorrectionDoLs.GetValueOrDefault(0));
            doLSFuelReportCommandDetailDto.CorrectionType =
                (!string.IsNullOrWhiteSpace(eventReport.CorrectionTypeDoLs) && eventReport.CorrectionTypeDoLs == "+") ? CorrectionTypeEnum.Plus : CorrectionTypeEnum.Minus;

            doLSFuelReportCommandDetailDto.MeasuringUnitCode = "TON";
            doLSFuelReportCommandDetailDto.FuelType = "DOLS";

            res.FuelReportDetails.Add(doLSFuelReportCommandDetailDto);

            //---------------IFO LS---------------
            var ifoLSFuelReportCommandDetailDto = new FuelReportCommandDetailDto();
            ifoLSFuelReportCommandDetailDto.ROB = Convert.ToDecimal(eventReport.ROBatNoonIfoLs.GetValueOrDefault(0));
            ifoLSFuelReportCommandDetailDto.Consumption = Convert.ToDecimal(eventReport.ConsumpInPortIfoLs.GetValueOrDefault(0)) + Convert.ToDecimal(eventReport.ConsumpAtSeaIfoLs.GetValueOrDefault(0));
            ifoLSFuelReportCommandDetailDto.Transfer = eventReport.TransferIfoLs.GetValueOrDefault(0);
            ifoLSFuelReportCommandDetailDto.Receive = Convert.ToDecimal(eventReport.RecivedAtPortIfoLs.GetValueOrDefault(0));
            ifoLSFuelReportCommandDetailDto.Correction = Convert.ToDecimal(eventReport.CorrectionIfoLs.GetValueOrDefault(0));
            ifoLSFuelReportCommandDetailDto.CorrectionType =
                (!string.IsNullOrWhiteSpace(eventReport.CorrectionTypeIfoLs) && eventReport.CorrectionTypeIfoLs == "+") ? CorrectionTypeEnum.Plus : CorrectionTypeEnum.Minus;

            ifoLSFuelReportCommandDetailDto.MeasuringUnitCode = "TON";
            ifoLSFuelReportCommandDetailDto.FuelType = "IFOLS";

            res.FuelReportDetails.Add(ifoLSFuelReportCommandDetailDto);

            return res;
        }

        public static FuelReportTypeEnum GetFuelReportTypeEnum(byte? fuelReportType)
        {
            //switch (fuelReportType)
            //{
            //    case 1:
            //        return FuelReportTypeEnum.NoonReport;
            //    case 2:
            //        return FuelReportTypeEnum.EndOfVoyage;
            //    case 3:
            //        return FuelReportTypeEnum.ArrivalReport;
            //    case 4:
            //        return FuelReportTypeEnum.DepartureReport;
            //    case 5:
            //        return FuelReportTypeEnum.EndOfYear;
            //    case 6:
            //        return FuelReportTypeEnum.EndOfMonth;
            //    case 7:
            //        return FuelReportTypeEnum.CharterInEnd;
            //    case 8:
            //        return FuelReportTypeEnum.CharterOutStart;
            //    case 9:
            //        return FuelReportTypeEnum.DryDock;
            //    case 10:
            //        return FuelReportTypeEnum.BeginOfOffHire;
            //    case 11:
            //        return FuelReportTypeEnum.BeginOfLayUp;
            //    case 12:
            //        return FuelReportTypeEnum.EndOfOffhire;
            //    case 13:
            //        return FuelReportTypeEnum.BeginOfPassage;
            //    case 14:
            //        return FuelReportTypeEnum.EndOfPassage;
            //    case 15:
            //        return FuelReportTypeEnum.Bunkering;
            //    case 16:
            //        return FuelReportTypeEnum.Debunkering;
            //    default:
            //        return FuelReportTypeEnum.None;
            //}

            if (fuelReportType.HasValue)
                return (FuelReportTypeEnum) fuelReportType.Value;
            
            return FuelReportTypeEnum.None;
        }
    }
}