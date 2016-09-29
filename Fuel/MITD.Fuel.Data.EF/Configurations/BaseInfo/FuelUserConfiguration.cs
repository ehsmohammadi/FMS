using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations.BaseInfo
{
    public class FuelUserConfiguration : EntityTypeConfiguration<FuelUser>
    {
        public FuelUserConfiguration()
        {
            HasKey(p => p.Id).ToTable("UserView", "BasicInfo");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(p => p.IdentityId);
            
            Property(p => p.Name).HasMaxLength(200);

            Property(p => p.IsFRApprover);

            HasRequired(p => p.Company).WithMany(ep => ep.Users).HasForeignKey(u => u.CompanyId);
        }
    }
}