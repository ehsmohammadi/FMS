#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts;
#endregion

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class OrderAssignmentReferenceDto
    {
        private long destinationId;
        public long DestinationId
        {
            get { return destinationId; }
            set { this.SetField(p => p.DestinationId, ref destinationId, value); }
        }

        private OrderAssignementReferenceTypeEnum destinationType;
        public OrderAssignementReferenceTypeEnum DestinationType
        {
            get { return destinationType; }
            set { this.SetField(p => p.DestinationType, ref destinationType, value); }
        }
    }
}