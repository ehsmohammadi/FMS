using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class OrderApproveWorkFlowConfiguration : EntityTypeConfiguration<OrderWorkflowLog>
    {
        public OrderApproveWorkFlowConfiguration()
        {
            
            HasRequired(c => c.Order).WithMany(c => c.ApproveWorkFlows)
                .HasForeignKey(c=>c.OrderId);


        }
    }
}