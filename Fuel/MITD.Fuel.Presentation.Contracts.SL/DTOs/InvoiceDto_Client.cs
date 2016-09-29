using System.Linq;
using System.Windows.Media;
using MITD.Fuel.Presentation.Contracts.SL.Extensions;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class InvoiceDto : ViewModelBase
    {
        public decimal TotalOfDivisionPrice
        {
            get { return totalOfadditionalPrice; }
            set { this.SetField(p => p.TotalOfDivisionPrice, ref totalOfadditionalPrice, value); }
        }
        
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == this.GetPropertyName(p => p.AdditionalPrices))
            {
                if (this.AdditionalPrices == null || this.AdditionalPrices.Count == 0)
                    this.TotalOfDivisionPrice = 0;
                this.TotalOfDivisionPrice = this.AdditionalPrices.Sum(c => c.Price * (int)c.EffectiveFactorType);
            }

            if (propertyName == this.GetPropertyName(p => p.TotalOfDivisionPrice))
            {
            }
        }
    }
}
