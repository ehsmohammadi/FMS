#region

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;

#endregion

namespace MITD.Fuel.Data.EF.Configurations
{
    public class WorkflowStepConfiguration : EntityTypeConfiguration<WorkflowStep>
    {
        public WorkflowStepConfiguration()
        {
            HasKey(p => p.Id).ToTable("WorkflowStep", "Fuel");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(c => c.Workflow).WithMany().HasForeignKey(c => c.WorkflowId);
            HasMany(c => c.ActivityFlows).WithRequired(c => c.WorkflowStep).HasForeignKey(c => c.WorkflowStepId);
        }
    }
}