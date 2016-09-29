using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class ScrapDto : DTOBase
    {
        public ScrapDto()
        {
        }

        long id;
        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((object.ReferenceEquals(this.Id, value) != true)) {this.id= value;}}
        }

        private DateTime scrapDate;
        [DataMember]
        public DateTime ScrapDate
        {
            get { return this.scrapDate; }
            set { if ((object.ReferenceEquals(this.ScrapDate, value) != true)) {this.scrapDate= value;}}
        }


        private ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VesselDto vessel;
        [DataMember]
        public ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VesselDto Vessel
        {
            get { return this.vessel; }
            set { if ((object.ReferenceEquals(this.Vessel, value) != true)) {this.vessel= value;}}
        }

        private CompanyDto secondParty;
        [DataMember]
        public CompanyDto SecondParty
        {
            get { return this.secondParty; }
            set { if ((object.ReferenceEquals(this.SecondParty, value) != true)) {this.secondParty= value;}}
        }

        private ObservableCollection<ScrapDetailDto> scrapDetails;
        [DataMember]
        public ObservableCollection<ScrapDetailDto> ScrapDetails
        {
            get { return this.scrapDetails; }
            set { if ((object.ReferenceEquals(this.ScrapDetails, value) != true)) {this.scrapDetails= value;}}
        }

        private ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.UserDto userInCharge;
        [DataMember]
        public ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.UserDto UserInCharge
        {
            get { return this.userInCharge; }
            set { if ((object.ReferenceEquals(this.UserInCharge, value) != true)) {this.userInCharge= value;}}
        }

        private string currentState;
        [DataMember]
        public string CurrentState
        {
            get { return this.currentState; }
            set { if ((object.ReferenceEquals(this.CurrentState, value) != true)) {this.currentState= value;}}
        }


        private bool isScrapEditPermitted;
        [DataMember]
        public bool IsScrapEditPermitted
        {
            get { return this.isScrapEditPermitted; }
            set { if ((object.ReferenceEquals(this.IsScrapEditPermitted, value) != true)) {this.isScrapEditPermitted= value;}}
        }

        private bool isScrapDeletePermitted;
        [DataMember]
        public bool IsScrapDeletePermitted
        {
            get { return this.isScrapDeletePermitted; }
            set { if ((object.ReferenceEquals(this.IsScrapDeletePermitted, value) != true)) {this.isScrapDeletePermitted= value;}}
        }



        private bool isScrapAddDetailPermitted;
        [DataMember]
        public bool IsScrapAddDetailPermitted
        {
            get { return this.isScrapAddDetailPermitted; }
            set { if ((object.ReferenceEquals(this.IsScrapAddDetailPermitted, value) != true)) {this.isScrapAddDetailPermitted= value;}}
        }

        private bool isScrapEditDetailPermitted;
        [DataMember]
        public bool IsScrapEditDetailPermitted
        {
            get { return this.isScrapEditDetailPermitted; }
            set { if ((object.ReferenceEquals(this.IsScrapEditDetailPermitted, value) != true)) {this.isScrapEditDetailPermitted= value;}}
        }


        private bool isScrapDeleteDetailPermitted;
        [DataMember]
        public bool IsScrapDeleteDetailPermitted
        {
            get { return this.isScrapDeleteDetailPermitted; }
            set { if ((object.ReferenceEquals(this.IsScrapDeleteDetailPermitted, value) != true)) {this.isScrapDeleteDetailPermitted= value;}}
        }

        private bool isGoodEditable;
        [DataMember]
        public bool IsGoodEditable
        {
            get { return this.isGoodEditable; }
            set { if ((object.ReferenceEquals(this.IsGoodEditable, value) != true)) {this.isGoodEditable= value;}}
        }

        private bool isTankEditable;
        [DataMember]
        public bool IsTankEditable
        {
            get { return this.isTankEditable; }
            set { if ((object.ReferenceEquals(this.IsTankEditable, value) != true)) {this.isTankEditable= value;}}
        }
    }
}
