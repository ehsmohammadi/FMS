using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.FuelSecurity.Domain.Model;

namespace MITD.Fuel.Data.EF.Configurations.Security
{
    public class ExceptionLogConfiguration : EntityTypeConfiguration<ExceptionLog>
    {
        public ExceptionLogConfiguration()
        {
            HasKey(p => p.Id).ToTable("ExceptionLogs", "Fuel");
            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
           

        }
    }
}
