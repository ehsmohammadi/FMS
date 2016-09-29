#region

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;

#endregion

namespace MITD.Fuel.Data.EF.Configurations
{
    public class InvoiceConfiguration : EntityTypeConfiguration<Invoice>
    {
        public InvoiceConfiguration()
        {
            HasKey(p => p.Id).ToTable("Invoice", "Fuel");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.TimeStamp).IsRowVersion();

            HasOptional(c => c.Transporter).WithMany().HasForeignKey(c => c.TransporterId).WillCascadeOnDelete(false);
            HasOptional(c => c.Supplier).WithMany().HasForeignKey(c => c.SupplierId).WillCascadeOnDelete(false);
            HasRequired(c => c.Owner).WithMany().HasForeignKey(c => c.OwnerId).WillCascadeOnDelete(false);

            HasMany(c => c.OrderRefrences).WithMany(c => c.Invoices).Map(m => m.ToTable("InvoiceOrders", "Fuel"));

            Ignore(c => c.InvoiceState);

            HasOptional(p => p.InvoiceRefrence).WithMany(p => p.Attachments).HasForeignKey(p => p.InvoiceRefrenceId);
        }
    }
}