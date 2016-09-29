using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class FuelReportWorkflowLogConfiguration : EntityTypeConfiguration<FuelReportWorkflowLog>
    {
        public FuelReportWorkflowLogConfiguration()
        {
            HasRequired(c => c.FuelReport).WithMany(c => c.ApproveWorkFlows)
                       .HasForeignKey(c => c.FuelReportId);
        }
    }
}