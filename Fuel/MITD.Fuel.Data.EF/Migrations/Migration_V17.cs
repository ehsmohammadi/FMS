using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(17)]
    public class Migration_V17 : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Alter.Table("Vouchers").InSchema(Migration_Initial.FUEL_SCHEMA).AlterColumn("ReferenceNo").AsString(512);
            Alter.Table("VoucherLogs").InSchema(Migration_Initial.FUEL_SCHEMA).AlterColumn("RefrenceNo").AsString(512);
        }

    }
}
