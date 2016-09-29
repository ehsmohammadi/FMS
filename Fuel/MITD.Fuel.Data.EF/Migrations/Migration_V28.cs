
using System.Data;
using FluentMigrator;
using MITD.Fuel.Data.EF.Migrations;

namespace MITD.Data.EF.Migrations
{
    [Migration(28)]
   public class Migration_V28:Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            this.Alter.Table("Vessel").InSchema(Migration_Initial.FUEL_SCHEMA).AlterColumn("Code").AsString(50).Unique().NotNullable();
        }
    }
}
