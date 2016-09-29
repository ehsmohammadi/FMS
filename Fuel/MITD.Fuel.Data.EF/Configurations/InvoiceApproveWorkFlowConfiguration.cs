using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class InvoiceApproveWorkFlowConfiguration : EntityTypeConfiguration<InvoiceWorkflowLog>
    {
        public InvoiceApproveWorkFlowConfiguration()
        {
            
            HasRequired(c => c.Invoice).WithMany(c => c.ApproveWorkFlows)
                .HasForeignKey(c=>c.InvoiceId);


        }
    }
}