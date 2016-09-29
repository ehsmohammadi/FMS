using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(19)]
    public class Migration_V19 : Migration
    {
        public override void Up()
        {
            Alter.Table("OrderItemBalances").InSchema(Migration_Initial.FUEL_SCHEMA)
                 .AddColumn("PairingInvoiceItemId").AsInt64().Nullable()
                 .ForeignKey("FK_OrderItemBalances_PairingInvoiceItemId_InvoiceItems_Id", Migration_Initial.FUEL_SCHEMA, "InvoiceItems", "Id");
        }

        public override void Down()
        {
            Delete.Column("PairingInvoiceItemId").FromTable("OrderItemBalances").InSchema(Migration_Initial.FUEL_SCHEMA);
        }
    }
}
