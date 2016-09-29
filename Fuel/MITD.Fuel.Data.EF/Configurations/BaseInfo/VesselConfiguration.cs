

#region

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Data.EF.Configurations.BaseInfo
{
    public class VesselConfiguration : EntityTypeConfiguration<Vessel>
    {
        public VesselConfiguration()
        {
            HasKey(p => p.Id).ToTable("Vessel", "Fuel");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.Code).HasMaxLength(50);

            Property(p => p.OwnerId);

            HasRequired(p => p.Owner).WithMany(ep => ep.Fleet).HasForeignKey(p => p.OwnerId).WillCascadeOnDelete(false);

            Property(t => t.RowVersion).IsRowVersion();
        }
    }
}