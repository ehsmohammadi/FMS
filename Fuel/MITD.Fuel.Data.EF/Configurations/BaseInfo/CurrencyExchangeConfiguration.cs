using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations.BaseInfo
{
    public class CurrencyExchangeConfiguration : EntityTypeConfiguration<CurrencyExchange>
    {
        public CurrencyExchangeConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("CurrencyExchangeView", "BasicInfo");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(p => p.FromCurrency).WithMany().HasForeignKey(p => p.FromCurrencyId);
            HasRequired(p => p.ToCurrency).WithMany().HasForeignKey(p => p.ToCurrencyId);
        }
    }
}
