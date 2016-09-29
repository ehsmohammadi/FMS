using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations.BaseInfo
{
    public class SharedGoodConfiguration : EntityTypeConfiguration<SharedGood>
    {

        public SharedGoodConfiguration()
        {
            this.HasKey(p => p.Id)
                .ToTable("SharedGoodView", "BasicInfo");
                //.ToTable("InventorySharedGoodView", "BasicInfo");

            this.Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Name)
                .HasMaxLength(200);

            this.HasRequired(p => p.MainUnit).WithMany().HasForeignKey(p => p.MainUnitId)
                                        .WillCascadeOnDelete(false);

        }

    }
}