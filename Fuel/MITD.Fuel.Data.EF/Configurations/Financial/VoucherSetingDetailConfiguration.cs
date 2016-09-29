using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;

namespace MITD.Fuel.Data.EF.Configurations.Financial
{
    public class VoucherSetingDetailConfiguration : EntityTypeConfiguration<VoucherSetingDetail>
    {
        public VoucherSetingDetailConfiguration()
        {

            HasKey(c => c.Id).ToTable("VoucherSetingDetails", "Fuel");
            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(c => c.Good).WithMany().HasForeignKey(c => c.GoodId).WillCascadeOnDelete(false);
           
            HasRequired(c => c.VoucherSeting).WithMany(d=>d.VoucherSetingDetails).HasForeignKey(c => c.VoucherSetingId).WillCascadeOnDelete(false);


        



            Ignore(c => c.CreditSegmentTypes);
            Ignore(c => c.DebitSegmentTypes);

            //HasMany(c => c.Accounts)
            //    .WithMany(d => d.VoucherSetingDetails)
            //    .Map(m =>
            //         {
            //             m.MapLeftKey("VoucherSetingDetailId");
            //             m.MapRightKey("AccountId");
            //             m.ToTable("AsgnVoucherAconts", "Fuel");
            //         });

        }
    }
}
