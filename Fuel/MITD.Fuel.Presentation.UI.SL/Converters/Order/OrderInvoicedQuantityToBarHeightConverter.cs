using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.UI.SL.Converters.Order
{
    public class OrderInvoicedQuantityToBarHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is OrderDto)) return 0;

            var order = value as OrderDto;

            if (order.OrderItems == null || order.OrderItems.Count == 0) return 0;

            var totalOperatedQuantity = order.OrderItems.Sum(i => i.OperatedQuantityInMainUnit);
            var totalInvoicedQuantity = order.OrderItems.Sum(i => i.InvoicedInMainUnit);
            var calculatedHeightPercentage = totalOperatedQuantity == 0 ? 0 : Math.Floor((double)(totalInvoicedQuantity / totalOperatedQuantity) * 10) / 10; //This line, ensures the height will be fully filled if the whole Received Quantity is invoiced. It leaves a gap in UI for which it is not fully invoiced.

            //calculatedHeightPercentage = calculatedHeightPercentage == 0 && totalInvoicedQuantity != 0 ? 0.05 : calculatedHeightPercentage;

            return Math.Min(calculatedHeightPercentage * 20, 20);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
