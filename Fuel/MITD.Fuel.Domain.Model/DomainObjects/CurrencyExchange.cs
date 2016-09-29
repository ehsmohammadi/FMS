#region

using System;
using MITD.Fuel.Domain.Model.Exceptions;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class CurrencyExchange
    {
        public CurrencyExchange()
        {
            //To be used as parameter 'TEntity' in the generic type or method 'MITD.DataAccess.EF.EntityTypeConfigurationBase<TEntity>'	                        
        }

        public int Id { get; set; }
        public long FromCurrencyId { get; set; }
        public long ToCurrencyId { get; set; }
        public decimal Coefficient { get; set; }
        public DateTime? EffectiveDateStart { get; set; }
        public DateTime? EffectiveDateEnd { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual Currency FromCurrency { get; set; }
        public virtual Currency ToCurrency { get; set; }
    }
}