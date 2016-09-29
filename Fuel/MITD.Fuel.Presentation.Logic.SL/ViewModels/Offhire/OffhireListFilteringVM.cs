using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Castle.DynamicProxy.Generators.Emitters;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class OffhireListFilteringVM : WorkspaceViewModel
    {
        private ObservableCollection<VesselInCompanyDto> vessels;
        public ObservableCollection<VesselInCompanyDto> Vessels
        {
            get { return vessels; }
            set { this.SetField(p => p.Vessels, ref vessels, value); }
        }

        private VesselInCompanyDto selectedVessel;
        public VesselInCompanyDto SelectedVessel
        {
            get { return selectedVessel; }
            set { this.SetField(p => p.SelectedVessel, ref selectedVessel, value); }
        }

        public long? SelectedVesselId
        {
            get { return (SelectedVessel == null || SelectedVessel.Id == long.MinValue) ? null : (long?)SelectedVessel.Id; }
        }

        private DateTime? fromDate;
        public DateTime? FromDate
        {
            get { return fromDate; }
            set { this.SetField(p => p.FromDate, ref fromDate, value); }
        }

        private DateTime? toDate;
        public DateTime? ToDate
        {
            get { return toDate; }
            set { this.SetField(p => p.ToDate, ref toDate, value); }
        }

        public OffhireListFilteringVM()
        {
            this.Vessels = new ObservableCollection<VesselInCompanyDto>();
        }

        public void Initialize(IEnumerable<VesselInCompanyDto> vesselInCompanyDtos)
        {
            this.Vessels.Clear();

            this.Vessels.Add(new VesselInCompanyDto()
                             {
                                 Id = long.MinValue,
                                 Code = string.Empty,
                                 Name = string.Empty
                             });

            foreach (var vessel in vesselInCompanyDtos)
            {
                this.Vessels.Add(vessel);
            }

            ResetToDefaults();
        }

        public void ResetToDefaults()
        {
            this.SelectedVessel = null;

            this.FromDate = null;

            this.ToDate = null;
        }
    }
}
