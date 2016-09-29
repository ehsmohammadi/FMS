using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;


namespace MITD.Fuel.Data.EF.Configurations
{
    public class InvoiceItemConfiguration : EntityTypeConfiguration<InvoiceItem>
    {
        public InvoiceItemConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("InvoiceItems", "Fuel");
            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.TimeStamp).IsRowVersion();

            HasRequired(c => c.Invoice)
                .WithMany(c => c.InvoiceItems)
                .HasForeignKey(c => c.InvoiceId);

            HasRequired(c => c.Good).WithMany().HasForeignKey(c => c.GoodId).WillCascadeOnDelete(false);
            HasRequired(c => c.MeasuringUnit).WithMany().HasForeignKey(c => c.MeasuringUnitId).WillCascadeOnDelete(false);
            Ignore(c => c.Price);

            Property(p => p.Quantity).HasPrecision(18, 3);
            Property(p => p.Fee).HasPrecision(18, 2);
            Property(p => p.DivisionPrice).HasPrecision(18, 2);
        }
    }
}