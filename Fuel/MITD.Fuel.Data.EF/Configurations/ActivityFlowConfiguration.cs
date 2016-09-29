#region

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Data.EF.Extensions;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;

#endregion

namespace MITD.Fuel.Data.EF.Configurations
{
    public class ActivityFlowConfiguration : EntityTypeConfiguration<ActivityFlow>
    {
        public ActivityFlowConfiguration()
        {
            HasKey(p => p.Id).ToTable("ActivityFlow", "Fuel");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(c => c.WorkflowStep).WithMany(c=>c.ActivityFlows).HasForeignKey(c => c.WorkflowStepId).WillCascadeOnDelete(false);
            HasRequired(c => c.WorkflowNextStep).WithMany().HasForeignKey(c => c.WorkflowNextStepId).WillCascadeOnDelete(false);
            HasRequired(c => c.ActionType).WithMany().HasForeignKey(c => c.ActionTypeId);

            // Unique Constraint
            Property(p => p.WorkflowStepId).IsUnique("UC_WorkflowStep_ActionType", 1);
            Property(p => p.ActionTypeId).IsUnique("UC_WorkflowStep_ActionType", 2);
        }
    }
}