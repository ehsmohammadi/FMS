using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(20)]
    public class Migration_V20 : Migration
    {
        public override void Up()
        {
            Alter.Column("Quantity").OnTable("InvoiceItems").InSchema(Migration_Initial.FUEL_SCHEMA)
                .AsDecimal(18, 3).NotNullable();


            Alter.Column("Consumption").OnTable("FuelReportDetail").InSchema(Migration_Initial.FUEL_SCHEMA)
                .AsDecimal(18, 3).NotNullable();
            Alter.Column("ROB").OnTable("FuelReportDetail").InSchema(Migration_Initial.FUEL_SCHEMA)
                .AsDecimal(18, 3).NotNullable();
            Alter.Column("Receive").OnTable("FuelReportDetail").InSchema(Migration_Initial.FUEL_SCHEMA)
                .AsDecimal(18, 3).Nullable();
            Alter.Column("Transfer").OnTable("FuelReportDetail").InSchema(Migration_Initial.FUEL_SCHEMA)
                .AsDecimal(18, 3).Nullable();
            Alter.Column("Correction").OnTable("FuelReportDetail").InSchema(Migration_Initial.FUEL_SCHEMA)
                .AsDecimal(18, 3).Nullable();


            Alter.Column("Quantity").OnTable("OrderItems").InSchema(Migration_Initial.FUEL_SCHEMA)
                .AsDecimal(18, 3).NotNullable();
            Alter.Column("InvoicedInMainUnit").OnTable("OrderItems").InSchema(Migration_Initial.FUEL_SCHEMA)
                .AsDecimal(18, 3).NotNullable();
            Alter.Column("ReceivedInMainUnit").OnTable("OrderItems").InSchema(Migration_Initial.FUEL_SCHEMA)
                .AsDecimal(18, 3).NotNullable();


            Alter.Column("QuantityAmountInMainUnit").OnTable("OrderItemBalances").InSchema(Migration_Initial.FUEL_SCHEMA)
                .AsDecimal(18, 3).NotNullable();


            Alter.Column("Rob").OnTable("CharterItem").InSchema(Migration_Initial.FUEL_SCHEMA)
                .AsDecimal(18, 3).NotNullable();


            Alter.Column("Quantity").OnTable("OffhireDetail").InSchema(Migration_Initial.FUEL_SCHEMA)
                .AsDecimal(18, 3).NotNullable();


            Alter.Column("Quantity").OnTable("InvoiceItems").InSchema(Migration_Initial.FUEL_SCHEMA)
                .AsDecimal(18, 3).NotNullable();
        }

        public override void Down()
        {
        }
    }
}
