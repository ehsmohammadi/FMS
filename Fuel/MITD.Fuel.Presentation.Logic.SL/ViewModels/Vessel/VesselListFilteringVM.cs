using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class VesselListFilteringVM : WorkspaceViewModel
    {
        private ObservableCollection<CompanyDto> owners;
        public ObservableCollection<CompanyDto> Owners
        {
            get { return owners; }
            set { this.SetField(p => p.Owners, ref owners, value); }
        }

        private CompanyDto selectedOwner;
        public CompanyDto SelectedOwner
        {
            get { return selectedOwner; }
            set { this.SetField(p => p.SelectedOwner, ref selectedOwner, value); }
        }

        
        public long? SelectedOwnerId
        {
            get { return (SelectedOwner == null || SelectedOwner.Id == long.MinValue) ? null : (long?)SelectedOwner.Id; }
        }

        public VesselListFilteringVM()
        {
            this.Owners = new ObservableCollection<CompanyDto>();
        }

        public void Initialize(IEnumerable<CompanyDto> companyDtos)
        {
            this.Owners.Clear();

            this.Owners.Add(new CompanyDto()
                                {
                                    Id = long.MinValue,
                                    Code = string.Empty,
                                    Name = string.Empty
                                });

            foreach (var company in companyDtos)
            {
                this.Owners.Add(company);
            }

            ResetToDefaults();

            if (this.Owners.Count == 2)
            {
                this.SelectedOwner = this.Owners[1];
            }
        }

        public void ResetToDefaults()
        {
            this.SelectedOwner = null;
        }
    }
}
