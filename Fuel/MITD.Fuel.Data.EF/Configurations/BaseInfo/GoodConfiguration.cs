using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations.BaseInfo
{

    public class GoodConfiguration : EntityTypeConfiguration<Good>
    {
        public GoodConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("CompanyGoodView", "BasicInfo");
                //.ToTable("InventoryCompanyGoodView", "BasicInfo");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(p => p.Code);
            Property(p => p.Name).HasMaxLength(200);
            Property(p => p.CompanyId);
            Property(p => p.SharedGoodId);

            HasRequired(p => p.SharedGood).WithMany().HasForeignKey(p => p.SharedGoodId);
            HasRequired(p => p.Company).WithMany(c => c.Goods).HasForeignKey(p => p.CompanyId);
        }

    }
}
