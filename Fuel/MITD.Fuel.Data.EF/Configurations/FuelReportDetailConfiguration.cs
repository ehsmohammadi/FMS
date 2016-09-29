using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class FuelReportDetailConfiguration : EntityTypeConfiguration<FuelReportDetail>
    {
        public FuelReportDetailConfiguration()
        {
            HasKey(p => p.Id).ToTable("FuelReportDetail", "Fuel");
            // Properties:

            Property(p => p.TimeStamp).IsRowVersion();

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.Consumption).HasPrecision(18,3);

            Property(p => p.Correction).HasPrecision(18, 3);

            Property(p => p.IsCorrectionPricingTypeRevised);
            Property(p => p.CorrectionPricingType);

            Property(p => p.CorrectionPrice).HasPrecision(18, 2);
            Property(p => p.CorrectionPriceCurrencyISOCode);

            Property(p => p.CorrectionType);

            Property(p => p.Receive).HasPrecision(18, 3);

            Property(p => p.ROB).HasPrecision(18, 3);

            Property(p => p.ROBUOM);

            Property(p => p.Transfer).HasPrecision(18, 3);

            Property(p => p.FuelReportId);

            Property(p => p.ReceiveType);

            Property(p => p.TransferType);

            Property(p => p.GoodId);

            Property(p => p.MeasuringUnitId);

            Property(p => p.CorrectionPriceCurrencyId);

            Property(p => p.TankId);


            Property(p => p.ReceiveReference.ReferenceId);
            Property(p => p.ReceiveReference.ReferenceType);
            Property(p => p.ReceiveReference.Code);

            Property(p => p.TransferReference.ReferenceId);
            Property(p => p.TransferReference.ReferenceType);
            Property(p => p.TransferReference.Code);

            Property(p => p.CorrectionReference.ReferenceId);
            Property(p => p.CorrectionReference.ReferenceType);
            Property(p => p.CorrectionReference.Code);


            HasRequired(p => p.FuelReport).WithMany(p => p.FuelReportDetails).HasForeignKey(p => p.FuelReportId);

            HasRequired(p => p.Good).WithMany().HasForeignKey(p => p.GoodId);

            HasRequired(p => p.MeasuringUnit).WithMany().HasForeignKey(p => p.MeasuringUnitId);

            HasRequired(p => p.Tank).WithMany().HasForeignKey(p => p.TankId);

            HasOptional(p => p.CorrectionPriceCurrency).WithMany().HasForeignKey(p => p.CorrectionPriceCurrencyId);

            HasMany(p => p.InventoryOperations).WithOptional(p => p.FuelReportDetail).HasForeignKey(p => p.FuelReportDetailId).WillCascadeOnDelete(false); 
        }
    }
}