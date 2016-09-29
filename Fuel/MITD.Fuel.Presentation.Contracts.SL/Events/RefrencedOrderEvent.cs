using System;
using System.Collections.ObjectModel;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.Events
{
    public class RefrencedOrderEvent
    {
        public ObservableCollection<OrderDto> ReferencedOrders { get; set; }
        public Guid UniqId { get; set; }
        public bool  Changed { get; set; }
    }
}