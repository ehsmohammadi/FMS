using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class WorkflowLogConfiguration : EntityTypeConfiguration<WorkflowLog>
    {
        public WorkflowLogConfiguration()
        {
            HasKey(p => p.Id).ToTable("WorkflowLog", "Fuel");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Relations
            HasRequired(c => c.ActorUser).WithMany().HasForeignKey(c => c.ActorUserId).WillCascadeOnDelete(false);
            HasRequired(c => c.CurrentWorkflowStep).WithMany().HasForeignKey(c => c.CurrentWorkflowStepId);
        }
    }
}