using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class AccountConfiguration : EntityTypeConfiguration<Account>
    {
        public AccountConfiguration()
        {
            HasKey(c => c.Id).ToTable("Accounts", "Fuel");
            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.TimeStamp).IsRowVersion();



            //HasMany(c => c.VoucherSetingDetails)
            //    .WithMany(d => d.Accounts)
            //    .Map(m =>
            //    {
            //        m.MapLeftKey("AccountId");
            //        m.MapRightKey("VoucherSetingDetailId");
            //        m.ToTable("AsgnVoucherAconts", "Fuel");
            //    });
        }

    }
}
