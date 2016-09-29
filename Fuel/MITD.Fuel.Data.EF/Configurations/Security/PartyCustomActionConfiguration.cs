using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.FuelSecurity.Domain.Model;

namespace MITD.Fuel.Data.EF.Configurations.Security
{
    public class PartyCustomActionConfiguration : EntityTypeConfiguration<PartyCustomAction>
    {
        public PartyCustomActionConfiguration()
        {
            HasKey(p => p.Id).ToTable("Parties_CustomActions");

            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.IsGranted);

            HasRequired(p => p.Party).WithMany(p => p.CustomActions).HasForeignKey(p => p.PartyId).WillCascadeOnDelete(false);

            HasRequired(p => p.ActionType).WithMany().HasForeignKey(p => p.ActionTypeId).WillCascadeOnDelete(false);
        }
    }
}