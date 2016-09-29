using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates
{
    public abstract class VesselInCompanyState
    {
        public VesselStates State { get; private set; }

        protected readonly IVesselInCompanyStateFactory VesselInCompanyStateFactory;

        protected VesselInCompanyState(IVesselInCompanyStateFactory vesselInCompanyStateFactory, VesselStates state)
        {
            this.VesselInCompanyStateFactory = vesselInCompanyStateFactory;
            this.State = state;
        }

        public virtual void Activate(VesselInCompany vesselInCompany)
        {
            throw new InvalidStateException("Activate", string.Format("Cannot activate the vessel as it is in '{0}' state.", this.State));
        }
        public virtual void Deactivate(VesselInCompany vesselInCompany)
        {
            throw new InvalidStateException("Deactivate", string.Format("Cannot deactivate the vessel as it is in '{0}' state.", this.State));
        }
        public virtual void Scrap(VesselInCompany vesselInCompany)
        {
            throw new InvalidStateException("Scrap", string.Format("Cannot scrap the vessel as it is in '{0}' state.", this.State));
        }
        public virtual void StartCharterOut(VesselInCompany vesselInCompany)
        {
            throw new InvalidStateException("StartCharterOut", string.Format("Cannot start the Charter Out of the vessel as it is in '{0}' state.", this.State));
        }
        public virtual void EndCharterOut(VesselInCompany vesselInCompany)
        {
            throw new InvalidStateException("EndCharterOut", string.Format("Cannot end the Charter Out of the vessel as it is in '{0}' state.", this.State));
        }
        public virtual void StartCharterIn(VesselInCompany vesselInCompany)
        {
            throw new InvalidStateException("StartCharterIn", string.Format("Cannot start the Charter In of the vessel as it is in '{0}' state.", this.State));
        }
        public virtual void EndCharterIn(VesselInCompany vesselInCompany)
        {
            throw new InvalidStateException("EndCharterIn", string.Format("Cannot end the Charter In of the vessel as it is in '{0}' state.", this.State));
        }
    }
}