using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;

namespace MITD.Fuel.Data.EF.Configurations.Financial
{
    public class VoucherSetingConfiguration : EntityTypeConfiguration<VoucherSeting>
    {
        public VoucherSetingConfiguration()
        {

            HasKey(c => c.Id).ToTable("VoucherSetings","Fuel");
            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.TimeStamp).IsRowVersion();
            Ignore(c => c.VoucherDetailType);
                 
            HasOptional(c=>c.Company).WithMany().HasForeignKey(c=>c.CompanyId).WillCascadeOnDelete(false);

            

        }
    }
}
