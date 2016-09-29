using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;


namespace MITD.Fuel.Data.EF.Configurations
{
    public class OrderItemOperatedQuantityConfiguration : EntityTypeConfiguration<OrderItemOperatedQuantity>
    {
        public OrderItemOperatedQuantityConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("OrderItemOperatedQuantity", "Fuel");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.QuantityAmountInMainUnit).HasPrecision(18, 3);
            Property(p => p.UnitCode).HasMaxLength(50);

            HasRequired(c => c.OrderItem)
                .WithMany(c => c.OrderItemOperatedQuantities)
                .HasForeignKey(c => c.OrderItemId)
                .WillCascadeOnDelete(false);

            HasRequired(c => c.FuelReportDetail)
                .WithMany()
                .HasForeignKey(c => c.FuelReportDetailId)
                .WillCascadeOnDelete(false);
        }
    }
}