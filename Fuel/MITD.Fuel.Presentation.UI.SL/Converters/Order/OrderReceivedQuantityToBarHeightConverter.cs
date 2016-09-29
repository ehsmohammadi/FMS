using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.UI.SL.Converters.Order
{
    public class OrderReceivedQuantityToBarHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is OrderDto)) return 0;

            var order = value as OrderDto;

            if (order.OrderItems == null || order.OrderItems.Count == 0) return 0;

            var totalOperatedQuantity = order.OrderItems.Sum(i => i.OperatedQuantityInMainUnit);
            var totalOrderQuantity = order.OrderItems.Sum(i => i.Quantity);
            var calculatedHeightPercentage = totalOrderQuantity == 0 ? 0 : Math.Floor((double)(totalOperatedQuantity / totalOrderQuantity) * 10) / 10; //This line, ensures the height will be fully filled if the whole Order Quantity is received. It leaves a gap in UI for which it is not fully received.

            //calculatedHeightPercentage = calculatedHeightPercentage == 0 && totalReceivedQuantity != 0 ? 0.05 : calculatedHeightPercentage;

            return Math.Min(calculatedHeightPercentage * 20, 20);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
