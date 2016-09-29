using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(3)]
    [Description("Adding OrderItemBalance.InventoryOperationId field;")]
    public class Migration_V3 : Migration
    {
        public override void Up()
        {
            Alter.Table("OrderItemBalances").InSchema("Fuel")
                 .AddColumn("InventoryOperationId").AsInt64().Indexed().Nullable()
                 .ForeignKey("FK_OrderItemBalances_InventoryOperationId_InventoryOperation_Id", "Fuel", "InventoryOperation", "Id").WithDefaultValue((long?) null);
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_OrderItemBalances_InventoryOperationId_InventoryOperation_Id").OnTable("OrderItemBalances").InSchema("Fuel");
            Delete.Index().OnTable("OrderItemBalances").InSchema("Fuel").OnColumn("InventoryOperationId");
            Delete.Column("InventoryOperationId").FromTable("OrderItemBalances").InSchema("Fuel");
        }
    }
}
