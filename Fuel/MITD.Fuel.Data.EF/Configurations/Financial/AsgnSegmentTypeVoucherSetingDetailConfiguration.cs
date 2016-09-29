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
   public class AsgnSegmentTypeVoucherSetingDetailConfiguration:EntityTypeConfiguration<AsgnSegmentTypeVoucherSetingDetail>
   {
       public AsgnSegmentTypeVoucherSetingDetailConfiguration()
       {
           HasKey(c => c.Id).ToTable("AsgnSegmentTypeVoucherSetingDetail", "Fuel");
           Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
           Property(c => c.Typ).HasColumnName("Type");

           HasRequired(c => c.VoucherSetingDetail)
               .WithMany(d => d.AsgnSegmentTypeVoucherSetingDetails)
               .HasForeignKey(x => x.VoucherSetingDetailId)
               .WillCascadeOnDelete(false);
           


       }
    }
}
