using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using MITD.Core;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;


namespace MITD.Fuel.Domain.Model.DomainObjects
{
    /// <summary>
    /// There are no comments for MITD.Fuel.Domain.Model.DomainObjects.Voyage in the schema.
    /// </summary>
    public class Voyage
    {
        public Voyage()
        {
        }

        public Voyage(
            long id,
            string voyageNumber,
            string description,
            long vesselInCompanyId,
            long companyId,
            DateTime startDate,
            DateTime? endDate,
            bool isActive = true
            )
        {
            Id = id;
            this.VoyageNumber = voyageNumber;
            Description = description;
            VesselInCompanyId = vesselInCompanyId;
            CompanyId = companyId;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
        }

        public long Id { get; set; }
        public string VoyageNumber { get; set; }
        public string Description { get; set; }
        public long VesselInCompanyId { get; set; }
        public long CompanyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; private set; }
        public bool IsLocked { get; private set; }

        public virtual VesselInCompany VesselInCompany { get; set; }
        public virtual Company Company { get; set; }

        public void CancelVoyage()
        {
            this.IsActive = false;
        }

        public void Lock()
        {
            this.IsLocked = true;
        }

        public void Unlock()
        {
            this.IsLocked = false;

            var voyageDomainServie = ServiceLocator.Current.GetInstance<IVoyageDomainService>();

            voyageDomainServie.UpdateVoyageFromRotationData(this);
        }

        public void Update(RotationVoyage rotationVoyage)
        {
            if(IsLocked) throw new BusinessRuleException("", "Selected voyage is locked.");

            if(this.VoyageNumber != rotationVoyage.VoyageNumber) throw new BusinessRuleException("", "Given voyage has different voyage number than updating voyage.");

            this.IsActive = rotationVoyage.IsActive;
            this.StartDate = rotationVoyage.StartDate;
            this.EndDate = rotationVoyage.EndDate;

            this.Description = rotationVoyage.Description;
            this.CompanyId = rotationVoyage.CompanyId;
            this.VesselInCompanyId = rotationVoyage.VesselInCompanyId;
        }
    }
}