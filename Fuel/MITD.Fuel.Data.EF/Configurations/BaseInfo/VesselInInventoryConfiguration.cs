

#region

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Data.EF.Configurations.BaseInfo
{
    public class VesselInInventoryConfiguration : EntityTypeConfiguration<VesselInInventory>
    {
        public VesselInInventoryConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("CompanyVesselView", "BasicInfo");
                //.ToTable("InventoryCompanyVesselView", "BasicInfo");

            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(p => p.CompanyId);

            Property(p => p.Code);

            //HasKey(p => new { p.CompanyId, p.Code });

            Property(p => p.IsActive);

            HasRequired(p => p.Company).WithMany().HasForeignKey(p => p.CompanyId).WillCascadeOnDelete(false);

            HasMany(p => p.Tanks).WithRequired(t => t.VesselInInventory).HasForeignKey(p => p.VesselInInventoryId);
        }
    }
}