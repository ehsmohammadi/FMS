
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{

    public partial class ScrapDto
    {
        public ScrapDto()
        {
        }

        long id;

        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        private DateTime scrapDate;
        public DateTime ScrapDate
        {
            get { return this.scrapDate; }
            set { this.SetField(p => p.ScrapDate, ref this.scrapDate, value); }
        }


        private VesselInCompanyDto vesselInCompany;
        public VesselInCompanyDto VesselInCompany
        {
            get { return vesselInCompany; }
            set { this.SetField(p => p.VesselInCompany, ref vesselInCompany, value); }
        }

        private CompanyDto secondParty;
        public CompanyDto SecondParty
        {
            get { return secondParty; }
            set { this.SetField(p => p.SecondParty, ref secondParty, value); }
        }

        private ObservableCollection<ScrapDetailDto> scrapDetails;
        public ObservableCollection<ScrapDetailDto> ScrapDetails
        {
            get
            {
                return this.scrapDetails;
            }
            set { this.SetField(p => p.ScrapDetails, ref this.scrapDetails, value); }
        }

        private UserDto userInCharge;
        public UserDto UserInCharge
        {
            get { return userInCharge; }
            set { this.SetField(p => p.UserInCharge, ref userInCharge, value); }
        }

        private string currentState;
        public string CurrentState
        {
            get { return currentState; }
            set { this.SetField(p => p.CurrentState, ref currentState, value); }
        }


        private bool isScrapEditPermitted;
        public bool IsScrapEditPermitted
        {
            get { return isScrapEditPermitted; }
            set
            {
                this.SetField(p => p.IsScrapEditPermitted, ref isScrapEditPermitted, value); 
                
            }
        }

        private bool isScrapDeletePermitted;
        public bool IsScrapDeletePermitted
        {
            get { return isScrapDeletePermitted; }
            set { this.SetField(p => p.IsScrapDeletePermitted, ref isScrapDeletePermitted, value); }
        }



        private bool isScrapAddDetailPermitted;
        public bool IsScrapAddDetailPermitted
        {
            get { return this.isScrapAddDetailPermitted; }
            set { this.SetField(p => p.IsScrapAddDetailPermitted, ref this.isScrapAddDetailPermitted, value); }
        }

        private bool isScrapEditDetailPermitted;
        public bool IsScrapEditDetailPermitted
        {
            get { return this.isScrapEditDetailPermitted; }
            set { this.SetField(p => p.IsScrapEditDetailPermitted, ref this.isScrapEditDetailPermitted, value); }
        }


        private bool isScrapDeleteDetailPermitted;
        public bool IsScrapDeleteDetailPermitted
        {
            get { return this.isScrapDeleteDetailPermitted; }
            set { this.SetField(p => p.IsScrapDeleteDetailPermitted, ref this.isScrapDeleteDetailPermitted, value); }
        }

        private bool isGoodEditable;
        public bool IsGoodEditable
        {
            get { return isGoodEditable; }
            set { this.SetField(p => p.IsGoodEditable, ref isGoodEditable, value); }
        }

        private bool isTankEditable;
        public bool IsTankEditable
        {
            get { return isTankEditable; }
            set { this.SetField(p => p.IsTankEditable, ref isTankEditable, value); }
        }

        private WorkflowStageEnum approveStatus;
        public WorkflowStageEnum ApproveStatus
        {
            get { return approveStatus; }
            set { this.SetField(p => p.ApproveStatus, ref approveStatus, value); }
        }
    }
}
