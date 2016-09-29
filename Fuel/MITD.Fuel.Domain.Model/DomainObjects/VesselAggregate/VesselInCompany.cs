#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    /// <summary>
    /// There are no comments for MITD.Fuel.Domain.Model.DomainObjects.Vessel in the schema.
    /// </summary>
    public class VesselInCompany
    {
        #region Properties

        public long Id { get; private set; }

       // public long TestProperty { get; set; }

        public string Code
        {
            get { return Vessel.Code; }
        }

        //If Vessel Change Name should be implemented, then a mechanism must be designed for Vouvher / Inventory routing.
        public string Name { get; private set; }

        public string Description { get; private set; }

        public long CompanyId { get; private set; }

        public long VesselId { get; private set; }

        public bool IsActive { get; private set; }

        public VesselStates VesselStateCode { get; private set; }

        public VesselInCompanyState VesselInCompanyState { get; private set; }

        public virtual Company Company { get; private set; }

        public virtual Vessel Vessel { get; private set; }

        public List<Tank> Tanks
        {
            get
            {
                if (VesselInInventory == null) return null;

                return VesselInInventory.Tanks;
            }
        }

        public VesselInInventory VesselInInventory { get; private set; }

        public byte[] RowVersion { get; private set; }

        #endregion

        public VesselInCompany()
        {
        }

        public VesselInCompany(string name, string description,
            long companyId, long vesselId, VesselStates vesselStateCode, bool isActive)
        {
            //Id = id;
            //Code = code;
            Name = name;
            Description = description;
            CompanyId = companyId;
            this.VesselStateCode = vesselStateCode;
            IsActive = isActive;
            VesselId = vesselId;
        }

        public VesselInCompany(string name, string description,
            long companyId, Vessel vessel, VesselStates vesselStateCode, bool isActive)
            :this(name,description,companyId,0, vesselStateCode,isActive)
        {
            this.Vessel = vessel;
        }

        internal void Configure(VesselInInventory vesselInInventory, IVesselInCompanyStateFactory vesselInCompanyStateFactory)
        {
            this.VesselInInventory = vesselInInventory;

            this.VesselInCompanyState = vesselInCompanyStateFactory.CreatState(this.VesselStateCode);
        }

        public void SetState(VesselInCompanyState vesselInCompanyState)
        {
            this.VesselStateCode = vesselInCompanyState.State;

            this.VesselInCompanyState = vesselInCompanyState;
        }

        public void Scrap()
        {
            this.VesselInCompanyState.Scrap(this);

        }
        public void Activate()
        {
            this.VesselInCompanyState.Activate(this);

        }
        public void Deactivate()
        {
            this.VesselInCompanyState.Deactivate(this);

        }
        public void StartCharterIn()
        {
            this.VesselInCompanyState.StartCharterIn(this);

        }
        public void EndCharterIn()
        {
            this.VesselInCompanyState.EndCharterIn(this);

        }
        public void StartCharterOut()
        {
            this.VesselInCompanyState.StartCharterOut(this);

        }

        public void EndCharterOut()
        {
            this.VesselInCompanyState.EndCharterOut(this);

        }
    }
}