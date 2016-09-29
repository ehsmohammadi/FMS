using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations.BaseInfo
{

    public class CompanyGoodUnitConfiguration : EntityTypeConfiguration<GoodUnit>
    {
        public CompanyGoodUnitConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("CompanyGoodUnitView", "BasicInfo");
                //.ToTable("InventoryCompanyGoodUnitView", "BasicInfo");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(p => p.GoodId).HasColumnName("CompanyGoodId");

            Property(p => p.Name).HasMaxLength(200);

            Property(p => p.Abbreviation).HasMaxLength(200);

            Property(p => p.Coefficient);

            Ignore(p => p.MainGoodUnit);

            HasRequired(p => p.Good).WithMany(c => c.GoodUnits).HasForeignKey(p => p.GoodId).WillCascadeOnDelete(false);

            HasOptional(p => p.Parent).WithMany(c => c.ChildList).HasForeignKey(p => p.ParentId).WillCascadeOnDelete(false);
        }
    }
}
