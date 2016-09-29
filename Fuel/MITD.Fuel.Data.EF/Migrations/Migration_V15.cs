using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(15)]
    public class Migration_V15 : Migration
    {
        public override void Down()
        {
            Delete.Column("CreatedCharterId").FromTable("FuelReport").InSchema(Migration_Initial.FUEL_SCHEMA);
        }

        public override void Up()
        {
            Alter.Table("FuelReport").InSchema(Migration_Initial.FUEL_SCHEMA).AddColumn("CreatedCharterId").AsInt64().Nullable()
                 .ForeignKey("FK_FuelReport_CreatedCharterId_Charter_Id", Migration_Initial.FUEL_SCHEMA, "Charter", "Id");
        }

    }
}
