using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations.Financial
{
   public class FreeAccountConfiguration:EntityTypeConfiguration<FreeAccount>
   {
       public FreeAccountConfiguration()
       {
           HasKey(p => p.Id).ToTable("FreeAccounts", "Fuel");
           Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
       }
    }
}
