#region

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Data.EF.Configurations
{
    public class InventoryOperationConfiguration : EntityTypeConfiguration<InventoryOperation>
    {
        public InventoryOperationConfiguration()
        {
            HasKey(p => p.Id).ToTable("InventoryOperation", "Fuel");

            // Properties:
            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.InventoryOperationId);
            Property(p => p.ActionDate);
            Property(p => p.ActionNumber).HasMaxLength(200);
            Property(p => p.ActionType);

            Property(p => p.TimeStamp).IsRowVersion();
        }
    }
}