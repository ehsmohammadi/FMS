using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class OffhireDetailConfiguration : EntityTypeConfiguration<OffhireDetail>
    {
        public OffhireDetailConfiguration()
        {
            HasKey(p => p.Id).ToTable("OffhireDetail", "Fuel");

            Property(p => p.Quantity).HasPrecision(18, 3); ;
            Property(p => p.FeeInVoucherCurrency).HasPrecision(18, 2);
            Property(p => p.FeeInMainCurrency).HasPrecision(18, 2);

            Property(p => p.GoodId);
            HasRequired(p => p.Good).WithMany().HasForeignKey(p => p.GoodId).WillCascadeOnDelete(false);

            Property(p => p.UnitId);
            HasRequired(p => p.Unit).WithMany().HasForeignKey(p => p.UnitId).WillCascadeOnDelete(false);

            Property(p => p.TankId).IsOptional();
            HasOptional(p => p.Tank).WithMany().HasForeignKey(p => p.TankId).WillCascadeOnDelete(false);

            Property(p => p.OffhireId);
            HasRequired(p => p.Offhire).WithMany(o => o.OffhireDetails).HasForeignKey(p => p.OffhireId).WillCascadeOnDelete(false);

            Property(p => p.TimeStamp).IsRowVersion();
        }
    }
}