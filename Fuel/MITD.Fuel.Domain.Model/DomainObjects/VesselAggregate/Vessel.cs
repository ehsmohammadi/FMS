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
    public class Vessel
    {
        #region Properties

        public long Id { get; private set; }

        public string Code { get; private set; }

        public long OwnerId { get; private set; }

        public virtual Company Owner { get; private set; }

        public virtual List<VesselInCompany> VesselOperationStates { get; private set; }

        public byte[] RowVersion { get; set; }

        #endregion

        public Vessel()
        {

        }

        public Vessel(string code, long ownerId)
        {
            //Id = id;
            Code = code;
            OwnerId = ownerId;
        }
    }
}