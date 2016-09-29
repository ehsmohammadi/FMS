using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.Configurations
{
    public class ScrapDetailConfiguration : EntityTypeConfiguration<ScrapDetail>
    {
        public ScrapDetailConfiguration()
        {
            HasKey(p => p.Id).ToTable("ScrapDetail", "Fuel");

            Property(p => p.ROB);
            Property(p => p.Price);

            Property(p => p.CurrencyId);
            HasRequired(p => p.Currency).WithMany().HasForeignKey(p => p.CurrencyId).WillCascadeOnDelete(false);

            Property(p => p.GoodId);
            HasRequired(p => p.Good).WithMany().HasForeignKey(p => p.GoodId).WillCascadeOnDelete(false);

            Property(p => p.UnitId);
            HasRequired(p => p.Unit).WithMany().HasForeignKey(p => p.UnitId).WillCascadeOnDelete(false);

            Property(p => p.TankId).IsOptional();
            HasOptional(p => p.Tank).WithMany().HasForeignKey(p => p.TankId).WillCascadeOnDelete(false);

            Property(p => p.ScrapId).IsRequired();
            HasRequired(p => p.Scrap).WithMany(s => s.ScrapDetails).HasForeignKey(p => p.ScrapId).WillCascadeOnDelete(false);
        }
    }
}