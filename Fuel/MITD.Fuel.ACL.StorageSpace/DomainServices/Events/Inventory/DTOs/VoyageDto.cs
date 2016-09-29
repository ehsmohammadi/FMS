using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{

    [DataContract]
    public partial class VoyageDto : DTOBase
    {
        long id;
        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((object.ReferenceEquals(this.Id, value) != true)) {this.id= value;}}
        }

        string code;
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

        private DateTime startDate;

        [DataMember]
        public DateTime StartDate
        {
            get { return this.startDate; }
            set { if ((object.ReferenceEquals(this.StartDate, value) != true)) {this.startDate= value;}}
        }

        private DateTime endDate;
        [DataMember]
        public DateTime EndDate
        {
            get { return this.endDate; }
            set { if ((object.ReferenceEquals(this.EndDate, value) != true)) {this.endDate= value;}}
        }

        private bool isActive;
        [DataMember]
        public bool IsActive
        {
            get { return this.isActive; }
            set { if ((object.ReferenceEquals(this.IsActive, value) != true)) {this.isActive= value;}}
        }

        private FuelReportDto endOfVoyageFuelReport;
        [DataMember]
        public FuelReportDto EndOfVoyageFuelReport
        {
            get { return this.endOfVoyageFuelReport; }
            set { if ((object.ReferenceEquals(this.EndOfVoyageFuelReport, value) != true)) {this.endOfVoyageFuelReport= value;}}
        }

        private ObservableCollection<FuelReportInventoryOperationDto> endOfVoyageInventoryOperations;
        [DataMember]
        public ObservableCollection<FuelReportInventoryOperationDto> EndOfVoyageInventoryOperations
        {
            get { return this.endOfVoyageInventoryOperations; }
            set { if ((object.ReferenceEquals(this.EndOfVoyageInventoryOperations, value) != true)) {this.endOfVoyageInventoryOperations= value;}}
        }
    }
}
