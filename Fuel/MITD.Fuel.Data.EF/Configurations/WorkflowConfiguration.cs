using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class WorkflowConfiguration : EntityTypeConfiguration<Workflow>
    {
        public WorkflowConfiguration()
        {
            HasKey(p => p.Id).ToTable("Workflow", "Fuel");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(p=>p.Company).WithMany().HasForeignKey(p=>p.CompanyId);
        }
    }
}