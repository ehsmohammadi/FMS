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
    public class ScrapListFilteringVM : WorkspaceViewModel
    {
        private ObservableCollection<CompanyDto> companies;
        public ObservableCollection<CompanyDto> Companies
        {
            get { return companies; }
            set { this.SetField(p => p.Companies, ref companies, value); }
        }

        private CompanyDto selectedCompany;
        public CompanyDto SelectedCompany
        {
            get { return selectedCompany; }
            set { this.SetField(p => p.SelectedCompany, ref selectedCompany, value); }
        }

        public long? SelectedCompanyId
        {
            get { return (SelectedCompany == null || SelectedCompany.Id == long.MinValue) ? null : (long?)SelectedCompany.Id; }
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

        public ScrapListFilteringVM()
        {
            this.Companies = new ObservableCollection<CompanyDto>();
        }

        public void Initialize(IEnumerable<CompanyDto> companyDtos)
        {
            this.Companies.Clear();

            this.Companies.Add(new CompanyDto()
                                {
                                    Id = long.MinValue,
                                    Code = string.Empty,
                                    Name = string.Empty
                                });

            foreach (var company in companyDtos)
            {
                this.Companies.Add(company);
            }

            ResetToDefaults();
        }

        public void ResetToDefaults()
        {
            this.SelectedCompany = this.Companies.Count == 2 ? this.Companies[1] : null;

            this.FromDate = null;

            this.ToDate = null;
        }
    }
}
