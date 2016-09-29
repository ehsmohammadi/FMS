using System.Data.Entity.ModelConfiguration;
using MITD.FuelSecurity.Domain.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace MITD.Fuel.Data.EF.Configurations.Security
{
    public class PartyConfiguration : EntityTypeConfiguration<Party>
    {
        public PartyConfiguration()
        {
            ToTable("Parties");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            this.Property(p => p.PartyName);
            this.Property(p => p.RowVersion).IsRowVersion();
        }
    }
}