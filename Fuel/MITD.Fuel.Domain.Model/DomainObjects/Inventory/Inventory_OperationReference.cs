using System;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    [Serializable]
    public partial class Inventory_OperationReference
    {
        public long Id { get; set; } // Id (Primary key)
        public long OperationId { get; set; } // OperationId
        public int OperationType { get; set; } // OperationType
        public string ReferenceType { get; set; } // ReferenceType
        public string ReferenceNumber { get; set; } // ReferenceNumber
        public DateTime RegistrationDate { get; set; } // RegistrationDate

        public Inventory_OperationReference()
        {
            RegistrationDate = System.DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }
}