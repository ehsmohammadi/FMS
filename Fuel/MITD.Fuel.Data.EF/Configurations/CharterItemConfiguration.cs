using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class CharterItemConfiguration : EntityTypeConfiguration<CharterItem>
    {
        public CharterItemConfiguration()
        {
            HasKey(p => p.Id).ToTable("CharterItem", "Fuel");
            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.TimeStamp).IsRowVersion();

            HasRequired(p => p.Good).WithMany().HasForeignKey(p => p.GoodId).WillCascadeOnDelete(false);
            HasRequired(c => c.GoodUnit).WithMany().HasForeignKey(p => p.GoodUnitId).WillCascadeOnDelete(false);
            HasRequired(p => p.Charter).WithMany().HasForeignKey(p => p.CharterId).WillCascadeOnDelete(false);
            HasOptional(p => p.Tank).WithMany().HasForeignKey(p => p.TankId).WillCascadeOnDelete(false);
            Property(p => p.Rob).HasPrecision(18, 3);
            Property(p => p.Fee).HasPrecision(18, 2);
            Property(p => p.OffhireFee).HasPrecision(18, 2);
        }
    }
}
