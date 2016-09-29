#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class Tank
    {

        public long Id { get; private set; }

        public string Name { get; private set; }

        public long VesselInInventoryId { get; set; }

        public virtual VesselInInventory VesselInInventory { get; set; }

        public Tank()
        {
        }

        public Tank(long id, string name, long vesselInInventoryId)
        {
            Id = id;
            Name = name;
            VesselInInventoryId = vesselInInventoryId;
        }

    }
}