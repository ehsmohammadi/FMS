using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;

namespace MITD.Fuel.Data.EF.Configurations.Financial
{
   public class JournalEntryConfiguration:EntityTypeConfiguration<JournalEntry>
   {
       public JournalEntryConfiguration()
       {
           ToTable("JournalEntries", "Fuel").HasKey(c => c.Id);
           Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
           HasRequired(c => c.Voucher).WithMany(d=>d.JournalEntrieses).HasForeignKey(c => c.VoucherId).WillCascadeOnDelete(true);

           HasRequired(c => c.Currency).WithMany().HasForeignKey(c => c.CurrencyId).WillCascadeOnDelete(false);
           HasMany(c => c.Segments).WithRequired(d => d.JournalEntry).HasForeignKey(f => f.JournalEntryId).WillCascadeOnDelete(true);


           Property(c => c.TimeStamp).IsRowVersion();
           
       }
    }
}
