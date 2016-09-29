using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;

namespace MITD.Fuel.Data.EF.Configurations
{
   public class CharterApproveWorkFlowConfiguration : EntityTypeConfiguration<CharterWorkflowLog>
    {
       public CharterApproveWorkFlowConfiguration()
        {
          HasRequired(c => c.Charter).WithMany(c => c.ApproveWorkflows).HasForeignKey(c => c.CharterId);
        }
    }
}
