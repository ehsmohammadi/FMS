using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations.BaseInfo
{
    public class GoodPartyAssignmentConfiguration : EntityTypeConfiguration<GoodPartyAssignment>
    {
        public GoodPartyAssignmentConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("GoodPartyAssignmentView", "BasicInfo");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}