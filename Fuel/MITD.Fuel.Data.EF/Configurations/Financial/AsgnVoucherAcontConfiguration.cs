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
    // AsgnVoucherAconts
    public class AsgnVoucherAcontConfiguration : EntityTypeConfiguration<AsgnVoucherAcont>
    {
        public AsgnVoucherAcontConfiguration(string schema = "Fuel")
        {
            ToTable("AsgnVoucherAconts",schema);
           
            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            Property(x => x.TimeStamp)
                .IsRowVersion();

            Property(x => x.Typ).HasColumnName("Type");

            // Foreign keys
            HasRequired(a => a.VoucherSetingDetail)
                .WithMany(b => b.AsgnVoucherAconts)
                .HasForeignKey(c => c.VoucherSetingDetailId); 
         
            HasRequired(a => a.Account)
                .WithMany(b => b.AsgnVoucherAconts)
                .HasForeignKey(c => c.AccountId);
                
        }
    }                               
}


