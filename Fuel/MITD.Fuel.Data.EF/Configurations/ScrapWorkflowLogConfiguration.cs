using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class ScrapWorkflowLogConfiguration : EntityTypeConfiguration<ScrapWorkflowLog>
    {
        public ScrapWorkflowLogConfiguration()
        {
            HasRequired(c => c.Scrap).WithMany(c => c.ApproveWorkflows).HasForeignKey(c => c.ScrapId);
        }
    }
}