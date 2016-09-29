using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class FuelReportConfiguration : EntityTypeConfiguration<FuelReport>
    {
        public FuelReportConfiguration()
        {

            HasKey(p => p.Id).ToTable("FuelReport", "Fuel");

            // Properties
            Property(p => p.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.Code).HasMaxLength(200);

            Property(p => p.Description).HasMaxLength(2000);

            Property(p => p.EventDate);

            Property(p => p.ReportDate);

            Property(p => p.FuelReportType);

            Property(p => p.State);
            Ignore(p => p.EntityState);

            Property(p => p.TimeStamp).IsRowVersion();

            // Association:


            HasMany(p => p.FuelReportDetails)
                .WithRequired(p => p.FuelReport)
                .HasForeignKey(p => p.FuelReportId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.Voyage).WithMany().HasForeignKey(p => p.VoyageId);

            HasRequired(p => p.VesselInCompany).WithMany().HasForeignKey(p => p.VesselInCompanyId);

            HasMany(p => p.ConsumptionInventoryOperations).WithOptional(p => p.FuelReport).HasForeignKey(p => p.FuelReport_Id).WillCascadeOnDelete(false); 

            HasOptional(p => p.CreatedCharter).WithMany().HasForeignKey(p => p.CreatedCharterId);
            Property(p => p.HasUpdateRequest);
            Property(p => p.IsTheFirstReport);
            Property(p => p.ActivationCharterContractId);
            HasOptional(p => p.ActivationCharterContract).WithMany().HasForeignKey(p => p.ActivationCharterContractId);
        }
    }
}