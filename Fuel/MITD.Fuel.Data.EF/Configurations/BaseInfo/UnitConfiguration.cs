using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations.BaseInfo
{

    public class UnitConfiguration : EntityTypeConfiguration<Unit>
    {
        public UnitConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("UnitView", "BasicInfo");
                //.ToTable("InventoryUnitView", "BasicInfo");



            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(p => p.Name)
                .HasMaxLength(200);


            //// Association:
            //this
            //    .HasMany(p => p.OrderItems)
            //        .WithOptional(c => c.GoodUnit)
            //    .HasForeignKey(p => new { p.GoodUnitId })
            //        .WillCascadeOnDelete(false);
        }
    }
}
    