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
    public class CurrencyExchangeListFilteringVM : WorkspaceViewModel
    {
        private ObservableCollection<CurrencyDto> fromCurrencyDtos;
        public ObservableCollection<CurrencyDto> FromCurrencyDtos
        {
            get { return fromCurrencyDtos; }
            set { this.SetField(p => p.FromCurrencyDtos, ref fromCurrencyDtos, value); }
        }

        private ObservableCollection<CurrencyDto> toCurrencyDtos;
        public ObservableCollection<CurrencyDto> ToCurrencyDtos
        {
            get { return toCurrencyDtos; }
            set { this.SetField(p => p.ToCurrencyDtos, ref toCurrencyDtos, value); }
        }

        private CurrencyDto selectedFromCurrency;
        public CurrencyDto SelectedFromCurrency
        {
            get { return selectedFromCurrency; }
            set { this.SetField(p => p.SelectedFromCurrency, ref selectedFromCurrency, value); }
        }

        public long? SelectedFromCurrencyId
        {
            get { return (SelectedFromCurrency == null || SelectedFromCurrency.Id == long.MinValue) ? null : (long?)SelectedFromCurrency.Id; }
        }

        private CurrencyDto selectedToCurrency;
        public CurrencyDto SelectedToCurrency
        {
            get { return selectedToCurrency; }
            set { this.SetField(p => p.SelectedToCurrency, ref selectedToCurrency, value); }
        }

        public long? SelectedToCurrencyId
        {
            get { return (SelectedToCurrency == null || SelectedToCurrency.Id == long.MinValue) ? null : (long?)SelectedToCurrency.Id; }
        }

        private ObservableCollection<FiscalYearDto> fiscalYearDtos;
        public ObservableCollection<FiscalYearDto> FiscalYearDtos
        {
            get { return fiscalYearDtos; }
            set { this.SetField(p => p.FiscalYearDtos, ref fiscalYearDtos, value); }
        }

        private FiscalYearDto selectedFiscalYear;
        public FiscalYearDto SelectedFiscalYear
        {
            get { return selectedFiscalYear; }
            set { this.SetField(p => p.SelectedFiscalYear, ref selectedFiscalYear, value); }
        }

        public int SelectedFiscalYearId
        {
            get { return (SelectedFiscalYear == null || SelectedFiscalYear.Id == long.MinValue) ? 0 : SelectedFiscalYear.Id; }
        }

        public int SelectedFiscalYearNumber
        {
            get { return (SelectedFiscalYear == null || SelectedFiscalYear.Id == long.MinValue) ? 0 : SelectedFiscalYear.YearNumber; }
        }

        public CurrencyExchangeListFilteringVM()
        {
            this.FromCurrencyDtos = new ObservableCollection<CurrencyDto>();
            this.ToCurrencyDtos = new ObservableCollection<CurrencyDto>();

            this.FiscalYearDtos = new ObservableCollection<FiscalYearDto>();
        }

        public void SetFromCurrencies(IEnumerable<CurrencyDto> fromCurrencyDtos)
        {
            this.FromCurrencyDtos.Clear();

            this.FromCurrencyDtos.Add(new CurrencyDto()
                                {
                                    Id = long.MinValue,
                                    Abbreviation = "--",
                                    Name = "--"
                                });

            foreach (var currency in fromCurrencyDtos)
            {
                this.FromCurrencyDtos.Add(currency);
            }

            this.SelectedFromCurrency = this.FromCurrencyDtos.Count == 2 ? this.FromCurrencyDtos[1] : null;
        }
        public void SetToCurrencies(IEnumerable<CurrencyDto> toCurrencyDtos)
        {
            this.ToCurrencyDtos.Clear();

            this.ToCurrencyDtos.Add(new CurrencyDto()
            {
                Id = long.MinValue,
                Abbreviation = "--",
                Name = "--"
            });

            foreach (var currency in toCurrencyDtos)
            {
                this.ToCurrencyDtos.Add(currency);
            }
            this.SelectedToCurrency = this.ToCurrencyDtos.Count == 2 ? this.ToCurrencyDtos[1] : null;

        }
        public void SetFiscalYears(IEnumerable<FiscalYearDto> fiscalYearDtos)
        {
            this.FiscalYearDtos.Clear();

            //this.FiscalYearDtos.Add(new FiscalYearDto()
            //{
            //    Id = int.MinValue,
            //    DisplayText = string.Empty,
            //    YearNumber = 0
            //});

            foreach (var fiscalYear in fiscalYearDtos)
            {
                this.FiscalYearDtos.Add(fiscalYear);
            }

            this.SelectedFiscalYear = this.FiscalYearDtos.Count >= 1 ? this.FiscalYearDtos[0] : null;
        }

        public void ResetToDefaults()
        {
            this.SelectedFromCurrency = this.FromCurrencyDtos.Count == 2 ? this.FromCurrencyDtos[1] : null;
            this.SelectedToCurrency = this.ToCurrencyDtos.Count == 2 ? this.ToCurrencyDtos[1] : null;
            this.SelectedFiscalYear = this.FiscalYearDtos.Count == 2 ? this.FiscalYearDtos[1] : null;
        }
    }
}
