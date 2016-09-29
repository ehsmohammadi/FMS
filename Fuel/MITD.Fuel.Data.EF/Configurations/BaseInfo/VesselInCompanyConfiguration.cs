

#region

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Data.EF.Configurations.BaseInfo
{
    public class VesselInCompanyConfiguration : EntityTypeConfiguration<VesselInCompany>
    {
        public VesselInCompanyConfiguration()
        {
            HasKey(p => p.Id).ToTable("VesselInCompany", "Fuel");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Ignore(p => p.Code);//.HasMaxLength(50);

            Property(p => p.Name).HasMaxLength(200);

            Property(p => p.Description).HasMaxLength(2000);

            Property(p => p.CompanyId);

            Property(p => p.VesselId);

            Property(p => p.VesselStateCode).IsRequired();

            HasRequired(p => p.Company).WithMany(ep => ep.VesselsOperationInCompany).HasForeignKey(p => p.CompanyId).WillCascadeOnDelete(false);

            HasRequired(p => p.Vessel).WithMany(v => v.VesselOperationStates).HasForeignKey(p => p.VesselId);

            Ignore(p => p.Tanks);
            Ignore(p => p.IsActive);
            Ignore(p => p.VesselInInventory);
            Ignore(p => p.VesselInCompanyState);
            //HasRequired(p => p.VesselInInventory).WithRequiredDependent().Map(fk => fk.MapKey("CompanyId", "Code"));// (p => new { p.CompanyId, p.Code });

            Property(t => t.RowVersion).IsRowVersion();
        }
    }
}