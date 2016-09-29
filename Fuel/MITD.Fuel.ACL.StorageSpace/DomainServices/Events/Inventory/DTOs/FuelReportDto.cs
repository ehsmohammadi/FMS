using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;
using FuelReportTypeEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.FuelReportTypeEnum;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class FuelReportDto : DTOBase
    {
        public FuelReportDto()
        {
        }

        long id;
        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((object.ReferenceEquals(this.Id, value) != true)) {this.id= value;}}
        }

        string code;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "code can't be empty")]
        [DataMember]
        public string Code
        {
            get { return this.code; }
            set { if ((object.ReferenceEquals(this.Code, value) != true)) {this.code= value;}}
        }

        string _description;
        [DataMember]
        public string Description
        {
            get { return this._description; }
            set { if ((object.ReferenceEquals(this.Description, value) != true)) {this._description= value;}}
        }



        string _CurrentStateName;
        [DataMember]
        public string CurrentStateName
        {
            get { return this._CurrentStateName; }
            set { if ((object.ReferenceEquals(this.CurrentStateName, value) != true)) {this._CurrentStateName= value;}}
        }
        string _UserInChargName;
        [DataMember]
        public string UserInChargName
        {
            get { return this._UserInChargName; }
            set { if ((object.ReferenceEquals(this.UserInChargName, value) != true)) {this._UserInChargName= value;}}
        }

        FuelReportTypeEnum _fuelReportType;
        [DataMember]
        public FuelReportTypeEnum FuelReportType
        {
            get { return this._fuelReportType; }
            set { if ((object.ReferenceEquals(this.FuelReportType, value) != true)) {this._fuelReportType= value;}}
        }

        DateTime _reportDate;
        [DataMember]
        public DateTime ReportDate
        {
            get { return this._reportDate; }
            set { if ((object.ReferenceEquals(this.ReportDate, value) != true)) {this._reportDate= value;}}
        }

        DateTime _eventDate;
        [DataMember]
        public DateTime EventDate
        {
            get { return this._eventDate; }
            set { if ((object.ReferenceEquals(this.EventDate, value) != true)) {this._eventDate= value;}}
        }

        ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VoyageDto _voyage;
        [DataMember]
        public ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VoyageDto Voyage
        {
            get { return this._voyage; }
            set { if ((object.ReferenceEquals(this.Voyage, value) != true)) {this._voyage= value;}}
        }

        private bool _isNextActionFinalApprove;


        [DataMember]
        public bool IsNextActionFinalApprove
        {
            get { return this._isNextActionFinalApprove; }
            set { if ((object.ReferenceEquals(this.IsNextActionFinalApprove, value) != true)) {this._isNextActionFinalApprove= value;}}
        }


        private bool _isActive;


        [DataMember]
        public bool IsActive
        {
            get { return this._isActive; }
            set { if ((object.ReferenceEquals(this.IsActive, value) != true)) {this._isActive= value;}}
        }

        private ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VesselDto vesselDto;
        [DataMember]
        public ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VesselDto VesselDto
        {
            get { return this.vesselDto; }
            set { if ((object.ReferenceEquals(this.VesselDto, value) != true)) {this.vesselDto= value;}}
        }

        private ObservableCollection<FuelReportDetailDto> fuelReportDetail;
        [DataMember]
        public ObservableCollection<FuelReportDetailDto> FuelReportDetail
        {
            get { return this.fuelReportDetail; }
            set { if ((object.ReferenceEquals(this.FuelReportDetail, value) != true)) {this.fuelReportDetail= value;}}
        }
    }
}
