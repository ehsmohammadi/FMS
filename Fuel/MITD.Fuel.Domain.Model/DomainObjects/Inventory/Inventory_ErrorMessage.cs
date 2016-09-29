using System;
namespace MITD.Fuel.Domain.Model.DomainObjects
{
    [Serializable]
    public partial class Inventory_ErrorMessage
    {
        public string ErrorMessage { get; set; } // ErrorMessage (Primary key)
        public string TextMessage { get; set; } // TextMessage (Primary key)
        public string Action { get; set; } // Action (Primary key)
    }
}