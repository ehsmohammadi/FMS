using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class CharterInConfiguration:EntityTypeConfiguration<CharterIn>
    {
        public CharterInConfiguration()
        {
            HasKey(p => p.Id).ToTable("CharterIn", "Fuel");
            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.TimeStamp).IsRowVersion();
           

        }

    }
}
