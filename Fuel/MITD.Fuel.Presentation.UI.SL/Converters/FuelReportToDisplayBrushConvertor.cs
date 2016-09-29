using System;
using global::System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;

namespace MITD.Fuel.Presentation.UI.SL.Converters
{
    public class FuelReportToDisplayBrushConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fuelReport = value as FuelReportDto;
            if (fuelReport != null)
            {
                var pCal = new PersianCalendar();

                if((fuelReport.FuelReportType == FuelReportTypeEnum.EndOfVoyage ||
                    fuelReport.FuelReportType == FuelReportTypeEnum.BunkeringCompleted ||
                    fuelReport.FuelReportType == FuelReportTypeEnum.DebunkeringCompleted ) ||
                    (fuelReport.FuelReportType == FuelReportTypeEnum.NoonReport &&   //Detecting End of Year record
                        (
                            (
                                pCal.GetMonth(fuelReport.EventDate) == 1 &&
                                pCal.GetDayOfMonth(fuelReport.EventDate) == 1 &&
                                fuelReport.EventDate.Year > 2016
                            ) || 
                            (
                                fuelReport.EventDate.Year <= 2016 &&
                                fuelReport.EventDate.Month == 3 &&
                                fuelReport.EventDate.Day == 21
                            )
                        ) &&
                        fuelReport.EventDate.TimeOfDay == new TimeSpan(12, 0, 0)
                    ) ||
                    fuelReport.FuelReportDetail.Any(frd=>frd.Transfer.HasValue || frd.Recieve.HasValue || frd.Correction.HasValue))
                {
                    return Colors.Orange;
                }
            }

            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
