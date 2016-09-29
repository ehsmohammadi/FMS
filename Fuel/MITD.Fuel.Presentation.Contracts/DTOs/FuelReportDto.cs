using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using MITD.Fuel.Presentation.Contracts.DTOs.Report;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{

    public partial class FuelReportDto
    {
        public FuelReportDto()
        {
        }

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



        string _CurrentStateName;
        public string CurrentStateName
        {
            get { return _CurrentStateName; }
            set { this.SetField(p => p.CurrentStateName, ref _CurrentStateName, value); }
        }

        string _UserInChargName;
        public string UserInChargName
        {
            get { return _UserInChargName; }
            set { this.SetField(p => p.UserInChargName, ref _UserInChargName, value); }
        }

        FuelReportTypeEnum _fuelReportType;
        public FuelReportTypeEnum FuelReportType
        {
            get { return _fuelReportType; }
            set { this.SetField(p => p.FuelReportType, ref _fuelReportType, value); }
        }

        DateTime _reportDate;
        public DateTime ReportDate
        {
            get { return _reportDate; }
            set { this.SetField(p => p.ReportDate, ref _reportDate, value); }
        }

        DateTime _eventDate;
        public DateTime EventDate
        {
            get { return _eventDate; }
            set { this.SetField(p => p.EventDate, ref _eventDate, value); }
        }

        VoyageDto _voyage;
        public VoyageDto Voyage
        {
            get { return _voyage; }
            set { this.SetField(p => p.Voyage, ref _voyage, value); }
        }

        private bool _enableCommercialEditing;
        public bool EnableCommercialEditing
        {
            get { return _enableCommercialEditing; }
            set { this.SetField(p => p.EnableCommercialEditing, ref _enableCommercialEditing, value); }
        }

        private VesselEventReportViewDto vesselEventReportViewDto;

        public VesselEventReportViewDto VesselEventReportViewDto
        {
            get { return vesselEventReportViewDto; }
            set { this.SetField(p => p.VesselEventReportViewDto, ref vesselEventReportViewDto, value); }
        }

        private VesselInCompanyDto vesselInCompanyDto;
        public VesselInCompanyDto VesselInCompanyDto
        {
            get { return vesselInCompanyDto; }
            set { this.SetField(p => p.VesselInCompanyDto, ref vesselInCompanyDto, value); }
        }

        private ObservableCollection<FuelReportDetailDto> fuelReportDetail;
        public ObservableCollection<FuelReportDetailDto> FuelReportDetail
        {
            get { return this.fuelReportDetail; }
            set { this.SetField(p => p.FuelReportDetail, ref this.fuelReportDetail, value); }
        }

        private bool isTheFirstRecord;
        public bool IsTheFirstRecord
        {
            get { return isTheFirstRecord; }
            set { this.SetField(p => p.IsTheFirstRecord, ref this.isTheFirstRecord, value); }
        }

        private bool hasUpdateRequest;
        public bool HasUpdateRequest
        {
            get { return hasUpdateRequest; }
            set { this.SetField(p => p.HasUpdateRequest, ref this.hasUpdateRequest, value); }
        }

        public List<FuelReportInventoryOperationDto> InventoryOperationDtos
        {
            get { return this.inventoryOperationDtos; }
            set { this.SetField(p => p.InventoryOperationDtos, ref this.inventoryOperationDtos, value); }
        }

        private List<FuelReportInventoryOperationDto> inventoryOperationDtos;

        private WorkflowStageEnum approveStatus;

        public WorkflowStageEnum ApproveStatus
        {
            get { return this.approveStatus; }
            set { this.SetField(p => p.ApproveStatus, ref this.approveStatus, value); }
        }
    }
}
