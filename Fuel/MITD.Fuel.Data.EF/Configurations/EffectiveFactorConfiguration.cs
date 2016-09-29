using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class EffectiveFactorConfiguration : EntityTypeConfiguration<EffectiveFactor>
    {
        public EffectiveFactorConfiguration()
        {
            this.HasKey(p => p.Id).ToTable("EffectiveFactor", "Fuel");
            this.Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.TimeStamp).IsRowVersion();

            HasMany(p => p.Segments).WithOptional(p => p.EffectiveFactor).HasForeignKey(p => p.EffectiveFactorId);

            HasRequired(c => c.Account).WithMany(d=>d.EffectiveFactors).HasForeignKey(c=>c.AccountId).WillCascadeOnDelete(false);


            this.Property(c => c.Name).HasMaxLength(200);
            this.Property(c => c.EffectiveFactorType);
        }
    }
}