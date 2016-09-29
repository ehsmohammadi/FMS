
using System.Data;
using FluentMigrator;
using MITD.Fuel.Data.EF.Migrations;

namespace MITD.Data.EF.Migrations
{
    [Migration(29)]
   public class Migration_V29:Migration
    {
        public override void Down()
        {
            this.Delete.Column("TrustIssueInventoryTransactionItemId").FromTable("FuelReportDetail").InSchema(Migration_Initial.FUEL_SCHEMA);
        }

        public override void Up()
        {
            this.Alter.Table("FuelReportDetail").InSchema(Migration_Initial.FUEL_SCHEMA).AddColumn("TrustIssueInventoryTransactionItemId").AsInt64().Nullable();
            this.Alter.Table("FuelReport").InSchema(Migration_Initial.FUEL_SCHEMA).AddColumn("IsTheFirstReport").AsBoolean().WithDefaultValue(false);
            this.Alter.Table("FuelReport").InSchema(Migration_Initial.FUEL_SCHEMA).AddColumn("ActivationCharterContractId").AsInt64().Nullable().
                 ForeignKey("FK_FuelReport_ActivationCharterContractId_Charter_Id", Migration_Initial.FUEL_SCHEMA, "Charter", "Id");
        }
    }
}
