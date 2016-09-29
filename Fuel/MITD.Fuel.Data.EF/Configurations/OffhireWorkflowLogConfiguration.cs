using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class OffhireWorkflowLogConfiguration : EntityTypeConfiguration<OffhireWorkflowLog>
    {
        public OffhireWorkflowLogConfiguration()
        {
            HasRequired(c => c.Offhire).WithMany(c => c.ApproveWorkflows).HasForeignKey(c => c.OffhireId);
        }
    }
}