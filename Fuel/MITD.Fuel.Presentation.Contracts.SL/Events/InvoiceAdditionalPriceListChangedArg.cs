using System;
using System.Collections.ObjectModel;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.Events
{
    public class InvoiceAdditionalPriceListChangedArg
    {
        private ObservableCollection<InvoiceAdditionalPriceDto> data;

        public ObservableCollection<InvoiceAdditionalPriceDto> Data 
        {
            get { return this.data; }
            set { this.data = value; }
        }

        public InvoiceAdditionalPriceListChangedArg(ObservableCollection<InvoiceAdditionalPriceDto> data)
        {
            this.Data = data;
        }
    }
}