

#region

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Data.EF.Configurations
{
    public class VoyageLogConfiguration : EntityTypeConfiguration<VoyageLog>
    {
        public VoyageLogConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("VoyageLog", "Fuel");

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.ChangeDate);

            Property(p => p.VoyageNumber).HasMaxLength(200);

            Property(p => p.Description).HasMaxLength(200);


            Property(p => p.StartDate);
            Property(p => p.EndDate);
            Property(p => p.IsActive);

            HasRequired(p => p.Company).WithMany().HasForeignKey(p=>p.CompanyId);
            HasRequired(p => p.VesselInCompany).WithMany().HasForeignKey(p=>p.VesselInCompanyId);

            //Map(p => p.Properties(vl => new { vl.Company.Id, vl.Company.Code, vl.Company.Name })).ToTable("VoyageLog", "Fuel");
            //Map(p => p.Properties(vl => new { vl.Vessel.Id, vl.Vessel.Code, vl.Vessel.Name })).ToTable("VoyageLog", "Fuel"); 

            //Map(p => p.Properties(vl => new { Company_Id = vl.Company.Id, Company_Code = vl.Company.Code, Company_Name = vl.Company.Name }));
            //Map(p => p.Properties(vl => new { Vessel_Id = vl.Vessel.Id, Vessel_Code = vl.Vessel.Code, Vessel_Name = vl.Vessel.Name }));


            HasRequired(v => v.ReferencedVoyage).WithMany().HasForeignKey(o => o.ReferencedVoyageId).WillCascadeOnDelete(false);
        }
    }
}