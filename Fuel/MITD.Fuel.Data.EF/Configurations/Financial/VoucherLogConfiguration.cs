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
    public class VoucherLogConfiguration : EntityTypeConfiguration<VoucherLog>
    {
        public VoucherLogConfiguration()
        {
            HasKey(p => p.Id).ToTable("VoucherLogs", "Fuel");
            Property(P => P.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
