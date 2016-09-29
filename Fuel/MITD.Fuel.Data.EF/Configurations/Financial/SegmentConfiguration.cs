using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;

namespace MITD.Fuel.Data.EF.Configurations.Financial
{
    public class SegmentConfiguration : EntityTypeConfiguration<Segment>
    {
        public SegmentConfiguration()
        {
            ToTable("Segments", "Fuel")
                .HasKey(c => c.Id);
            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //Ignore(c => c.SegmentType);
            Property(c => c.SegmentType.Id).HasColumnName("SegmentTypeId");
            Property(c => c.SegmentType.Code).HasColumnName("SegmentTypeCode");
            Property(c => c.SegmentType.Name).HasColumnName("SegmentTypeName");
           // HasRequired(c => c.JournalEntry).WithMany(d => d.Segments).HasForeignKey(c => c.JournalEntryId).WillCascadeOnDelete(false);
            //Property(c => c.SegmentTypeId).HasColumnName("SegmentTypeId");

            Property(c => c.TimeStamp).IsRowVersion();

            HasOptional(c => c.FreeAccount).WithMany(d => d.Segments).HasForeignKey(x => x.FreeAccountId).WillCascadeOnDelete(false);

            // HasRequired(c => c.JournalEntry).WithOptional(d => d.Segment).Map(m => m.MapKey("JournalEntryId"));
            //.Map(m => m.MapKey("JournalEntryId")).WillCascadeOnDelete(false);
        }
    }
}
