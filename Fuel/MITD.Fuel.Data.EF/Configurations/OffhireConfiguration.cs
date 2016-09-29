using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class OffhireConfiguration : EntityTypeConfiguration<Offhire>
    {
        public OffhireConfiguration()
        {
            HasKey(p => p.Id).ToTable("Offhire", "Fuel");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.ReferenceNumber);
            Property(p => p.StartDateTime);
            Property(p => p.EndDateTime);

            Property(p => p.VoucherDate);
            HasRequired(p => p.VoucherCurrency).WithMany().HasForeignKey(p => p.VoucherCurrencyId).WillCascadeOnDelete(false);

            Property(p => p.PricingReference.Number);
            Property(p => p.PricingReference.Type);

            Property(p => p.IntroducerType);

            Property(p => p.IntroducerId);
            HasRequired(p => p.Introducer).WithMany().HasForeignKey(p => p.IntroducerId).WillCascadeOnDelete(false);

            Property(p => p.OffhireLocationId);
            HasRequired(p => p.OffhireLocation).WithMany().HasForeignKey(p => p.OffhireLocationId).WillCascadeOnDelete(false);

            Property(p => p.VoyageId);
            HasOptional(p => p.Voyage).WithMany().HasForeignKey(p => p.VoyageId).WillCascadeOnDelete(false);

            Property(p => p.VesselInCompanyId);
            HasRequired(p => p.VesselInCompany).WithMany().HasForeignKey(p => p.VesselInCompanyId).WillCascadeOnDelete(false);

            HasMany(p => p.OffhireDetails).WithRequired(d => d.Offhire).HasForeignKey(p => p.OffhireId);


            Property(p => p.State);
            Ignore(p => p.EntityState);

            Property(p => p.TimeStamp).IsRowVersion();
        }
    }
}