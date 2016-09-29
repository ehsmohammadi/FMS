using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations.BaseInfo
{
    public class CurrencyConfiguration : EntityTypeConfiguration<Currency>
    {
        public CurrencyConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("CurrencyView", "BasicInfo");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(p => p.Name)
                .HasMaxLength(200);

        }
    }
}
