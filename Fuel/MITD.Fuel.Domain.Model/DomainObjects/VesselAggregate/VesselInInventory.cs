#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security;
using MITD.Fuel.Domain.Model.Enums;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    /// <summary>
    /// There are no comments for MITD.Fuel.Domain.Model.DomainObjects.Vessel in the schema.
    /// </summary>
    public class VesselInInventory
    {
        #region Properties

        public long Id { get; private set; }

        public string Code { get; private set; }

        public long CompanyId { get; private set; }

        public bool IsActive { get; private set; }

        public virtual Company Company { get; private set; }

        public virtual List<Tank> Tanks { get; private set; }

        #endregion

        public VesselInInventory()
        {
            //Tanks = new List<Tank>();
        }
    }
}