#region

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Data.EF.Configurations
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            HasKey(p => p.Id)
                .ToTable("Order", "Fuel");
            // Properties:

            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.TimeStamp).IsRowVersion();

            // Association:

            HasOptional(o => o.FromVesselInCompany).WithMany().HasForeignKey(o => o.FromVesselInCompanyId).WillCascadeOnDelete(false);
            HasOptional(o => o.ToVesselInCompany).WithMany().HasForeignKey(o => o.ToVesselInCompanyId).WillCascadeOnDelete(false);

            HasOptional(c => c.Transporter).WithMany().HasForeignKey(c => c.TransporterId).WillCascadeOnDelete(false);
            HasOptional(c => c.Receiver).WithMany().HasForeignKey(c => c.ReceiverId).WillCascadeOnDelete(false);
            HasOptional(c => c.Supplier).WithMany().HasForeignKey(c => c.SupplierId).WillCascadeOnDelete(false);
            HasRequired(c => c.Owner).WithMany().HasForeignKey(c => c.OwnerId).WillCascadeOnDelete(false);
           // HasOptional(p => p.CurrentApproveWorkFlowConfig).WithMany().HasForeignKey(p => p.CurrentApproveFlowId).WillCascadeOnDelete(false);


            Ignore(c => c.OrderState);


        }
    }
}