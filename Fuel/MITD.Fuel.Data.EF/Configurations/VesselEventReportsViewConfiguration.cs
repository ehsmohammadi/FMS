using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.ReportObjects;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class VesselEventReportsViewConfiguration : EntityTypeConfiguration<VesselEventReportsView>
    {
        public VesselEventReportsViewConfiguration()
        {
            HasKey(p => p.Id).ToTable("VesselEventReportsView", "Report");
        }
    }
}