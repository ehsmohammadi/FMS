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
   public class CharterConfiguration:EntityTypeConfiguration<Charter>
   {
       public CharterConfiguration()
       {
           HasKey(p => p.Id).ToTable("Charter", "Fuel");
           Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
           Property(t => t.TimeStamp).IsRowVersion();
           Ignore(p => p.CharterState);

           HasOptional(p => p.Owner).WithMany().HasForeignKey(p =>  p.OwnerId).WillCascadeOnDelete(false);
           HasOptional(p => p.Charterer).WithMany().HasForeignKey(p => p.ChartererId).WillCascadeOnDelete(false);
           HasOptional(p => p.VesselInCompany).WithMany().HasForeignKey(p => p.VesselInCompanyId).WillCascadeOnDelete(false);
           HasOptional(p => p.Currency).WithMany().HasForeignKey(p => p.CurrencyId).WillCascadeOnDelete(false);

         //  HasMany(p => p.ApproveWorkflows).WithRequired(p => p.Charter).HasForeignKey(p => p.CharterId);
           HasMany(p => p.CharterItems).WithRequired(p => p.Charter).HasForeignKey(p => p.CharterId);
           HasMany(p => p.InventoryOperationItems).WithOptional(p => p.Charter).HasForeignKey(p => p.CharterId);
       }
    }
}
