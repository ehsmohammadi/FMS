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
    public class OriginalAccountConfiguration : EntityTypeConfiguration<OriginalAccount>
    {
        public OriginalAccountConfiguration()
        {
            HasKey(c => c.Code).ToTable("AccountListView", "BasicInfo");
         
           
        }

    }
}
