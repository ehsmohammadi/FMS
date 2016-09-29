using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{

    public partial class VoyageLogDto
    {
        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        private DateTime changeDate;

        public DateTime ChangeDate
        {
            get { return changeDate; }
            set { this.SetField(p => p.StartDate, ref changeDate, value); }
        }

        string voyageNumber;
        [Required(AllowEmptyStrings = false, ErrorMessage = "VoyageNumber can't be empty")]
        public string VoyageNumber
        {
            get { return this.voyageNumber; }
            set { this.SetField(p => p.VoyageNumber, ref this.voyageNumber, value); }
        }

        string _description;
        public string Description
        {
            get { return _description; }
            set { this.SetField(p => p.Description, ref _description, value); }
        }

        private DateTime startDate;

        public DateTime StartDate
        {
            get { return startDate; }
            set { this.SetField(p => p.StartDate, ref startDate, value); }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set { this.SetField(p => p.EndDate, ref endDate, value); }
        }

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { this.SetField(p => p.IsActive, ref isActive, value); }
        }

        private CompanyDto company;
        public CompanyDto Company
        {
            get { return company; }
            set { this.SetField(p => p.Company, ref company, value); }
        }

        private VesselInCompanyDto vesselInCompany;

        public VesselInCompanyDto VesselInCompany
        {
            get { return vesselInCompany; }
            set { this.SetField(p => p.VesselInCompany, ref vesselInCompany, value); }
        }
    }
}
