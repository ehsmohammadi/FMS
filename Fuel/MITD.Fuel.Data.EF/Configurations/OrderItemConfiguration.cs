using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;


namespace MITD.Fuel.Data.EF.Configurations
{
    public class OrderItemConfiguration : EntityTypeConfiguration<OrderItem>
    {
        public OrderItemConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("OrderItems", "Fuel");
            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.TimeStamp).IsRowVersion();

            HasRequired(c => c.Order)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(c => c.OrderId);

            HasRequired(c => c.Good).WithMany().HasForeignKey(c => c.GoodId).WillCascadeOnDelete(false);
            HasRequired(c => c.MeasuringUnit).WithMany().HasForeignKey(c => c.MeasuringUnitId).WillCascadeOnDelete(false);

            Property(p => p.Quantity).HasPrecision(18, 3);
            Property(p => p.OperatedQuantityInMainUnit).HasPrecision(18, 3);
            Property(p => p.InvoicedInMainUnit).HasPrecision(18, 3);
        }
    }
}