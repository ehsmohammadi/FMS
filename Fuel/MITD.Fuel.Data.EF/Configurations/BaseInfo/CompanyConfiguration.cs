using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations.BaseInfo
{
    public class CompanyConfiguration : EntityTypeConfiguration<Company>
    {
        public CompanyConfiguration()
        {
            HasKey(p => new { p.Id })
               .ToTable("CompanyView", "BasicInfo");
               //.ToTable("InventoryCompanyView", "BasicInfo");

            Property(p => p.Id)
               .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(p => p.Name)
               .HasMaxLength(150);

            Property(p => p.Code)
                .HasMaxLength(50);

            Property(p => p.IsMemberOfHolding);
            
            Property(p => p.BasisId);
        }
    }
}
