using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{

    public partial class VoyageDto
    {
        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        string code;
        [Required(AllowEmptyStrings = false, ErrorMessage = "code can't be empty")]
        public string Code
        {
            get { return code; }
            set { this.SetField(p => p.Code, ref code, value); }
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

        private DateTime? endDate;
        public DateTime? EndDate
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

        private FuelReportDto endOfVoyageFuelReport;
        public FuelReportDto EndOfVoyageFuelReport
        {
            get { return this.endOfVoyageFuelReport; }
            set { this.SetField(p => p.EndOfVoyageFuelReport, ref this.endOfVoyageFuelReport, value); }
        }

        private ObservableCollection<FuelReportInventoryOperationDto> endOfVoyageInventoryOperations;
        public ObservableCollection<FuelReportInventoryOperationDto> EndOfVoyageInventoryOperations
        {
            get { return this.endOfVoyageInventoryOperations; }
            set { this.SetField(p => p.EndOfVoyageInventoryOperations, ref this.endOfVoyageInventoryOperations, value); }
        }
    }
}
