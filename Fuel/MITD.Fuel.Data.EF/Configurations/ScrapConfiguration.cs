using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class ScrapConfiguration : EntityTypeConfiguration<Scrap>
    {
        public ScrapConfiguration()
        {
            HasKey(p => p.Id).ToTable("Scrap", "Fuel");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.ScrapDate);

            HasRequired(p => p.VesselInCompany).WithMany().HasForeignKey(p => p.VesselInCompanyId).WillCascadeOnDelete(false);
            HasRequired(p => p.SecondParty).WithMany().HasForeignKey(p => p.SecondPartyId).WillCascadeOnDelete(false);

            HasMany(p => p.ScrapDetails).WithRequired(d => d.Scrap);

            Property(p => p.State);
            Ignore(p => p.EntityState);

            Property(p => p.TimeStamp).IsRowVersion();
            HasMany(p => p.InventoryOperations).WithOptional(p=>p.Scrap).HasForeignKey(p=>p.Scrap_Id).WillCascadeOnDelete(false);
        }
    }
}