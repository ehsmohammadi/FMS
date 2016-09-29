

#region

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Data.EF.Configurations
{
    public class RotationVoyageConfiguration : EntityTypeConfiguration<RotationVoyage>
    {
        public RotationVoyageConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("RotationVoyagesView", "BasicInfo");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(p => p.VoyageNumber);

            Property(p => p.Description);

            Property(p => p.VesselInCompanyId);

            Property(p => p.CompanyId);
            Property(p => p.StartDate);
            Property(p => p.EndDate);
            Property(p => p.IsActive);
        }
    }
}