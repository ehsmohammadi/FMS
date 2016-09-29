using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations.BaseInfo
{
    internal class ActivityLocationConfiguration : EntityTypeConfiguration<ActivityLocation>
    {
        public ActivityLocationConfiguration()
        {
            HasKey(p => p.Id).ToTable("ActivityLocation", "BasicInfo");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(p => p.Name).HasMaxLength(200);

            Property(p => p.Code).HasMaxLength(50);

            Property(p => p.Abbreviation).HasMaxLength(50);

            Property(p => p.CountryName).HasMaxLength(200);

            Property(p => p.Latitude);

            Property(p => p.Longitude);
        }

    }
}
