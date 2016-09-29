using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    //Adding InventoryOperationId field, FuelReport_Id FK field for EOV,EOM,EOY consumption issues.
    [Migration(4)]
    public class Migration_V4 : Migration
    {
        public override void Up()
        {
            Alter.Table("InventoryOperation").InSchema("Fuel")
                 .AddColumn("FuelReport_Id").AsInt64().Indexed().Nullable()
                    .ForeignKey("FK_InventoryOperation_Id_FuelReport_Id", "Fuel", "FuelReport", "Id").WithDefaultValue((long?) null)
                 .AddColumn("InventoryOperationId").AsInt64().Indexed().NotNullable().WithDefaultValue(-1);
        }

        public override void Down()
        {
            Delete.Index().OnTable("InventoryOperation").InSchema("Fuel").OnColumn("InventoryOperationId");
            Delete.Column("InventoryOperationId").FromTable("InventoryOperation").InSchema("Fuel");

            Delete.ForeignKey("FK_InventoryOperation_Id_FuelReport_Id").OnTable("InventoryOperation").InSchema("Fuel");
            Delete.Index().OnTable("InventoryOperation").InSchema("Fuel").OnColumn("FuelReport_Id");
            Delete.Column("FuelReport_Id").FromTable("InventoryOperation").InSchema("Fuel");
        }
    }
}
