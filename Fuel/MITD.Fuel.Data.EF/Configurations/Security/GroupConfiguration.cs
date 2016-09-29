using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.FuelSecurity.Domain.Model;

namespace MITD.Fuel.Data.EF.Configurations.Security
{
    public class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
            ToTable("Groups");
           // this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(p => p.Description);
        }
    }
}