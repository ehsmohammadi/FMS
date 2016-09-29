using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(24)]
    public class Migration_V24 : Migration
    {
        public override void Up()
        {
            //Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Create HAFEZ Rotation Linked Servers.sql");
            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Create HAFEZ Voyages Views.sql");
            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\AlterVoyagesView.V1.sql");
            Create.Table("CharterItemHistory").InSchema(Migration_Initial.FUEL_SCHEMA)
                 .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                 .WithColumn("CharterId").AsInt64().NotNullable().Indexed()
                  .WithColumn("CharterItemId").AsInt64().NotNullable().Indexed()
                 .WithColumn("GoodUnitId").AsInt64().NotNullable().Indexed()
                 .WithColumn("CharterStateId").AsInt32().NotNullable()
                 .WithColumn("GoodId").AsInt64().NotNullable().Indexed()
                 .WithColumn("TankId").AsInt64().Nullable().Indexed()
                 .WithColumn("Rob").AsDecimal(18, 3).NotNullable()
                 .WithColumn("Fee").AsDecimal(18, 3).NotNullable()
                 .WithColumn("OffhireFee").AsDecimal(18, 3).NotNullable()
                 .WithColumn("DateRegisterd").AsDateTime().NotNullable()
                 .WithColumn("TimeStamp").AsCustom("RowVersion");

            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Create HAFEZAccountListView.sql");
            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Alter AccountListView.V1.sql");
        }

        public override void Down()
        {
            //try
            //{
           //    Delete.Table("CharterItemHistory").InSchema(Migration_Initial.FUEL_SCHEMA);
            //}
            //catch (Exception)
            //{

            //}
           
        }
    }
}
