using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;

namespace MITD.Fuel.Presentation.Logic.SL.Converters
{
    public class OrderAssignementReferencesIdsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            OrderAssignementReferenceTypeEnum referenceType;
            if (!(value is OrderDto) || !Enum.TryParse(parameter.ToString(), true, out referenceType)) return string.Empty;

            var order = (OrderDto) value;

            if (referenceType == OrderAssignementReferenceTypeEnum.FuelReportDetail && order.OrderType == OrderTypeEnum.SupplyForDeliveredVessel)
                return "N/A";

            return string.Join(",", order.DestinationReferences.Where(i => i.DestinationType == referenceType).Select(i => i.DestinationId));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
