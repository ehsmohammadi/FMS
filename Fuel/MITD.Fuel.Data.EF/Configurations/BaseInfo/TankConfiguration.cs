using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations.BaseInfo
{
    internal class TankConfiguration : EntityTypeConfiguration<Tank>
    {
        public TankConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("CompanyVesselTankView", "BasicInfo");
                //.ToTable("InventoryCompanyVesselTankView", "BasicInfo");


            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(p => p.Name).HasMaxLength(200);

            Property(p => p.VesselInInventoryId);

            HasRequired(p => p.VesselInInventory).WithMany(v => v.Tanks).HasForeignKey(t => t.VesselInInventoryId);
        }

    }
}
