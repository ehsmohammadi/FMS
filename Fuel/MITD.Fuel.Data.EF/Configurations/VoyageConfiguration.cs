

#region

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Data.EF.Configurations
{
    public class VoyageConfiguration : EntityTypeConfiguration<Voyage>
    {
        public VoyageConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("Voyage", "Fuel");
                //.ToTable("VoyagesView", "BasicInfo");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(p => p.VoyageNumber);

            Property(p => p.Description);

            Property(p => p.VesselInCompanyId);

            Property(p => p.CompanyId);
            Property(p => p.StartDate);
            Property(p => p.EndDate);
            Property(p => p.IsActive);
            Property(p => p.IsLocked);

            HasRequired(v => v.VesselInCompany).WithMany().HasForeignKey(o => o.VesselInCompanyId);
            HasRequired(v => v.Company).WithMany().HasForeignKey(o => o.CompanyId);
        }
    }
}