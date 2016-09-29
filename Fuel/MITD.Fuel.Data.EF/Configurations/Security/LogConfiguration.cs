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
   public class LogConfiguration :EntityTypeConfiguration<Log>
    {
       public LogConfiguration()
       {
           HasKey(p => p.Id).ToTable("Logs", "Fuel");
           Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
           this.Property(p => p.ClassName);
           this.Property(p => p.Code);
           this.Property(p => p.LogDate);
           this.Property(p => p.Messages);
           this.Property(p => p.MethodName);
           this.Property(p => p.PartyId);
           this.Property(p => p.Title);
           this.Property(p => p.LogLevelId);
           this.Ignore(p => p.LogLevel);


       }
    }
}
