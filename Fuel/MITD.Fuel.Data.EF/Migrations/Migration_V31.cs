using System.ComponentModel;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(31)]
    [Description("Adding OrderItemBalance.InventoryOperationId field;")]
    public class Migration_V31 : Migration
    {
        public override void Up()
        {
            Alter.Column("FuelReportDetailId").OnTable("OrderItemBalances").InSchema(Migration_Initial.FUEL_SCHEMA)
                 .AsInt64().Nullable();
        }

        public override void Down()
        {
        }
    }
}
