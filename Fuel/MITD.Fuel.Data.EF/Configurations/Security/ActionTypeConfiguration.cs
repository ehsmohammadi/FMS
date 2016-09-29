using System.Data.Entity.ModelConfiguration;
using MITD.FuelSecurity.Domain.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace MITD.Fuel.Data.EF.Configurations.Security
{
    public class ActionTypeConfiguration : EntityTypeConfiguration<ActionType>
    {
        public ActionTypeConfiguration()
        {
            this.HasKey(p => p.Id).ToTable("ActionTypes");
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}